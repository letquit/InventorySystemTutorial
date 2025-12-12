using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 静态库存显示类，用于显示固定位置的库存界面
/// 继承自InventoryDisplay基类，负责管理静态库存UI的显示和更新
/// </summary>
public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlotUI[] slots;

    /// <summary>
    /// 当组件启用时注册库存变化事件监听器
    /// </summary>
    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }
    
    /// <summary>
    /// 当组件禁用时注销库存变化事件监听器
    /// 防止内存泄漏和无效引用
    /// </summary>
    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    /// <summary>
    /// 刷新静态库存显示界面
    /// 重新绑定库存系统并更新UI槽位显示
    /// </summary>
    private void RefreshStaticDisplay()
    {
        // 检查是否有分配的库存持有者
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.PrimaryInventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory assigned to {this.gameObject}");
        
        // 从索引0开始分配槽位
        AssignSlot(inventorySystem, 0);
    }

    /// <summary>
    /// 组件启动时的初始化方法
    /// 调用基类Start方法并刷新显示界面
    /// </summary>
    protected override void Start()
    {
        base.Start();

        RefreshStaticDisplay();
    } 
    
    /// <summary>
    /// 为库存系统分配UI槽位
    /// 建立UI槽位与库存槽位之间的映射关系
    /// </summary>
    /// <param name="invToDisplay">要显示的库存系统</param>
    /// <param name="offset">槽位偏移量</param>
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();
        
        // 根据库存持有者的偏移值创建槽位映射
        for (int i = 0; i < inventoryHolder.Offset; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
}