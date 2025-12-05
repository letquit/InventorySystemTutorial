using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 将指定数量的物品添加到库存中
    /// </summary>
    /// <param name="itemToAdd">要添加的物品数据</param>
    /// <param name="amountToAdd">要添加的物品数量</param>
    /// <returns>添加成功返回true，否则返回false</returns>
    public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
    {
        // 检查库存中是否已经存在该物品
        if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot))
        {
            foreach (var slot in invSlot)
            {
                if (slot.RoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }
        
        // 检查库存中是否有空槽位
        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
            OnInventorySlotChanged?.Invoke(freeSlot);
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// 检查库存中是否包含指定的物品
    /// </summary>
    /// <param name="itemToAdd">要检查的物品数据</param>
    /// <param name="invSlot">返回包含该物品的所有槽位列表</param>
    /// <returns>如果库存中包含该物品则返回true，否则返回false</returns>
    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
    {
        invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();

        return invSlot.Count > 0;
    }

    /// <summary>
    /// 检查库存中是否有空闲的槽位
    /// </summary>
    /// <param name="freeSlot">返回第一个空闲的槽位</param>
    /// <returns>如果有空闲槽位则返回true，否则返回false</returns>
    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null);
        return freeSlot != null;
    }
}