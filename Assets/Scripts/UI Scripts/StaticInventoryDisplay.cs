using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 静态库存显示类，用于在UI上显示固定的库存槽位
/// 继承自InventoryDisplay基类，负责初始化和管理静态库存的UI显示
/// </summary>
public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] private InventorySlotUI[] slots;
    
    /// <summary>
    /// 初始化组件，在Start阶段设置库存系统并绑定事件监听器
    /// 调用基类Start方法，然后初始化库存系统引用并注册库存槽位变化事件
    /// 最后调用AssignSlot方法分配槽位
    /// </summary>
    protected override void Start()
    {
        base.Start();

        // 检查是否有分配的库存持有者，如果有则设置库存系统并绑定事件
        if (inventoryHolder != null)
        {
            inventorySystem = inventoryHolder.InventorySystem;
            inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else Debug.LogWarning($"No inventory assigned to {this.gameObject}");
        
        AssignSlot(inventorySystem);
    } 
    
    /// <summary>
    /// 分配库存槽位到UI元素，建立UI槽位与库存槽位的映射关系
    /// 创建slotDictionary字典，并为每个UI槽位初始化对应的库存槽位数据
    /// </summary>
    /// <param name="invToDisplay">要显示的库存系统实例</param>
    public override void AssignSlot(InventorySystem invToDisplay)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

        // 检查UI槽位数量是否与库存系统大小匹配
        if (slots.Length != inventorySystem.InventorySize) Debug.Log($"Inventory slots out of sync on {this.gameObject}");
        
        // 遍历所有库存槽位，建立UI槽位与库存槽位的映射关系并初始化UI槽位
        for (int i = 0; i < inventorySystem.InventorySize; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
}