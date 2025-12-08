using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 库存显示抽象基类，用于管理游戏中的库存系统UI显示
/// 继承自MonoBehaviour，提供库存系统的可视化界面基础功能
/// </summary>
public abstract class InventoryDisplay : MonoBehaviour
{
    [SerializeField] private MouseItemData mouseInventoryItem;
    
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlotUI, InventorySlot> slotDictionary;
    
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
    public abstract void AssignSlot(InventorySystem invToDisplay);

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
    /// 处理库存槽位被点击的事件
    /// </summary>
    /// <param name="clickedUISlot">被点击的UI槽位组件</param>
    public void SlotClicked(InventorySlotUI clickedUISlot)
    {
        // 点击的槽位有物品且鼠标没有物品时，拾取槽位中的物品
        if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.assignedInventorySlot.ItemData == null)
        {
            // 如果玩家按住shift键？分割堆叠。

            mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
            clickedUISlot.ClearSlot();
            return;
        }
        
        // 点击的槽位为空且鼠标有物品时，将鼠标上的物品放入该槽位
        if (clickedUISlot.AssignedInventorySlot.ItemData == null &&
            mouseInventoryItem.assignedInventorySlot.ItemData != null)
        {
            clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.assignedInventorySlot);
            clickedUISlot.UpdateUISlot();
            
            mouseInventoryItem.ClearSlot();
        }
        
        
        // 两个槽位都有物品的情况，需要进一步判断如何处理
            // 两个物品是否相同？如果相同则合并它们。
                // 槽位堆叠数量 + 鼠标堆叠数量 > 槽位最大堆叠数量？如果是，则从鼠标取出。
            // 如果物品不同，则交换物品。
    }
}