using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 库存系统类，用于管理游戏中的物品库存
/// </summary>
[Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    /// <summary>
    /// 初始化库存系统
    /// </summary>
    /// <param name="size">库存槽位的数量</param>
    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        // 初始化指定数量的空库存槽位
        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    /// <summary>
    /// 向库存中添加物品
    /// </summary>
    /// <param name="itemToAdd">要添加的物品数据</param>
    /// <param name="amountToAdd">要添加的物品数量</param>
    /// <returns>添加成功返回true，否则返回false</returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        inventorySlots[0] = new InventorySlot(itemToAdd, amountToAdd);
        return true;
    }
}