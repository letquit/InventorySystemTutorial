using System;
using UnityEngine;

/// <summary>
/// 库存槽位类，用于表示库存系统中的单个物品槽位
/// 包含物品数据和堆叠数量信息
/// </summary>
[Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;
    
    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;

    /// <summary>
    /// 构造函数，使用指定的物品数据和数量初始化库存槽位
    /// </summary>
    /// <param name="source">要设置的物品数据</param>
    /// <param name="amount">初始堆叠数量</param>
    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData = source;
        stackSize = amount;
    }

    /// <summary>
    /// 默认构造函数，创建一个空的库存槽位
    /// </summary>
    public InventorySlot()
    {
        ClearSlot();
    }

    /// <summary>
    /// 清空当前槽位，将物品数据设为null，堆叠数量设为-1
    /// </summary>
    public void ClearSlot()
    {
        itemData = null;
        stackSize = -1;
    }

    /// <summary>
    /// 将指定库存槽位的物品分配给当前实例
    /// </summary>
    /// <param name="invSlot">要分配物品的库存槽位</param>
    public void AssignItem(InventorySlot invSlot)
    {
        // 如果当前物品数据与目标槽位的物品数据相同，则直接增加堆叠数量
        if (itemData == invSlot.itemData) AddToStack(invSlot.stackSize);
        else
        {
            // 如果物品数据不同，则替换当前物品数据并重新设置堆叠数量
            itemData = invSlot.itemData;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }

    /// <summary>
    /// 更新库存槽位中的物品数据和数量
    /// </summary>
    /// <param name="data">要设置的库存物品数据</param>
    /// <param name="amount">要设置的物品堆叠数量</param>
    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        itemData = data;
        stackSize = amount;
    }

    /// <summary>
    /// 检查向当前堆叠中添加指定数量物品后是否还有剩余空间，并返回可添加的数量
    /// </summary>
    /// <param name="amountToAdd">要添加的物品数量</param>
    /// <param name="amountRemaining">还可添加的物品数量</param>
    /// <returns>如果可以添加指定数量的物品则返回true，否则返回false</returns>
    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = itemData.maxStackSize - stackSize;
        
        return RoomLeftInStack(amountToAdd);
    }
    
    /// <summary>
    /// 检查向当前堆叠中添加指定数量物品是否会超出最大堆叠限制
    /// </summary>
    /// <param name="amountToAdd">要添加的物品数量</param>
    /// <returns>如果添加后不超过最大堆叠数则返回true，否则返回false</returns>
    public bool RoomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= itemData.maxStackSize) return true;
        else return false;
    }

    /// <summary>
    /// 向当前堆叠中添加指定数量的物品
    /// </summary>
    /// <param name="amount">要添加的物品数量</param>
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    /// <summary>
    /// 从当前堆叠中移除指定数量的物品
    /// </summary>
    /// <param name="amount">要移除的物品数量</param>
    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    /// <summary>
    /// 将当前物品堆栈分成两半，创建一个新的InventorySlot对象表示分割出的一半
    /// </summary>
    /// <param name="splitStack">输出参数，返回分割出的一半物品堆栈，如果分割失败则为null</param>
    /// <returns>如果成功分割堆栈则返回true，否则返回false</returns>
    public bool SplitStack(out InventorySlot splitStack)
    {
        // 检查堆栈大小是否足够分割
        if (stackSize <= 1)
        {
            splitStack = null;
            return false;
        }
        
        // 计算一半的堆栈数量并执行分割
        int halfStack = Mathf.RoundToInt(stackSize / 2f);
        RemoveFromStack(halfStack);
        
        // 创建新的物品槽位并返回
        splitStack = new InventorySlot(itemData, halfStack);
        return true;
    }
}