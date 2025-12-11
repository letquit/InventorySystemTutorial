using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// 玩家背包持有者类，继承自InventoryHolder
/// 管理玩家的快捷栏槽位和背包槽位两个背包系统
/// </summary>
public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] protected int secondaryInventorySize;
    // 背包槽位系统
    [SerializeField] protected InventorySystem secondInventorySystem;
    
    public InventorySystem SecondInventorySystem => secondInventorySystem;
    
    public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;
    
    /// <summary>
    /// 在对象唤醒时初始化，创建背包槽位系统
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        secondInventorySystem = new InventorySystem(secondaryInventorySize);
    }

    /// <summary>
    /// 每帧检查键盘输入，当按下B键时触发背包显示事件
    /// </summary>
    private void Update()
    {
        // 检测B键是否被按下，如果按下则调用背包显示事件
        if (Keyboard.current.bKey.wasPressedThisFrame) OnPlayerBackpackDisplayRequested?.Invoke(secondInventorySystem);
    }

    /// <summary>
    /// 尝试将指定物品添加到玩家背包中
    /// 优先添加到快捷栏槽位，如果快捷栏槽位已满则尝试添加到背包槽位
    /// </summary>
    /// <param name="data">要添加的物品数据</param>
    /// <param name="amount">要添加的物品数量</param>
    /// <returns>添加成功返回true，否则返回false</returns>
    public bool AddToInventory(InventoryItemData data, int amount)
    {
        // 首先尝试添加到快捷栏槽位
        if (primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        // 如果快捷栏槽位添加失败，尝试添加到背包槽位
        else if (secondInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }
        
        // 两个背包都无法添加时返回false
        return false;
    }
}