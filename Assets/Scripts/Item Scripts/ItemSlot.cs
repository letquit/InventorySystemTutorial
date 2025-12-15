using System;
using UnityEngine;

/// <summary>
/// 表示一个物品槽位的抽象基类，实现序列化回调接口。
/// 提供了物品数据、ID 和堆叠数量的基本管理功能，并支持清空、分配和修改堆叠等操作。
/// </summary>
public abstract class ItemSlot : ISerializationCallbackReceiver
{
    /// <summary>
    /// 物品数据引用，用于存储物品的详细信息
    /// </summary>
    [NonSerialized] protected InventoryItemData itemData;

    /// <summary>
    /// 物品唯一标识符
    /// </summary>
    [SerializeField] protected int itemID;

    /// <summary>
    /// 当前物品堆叠数量
    /// </summary>
    [SerializeField] protected int stackSize;
    
    /// <summary>
    /// 获取当前槽位中的物品数据
    /// </summary>
    public InventoryItemData ItemData => itemData;

    /// <summary>
    /// 获取当前槽位中物品的堆叠数量
    /// </summary>
    public int StackSize => stackSize;
    
    /// <summary>
    /// 清空物品槽位的数据
    /// </summary>
    /// <remarks>
    /// 将物品槽位的所有数据重置为初始状态，包括物品数据、物品ID和堆叠数量
    /// </remarks>
    public void ClearSlot()
    {
        itemData = null;
        itemID = -1;
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
            itemID = itemData.id;
            stackSize = 0;
            AddToStack(invSlot.stackSize);
        }
    }

    /// <summary>
    /// 使用指定的物品数据和数量来分配到当前槽位
    /// </summary>
    /// <param name="data">要分配的物品数据</param>
    /// <param name="amount">要分配的数量</param>
    public void AssignItem(InventoryItemData data, int amount)
    {
        if (itemData == data) AddToStack(amount);
        else
        {
            itemData = data;
            itemID = data.id;
            stackSize = 0;
            AddToStack(amount);
        }
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
        // 如果堆叠数量小于等于零，则清空槽位
        if (stackSize <= 0) ClearSlot();
    }
    
    /// <summary>
    /// 在序列化之前调用的回调方法（Unity 序列化流程的一部分）
    /// </summary>
    public void OnBeforeSerialize()
    {
        
    }

    /// <summary>
    /// 在反序列化之后调用的回调方法，用于加载物品数据
    /// </summary>
    public void OnAfterDeserialize()
    {
        // 如果物品ID无效则直接返回
        if (itemID == -1) return;
        
        // 从资源中加载数据库并获取对应的物品数据
        var db = Resources.Load<Database>("Database");
        itemData = db.GetItem(itemID);
    }
}