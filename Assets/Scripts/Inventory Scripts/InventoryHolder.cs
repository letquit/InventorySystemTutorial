using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 库存持有者类，用于管理游戏对象的库存系统
/// 该类继承自MonoBehaviour，可以作为Unity组件附加到游戏对象上
/// </summary>
[Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected InventorySystem inventorySystem;
    
    /// <summary>
    /// 获取当前的库存系统实例
    /// </summary>
    public InventorySystem InventorySystem => inventorySystem;

    /// <summary>
    /// 当动态库存显示被请求时触发的Unity事件
    /// 其他组件可以通过订阅此事件来响应库存显示请求
    /// </summary>
    public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

    /// <summary>
    /// 在对象唤醒时初始化库存系统
    /// 根据设定的库存大小创建新的库存系统实例
    /// </summary>
    private void Awake()
    {
        inventorySystem = new InventorySystem(inventorySize);
    }
}