using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 动态库存显示类，用于动态创建和管理库存UI槽位
/// 继承自InventoryDisplay基类，提供运行时动态刷新库存显示的功能
/// </summary>
public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlotUI slotPrefab;
    
    /// <summary>
    /// 初始化方法，在对象启用时调用
    /// 调用基类的Start方法进行基础初始化
    /// </summary>
    protected override void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 刷新动态库存显示
    /// 清除现有槽位并重新分配新的库存系统数据
    /// </summary>
    /// <param name="invToDisplay">要显示的库存系统对象</param>
    /// <param name="offset">库存槽位分配的起始偏移量</param>
    public void RefreshDynamicInventory(InventorySystem invToDisplay, int offset)
    {
        // 清除所有现有槽位
        ClearSlots();
        
        // 设置新的库存系统并订阅槽位变化事件
        inventorySystem = invToDisplay;
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged += UpdateSlot;
        
        AssignSlot(invToDisplay, offset);
    }

    /// <summary>
    /// 分配并初始化库存槽位UI
    /// 根据传入的库存系统创建对应数量的UI槽位
    /// </summary>
    /// <param name="invToDisplay">要显示的库存系统对象</param>
    /// <param name="offset">库存槽位分配的起始偏移量</param>
    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();
        
        if (invToDisplay == null) return;

        // 从指定偏移量开始创建库存槽位UI
        for (int i = offset; i < invToDisplay.InventorySize; i++)
        {
            var uiSlot = Instantiate(slotPrefab, transform);
            slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);
            uiSlot.Init(invToDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }
    }

    /// <summary>
    /// 清除所有已存在的槽位UI元素
    /// 销毁当前所有的子对象并清空槽位字典
    /// </summary>
    private void ClearSlots()
    {
        // 遍历并销毁所有子对象
        foreach (var item in transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
        
        // 清空槽位字典
        if (slotDictionary != null) slotDictionary.Clear();
    }

    /// <summary>
    /// 当对象被禁用时调用此方法，用于清理事件监听器
    /// </summary>
    private void OnDisable()
    {
        // 移除库存槽位变化事件的监听，确保对象禁用后不会继续响应事件
        if (inventorySystem != null) inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }
}