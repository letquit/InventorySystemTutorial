using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 库存显示抽象基类，用于管理游戏中的库存系统UI显示
/// 继承自MonoBehaviour，提供库存系统的可视化界面基础功能
/// </summary>
public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;
    
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlotUI, InventorySlot> slotDictionary;    // 将UI槽位与系统槽位配对
    
    /// <summary>
    /// 获取当前库存系统实例的属性
    /// </summary>
    public InventorySystem InventorySystem => inventorySystem;
    
    /// <summary>
    /// 获取槽位字典的属性，键为UI槽位组件，值为数据槽位
    /// </summary>
    public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => slotDictionary;

    /// <summary>
    /// Unity生命周期方法，在对象启用时调用
    /// 虚拟方法可供子类重写以添加初始化逻辑
    /// </summary>
    protected virtual void Start()
    {
        
    }
    
    /// <summary>
    /// 抽象方法，用于分配和初始化库存槽位
    /// 必须在子类中实现具体的槽位分配逻辑
    /// </summary>
    /// <param name="invToDisplay">需要显示的库存系统实例</param>
    /// <param name="offset">槽位偏移量，用于确定槽位的起始位置</param>
    public abstract void AssignSlot(InventorySystem invToDisplay, int offset);

    /// <summary>
    /// 更新指定库存槽位的UI显示
    /// 遍历槽位字典找到对应的UI槽位并更新其显示内容
    /// </summary>
    /// <param name="updatedSlot">需要更新的库存槽位数据</param>
    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        // 遍历所有槽位字典项，查找需要更新的槽位并刷新UI
        foreach (var slot in SlotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    /// <summary>
    /// 处理库存槽位被点击的事件。根据鼠标状态与 Shift 键状态执行不同的操作：
    /// 包括拾取物品、放置物品、拆分堆叠、合并堆叠或交换物品等逻辑。
    /// </summary>
    /// <param name="clickedUISlot">用户点击的 UI 槽位组件实例。</param>
    public void SlotClicked(InventorySlotUI clickedUISlot)
    {
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

        // 当前槽位有物品，而鼠标上无物品时：尝试拾取该物品
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.assignedInventorySlot.ItemData == null)
        {
            // 如果按下 Shift 键并且成功分割堆叠，则只拾取一半数量
            if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot))
            {
                mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                clickedUISlot.UpdateUISlot();
                return;
            }
            else
            {
                // 否则直接拾取整个槽位的物品
                mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();
                return;
            }
        }

        // 当前槽位为空，但鼠标上有物品时：将鼠标上的物品放入当前槽位
        if (clickedUISlot.AssignedInventorySlot.ItemData == null &&
            mouseInventoryItem.assignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.assignedInventorySlot);
            clickedUISlot.UpdateUISlot();

            mouseInventoryItem.ClearSlot();
            return;
        }

        // 两个槽位都存在物品的情况下进行进一步判断处理
        if (clickedUISlot.AssignedInventorySlot.ItemData != null &&
            mouseInventoryItem.assignedInventorySlot.ItemData != null)
        {
            bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData ==
                              mouseInventoryItem.assignedInventorySlot.ItemData;

            // 物品相同，并且目标槽位还有空间容纳鼠标上的物品数量
            if (isSameItem && clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.assignedInventorySlot.StackSize))
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.assignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
                return;
            }
            // 目标槽位无法完全容纳鼠标上的物品数量
            else if (isSameItem &&
                     !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(
                         mouseInventoryItem.assignedInventorySlot.StackSize, out int leftInStack))
            {
                if (leftInStack < 1)
                {
                    // 堆叠已满，交换两个槽位中的物品
                    SwapSlots(clickedUISlot);
                }
                else
                {
                    // 将部分物品加入目标槽位，剩余留在鼠标上
                    int remainingOnMouse = mouseInventoryItem.assignedInventorySlot.StackSize - leftInStack;
                    
                    clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                    clickedUISlot.UpdateUISlot();

                    var newItem = new InventorySlot(mouseInventoryItem.assignedInventorySlot.ItemData,
                        remainingOnMouse);
                    mouseInventoryItem.ClearSlot();
                    mouseInventoryItem.UpdateMouseSlot(newItem);
                    return;
                }
            }
            // 物品类型不同，直接交换两个槽位的内容
            else if (!isSameItem)
            {
                SwapSlots(clickedUISlot);
                return;
            }
        }
    }

    /// <summary>
    /// 在鼠标和指定槽位之间交换物品数据。
    /// 先保存鼠标上的物品信息，清空鼠标后设置为目标槽位的数据，
    /// 再把之前保存的信息赋给目标槽位并刷新显示。
    /// </summary>
    /// <param name="clickedUISlot">要与其交换物品的目标 UI 槽位。</param>
    private void SwapSlots(InventorySlotUI clickedUISlot)
    {
        var clonedSlot = new InventorySlot(mouseInventoryItem.assignedInventorySlot.ItemData,
            mouseInventoryItem.assignedInventorySlot.StackSize);
        mouseInventoryItem.ClearSlot();

        mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

        // 清除原槽位以避免在 AssignItem 中发生叠加异常
        clickedUISlot.ClearSlot();
        clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
        clickedUISlot.UpdateUISlot();
    }
}