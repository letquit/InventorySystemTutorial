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
    [SerializeField] private int gold;
    
    public int Gold => gold;
    
    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    /// <summary>
    /// 初始化库存系统
    /// </summary>
    /// <param name="size">库存槽位的数量</param>
    public InventorySystem(int size)
    {
        gold = 0;
        CreateInventory(size);
    }

    /// <summary>
    /// 初始化库存系统并设置初始金币数量
    /// </summary>
    /// <param name="size">库存槽位的数量</param>
    /// <param name="gold">初始金币数量</param>
    public InventorySystem(int size, int gold)
    {
        this.gold = gold;
        CreateInventory(size);
    }

    /// <summary>
    /// 创建指定数量的库存槽位
    /// </summary>
    /// <param name="size">要创建的库存槽位数量</param>
    private void CreateInventory(int size)
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
                if (slot.EnoughRoomLeftInStack(amountToAdd))
                {
                    slot.AddToStack(amountToAdd);
                    OnInventorySlotChanged?.Invoke(slot);
                    return true;
                }
            }
        }
        
        // 这实际上并没有考虑到最大栈大小。制作这个视频时我非常疲惫，哈哈。我会在后面的视频中修复这个问题——不过暂时保留这段代码也无妨。
        // 检查库存中是否有空槽位
        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            // 如果空槽位有足够的空间，则更新槽位的物品数据
            if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
            // 添加实现：只取能够填满堆栈的数量，并检查是否有另一个空槽位来放置剩余物品。
        }
        
        return false;
    }

    /// <summary>
    /// 检查库存中是否包含指定的物品
    /// </summary>
    /// <param name="itemToAdd">要检查的物品数据</param>
    /// <param name="invSlot">返回包含该物品的所有槽位列表</param>
    /// <returns>如果库存中包含该物品则返回true,获取所有这些槽位的列表，否则返回false</returns>
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