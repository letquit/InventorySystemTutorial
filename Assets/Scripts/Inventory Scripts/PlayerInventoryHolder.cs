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
    /// <summary>
    /// 当玩家库存发生变化时触发的事件回调
    /// </summary>
    public static UnityAction OnPlayerInventoryChanged;

    /// <summary>
    /// 当玩家请求显示库存界面时触发的事件回调
    /// </summary>
    /// <param name="inventorySystem">库存系统实例，包含玩家的物品信息</param>
    /// <param name="playerId">玩家ID，用于标识哪个玩家请求显示库存</param>
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;
    
    /// <summary>
    /// 初始化玩家库存数据，在游戏开始时将当前主库存系统保存到存档管理器中
    /// </summary>
    private void Start()
    {
        SaveGameManager.Data.PlayerInventory = new InventorySaveData(primaryInventorySystem);
    }

    /// <summary>
    /// 从存档数据中加载玩家库存信息
    /// </summary>
    /// <param name="data">包含玩家库存数据的存档数据对象</param>
    protected override void LoadInventory(SaveData data)
    {
        // 检查存档中的库存系统数据是否存在
        if (data.PlayerInventory.invSystem != null)
        {
            this.primaryInventorySystem = data.PlayerInventory.invSystem;
            // 触发库存变更事件
            OnPlayerInventoryChanged?.Invoke();
        }
    }
    
    /// <summary>
    /// 每帧检测玩家输入，当按下B键时触发动态库存显示请求事件
    /// </summary>
    private void Update()
    {
        // 检测B键是否在当前帧被按下
        if (Keyboard.current.bKey.wasPressedThisFrame)
            OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
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
        
        // 两个背包都无法添加时返回false
        return false;
    }
}