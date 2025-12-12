using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// 库存持有者类，用于管理游戏对象的库存系统
/// 该类继承自MonoBehaviour，可以作为Unity组件附加到游戏对象上
/// </summary>
[Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    // 快捷栏槽位
    [SerializeField] protected InventorySystem primaryInventorySystem;
    
    [SerializeField] protected int offset = 10;
    
    /// <summary>
    /// 获取显示偏移量的属性
    /// </summary>
    public int Offset => offset;
    
    /// <summary>
    /// 获取当前的库存系统实例
    /// </summary>
    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;

    /// <summary>
    /// 当请求显示动态库存界面时触发的事件回调
    /// </summary>
    /// <param name="inventorySystem">需要显示的库存系统实例</param>
    /// <param name="offset">显示偏移量，用于调整界面显示的位置或数量</param>
    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested;


    /// <summary>
    /// 在对象唤醒时初始化库存系统
    /// 根据设定的库存大小创建新的库存系统实例
    /// 同时注册加载游戏时的库存数据恢复逻辑
    /// </summary>
    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory;
        
        primaryInventorySystem = new InventorySystem(inventorySize);
    }

    /// <summary>
    /// 抽象方法：从保存的数据中加载库存信息
    /// 子类必须实现此方法以支持库存系统的持久化加载
    /// </summary>
    /// <param name="saveData">包含库存及其他相关数据的保存数据结构</param>
    protected abstract void LoadInventory(SaveData saveData);
}

/// <summary>
/// 用于存储库存系统及其关联变换信息的可序列化结构体
/// 可在保存/加载过程中传递完整的库存状态与位置旋转信息
/// </summary>
[Serializable]
public struct InventorySaveData
{
    /// <summary>
    /// 关联的库存系统实例
    /// </summary>
    public InventorySystem invSystem;
    
    /// <summary>
    /// 对象的世界坐标位置
    /// </summary>
    public Vector3 position;
    
    /// <summary>
    /// 对象的世界坐标旋转
    /// </summary>
    public Quaternion rotation;
    
    /// <summary>
    /// 构造函数：使用指定的库存系统、位置和旋转初始化结构体
    /// </summary>
    /// <param name="invSystem">要保存的库存系统实例</param>
    /// <param name="position">世界坐标中的位置</param>
    /// <param name="rotation">世界坐标中的旋转</param>
    public InventorySaveData(InventorySystem invSystem, Vector3 position, Quaternion rotation)
    {
        this.invSystem = invSystem;
        this.position = position;
        this.rotation = rotation;
    }

    /// <summary>
    /// 构造函数：仅使用库存系统进行初始化，默认位置为原点，旋转为单位四元数
    /// </summary>
    /// <param name="invSystem">要保存的库存系统实例</param>
    public InventorySaveData(InventorySystem invSystem)
    {
        this.invSystem = invSystem;
        this.position = Vector3.zero;
        this.rotation = Quaternion.identity;
    }
}