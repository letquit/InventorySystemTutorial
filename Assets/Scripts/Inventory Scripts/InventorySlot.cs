using System;
using UnityEngine;

/// <summary>
/// 库存槽位类，用于表示库存系统中的单个物品槽位
/// 包含物品数据和堆叠数量信息
/// </summary>
[Serializable]
public class InventorySlot : ISerializationCallbackReceiver
{
    /// <summary>
    /// 物品数据引用，用于存储物品的详细信息
    /// </summary>
    [NonSerialized] private InventoryItemData itemData;

    /// <summary>
    /// 物品唯一标识符
    /// </summary>
    [SerializeField] private int itemID;

    /// <summary>
    /// 当前物品堆叠数量
    /// </summary>
    [SerializeField] private int stackSize;
    
    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;

    /// <summary>
    /// 初始化库存槽位对象
    /// </summary>
    /// <param name="source">库存物品数据源，用于初始化槽位的物品信息</param>
    /// <param name="amount">物品数量，表示该槽位中物品的堆叠大小</param>
    public InventorySlot(InventoryItemData source, int amount)
    {
        // 初始化库存槽位的基本属性
        itemData = source;
        itemID = itemData.id;
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
    /// 更新库存槽位的信息
    /// </summary>
    /// <param name="data">要设置的物品数据</param>
    /// <param name="amount">要设置的堆叠数量</param>
    public void UpdateInventorySlot(InventoryItemData data, int amount)
    {
        // 更新物品数据和ID
        itemData = data;
        itemID = itemData.id;
        // 设置堆叠数量
        stackSize = amount;
    }

    /// <summary>
    /// 检查向当前堆叠中添加指定数量物品后是否还有剩余空间，并返回可添加的数量
    /// </summary>
    /// <param name="amountToAdd">要添加的物品数量</param>
    /// <param name="amountRemaining">还可添加的物品数量</param>
    /// <returns>如果可以添加指定数量的物品则返回true，否则返回false</returns>
    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = itemData.maxStackSize - stackSize;
        
        return EnoughRoomLeftInStack(amountToAdd);
    }
    
    /// <summary>
    /// 检查向当前堆叠中添加指定数量物品是否会超出最大堆叠限制
    /// </summary>
    /// <param name="amountToAdd">要添加的物品数量</param>
    /// <returns>如果添加后不超过最大堆叠数则返回true，否则返回false</returns>
    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (itemData == null || itemData != null && stackSize + amountToAdd <= itemData.maxStackSize) return true;
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
        
        // 创建新的物品槽位,堆叠数量为原来的一半。并返回
        splitStack = new InventorySlot(itemData, halfStack);

        return true;
    }

    /// <summary>
    /// 在序列化之前调用的回调方法
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