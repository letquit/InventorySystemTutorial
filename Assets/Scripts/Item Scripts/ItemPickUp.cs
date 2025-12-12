using System;
using UnityEngine;

/// <summary>
/// 物品拾取组件类
/// 用于处理玩家拾取物品的逻辑，当玩家进入拾取范围时自动将物品添加到背包中
/// 需要挂载在带有SphereCollider组件的游戏对象上
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    /// <summary>
    /// 拾取范围半径
    /// </summary>
    public float pickUpRadius = 1f;

    /// <summary>
    /// 要拾取的物品数据
    /// </summary>
    public InventoryItemData itemData;

    private SphereCollider myCollider;

    /// <summary>
    /// 物品保存数据结构，用于序列化存储位置、旋转及物品信息
    /// </summary>
    [SerializeField] private ItemPickUpSaveData itemSaveData;

    /// <summary>
    /// 唯一标识符，用于识别该物品实例
    /// </summary>
    private string id;

    /// <summary>
    /// 组件初始化方法
    /// 在Awake阶段获取SphereCollider组件并设置为触发器模式，同时配置拾取半径
    /// 同时注册加载游戏事件监听，并初始化物品保存数据
    /// </summary>
    private void Awake()
    {
        id = GetComponent<UniqueID>().ID;
        SaveLoad.OnLoadGame += LoadData;
        itemSaveData = new ItemPickUpSaveData(itemData, transform.position, transform.rotation);
        
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = pickUpRadius;
    }

    /// <summary>
    /// 初始化操作，在Start阶段将当前物品加入活跃物品列表中
    /// </summary>
    private void Start()
    {
        SaveGameManager.Data.ActiveItems.Add(id, itemSaveData);
    }

    /// <summary>
    /// 加载游戏数据回调方法
    /// 根据传入的存档数据判断当前物品是否已被收集，若已收集则销毁该游戏对象
    /// </summary>
    /// <param name="data">从存档读取的数据</param>
    private void LoadData(SaveData data)
    {
        if (data.CollectedItems.Contains(id)) Destroy(this.gameObject);
    }
    
    /// <summary>
    /// 销毁前清理资源
    /// 移除加载游戏事件监听，并从活跃物品列表中移除自身记录
    /// </summary>
    private void OnDestroy()
    {
        if (SaveGameManager.Data.ActiveItems.ContainsKey(id)) SaveGameManager.Data.ActiveItems.Remove(id);
        SaveLoad.OnLoadGame -= LoadData;
    }

    /// <summary>
    /// 触发器进入事件处理方法
    /// 当其他碰撞体进入拾取范围时调用，检测是否为拥有背包的物体，如果是则尝试将物品添加到背包中
    /// 添加成功后销毁当前游戏对象
    /// </summary>
    /// <param name="other">进入触发器范围的碰撞体组件</param>
    private void OnTriggerEnter(Collider other)
    {
        // 获取碰撞体对应游戏对象上的玩家背包组件
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        
        // 如果没有找到背包组件则直接返回
        if (!inventory) return;

        // 尝试将物品添加到背包中，如果添加成功则销毁当前游戏对象
        if (inventory.AddToInventory(itemData, 1))
        {
            SaveGameManager.Data.CollectedItems.Add(id);
            Destroy(this.gameObject);
        }
    }
}

/// <summary>
/// 可序列化的物品拾取保存数据结构
/// 包含物品数据、世界坐标和旋转信息，用于持久化保存场景中的可拾取物品状态
/// </summary>
[Serializable]
public struct ItemPickUpSaveData
{
    /// <summary>
    /// 物品数据引用
    /// </summary>
    public InventoryItemData itemData;

    /// <summary>
    /// 物品在世界空间中的位置
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 物品在世界空间中的旋转
    /// </summary>
    public Quaternion rotation;
    
    /// <summary>
    /// 构造函数，使用指定的物品数据、位置与旋转创建一个ItemPickUpSaveData实例
    /// </summary>
    /// <param name="itemData">要保存的物品数据</param>
    /// <param name="position">物品的世界坐标</param>
    /// <param name="rotation">物品的世界旋转</param>
    public ItemPickUpSaveData(InventoryItemData itemData, Vector3 position, Quaternion rotation)
    {
        this.itemData = itemData;
        this.position = position;
        this.rotation = rotation;
    }
}