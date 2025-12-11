using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 箱子库存类，继承自InventoryHolder并实现IInteractable接口，用于管理箱子的物品存储和交互功能
/// </summary>
[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    /// <summary>
    /// 交互完成时触发的Unity事件回调
    /// </summary>
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    
    /// <summary>
    /// 在Awake阶段注册游戏加载事件以恢复库存数据，并调用基类初始化逻辑
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        SaveLoad.OnLoadGame += LoadInventory;
    }

    /// <summary>
    /// 初始化箱子保存数据，在Start中将当前箱子的状态存入全局保存字典
    /// </summary>
    private void Start()
    {
        var chestSaveData = new ChestSaveData(this.primaryInventorySystem, this.transform.position, this.transform.rotation);
        // SaveGameManager.Data.ChestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
        // 将当前箱子的数据添加到保存系统中的字典里
        var chestId = GetComponent<UniqueID>().ID;
        SaveGameManager.Data.ChestDictionary[chestId] = chestSaveData;
    }

    /// <summary>
    /// 根据唯一ID从保存数据中加载箱子的位置、旋转及库存信息
    /// </summary>
    /// <param name="data">包含所有保存数据的对象</param>
    private void LoadInventory(SaveData data)
    {
        // 检查此特定箱子的保存数据，如果存在则加载它
        if (data.ChestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out ChestSaveData chestData))
        {
            this.primaryInventorySystem = chestData.invSystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }
    }
    
    /// <summary>
    /// 处理与箱子的交互逻辑，当玩家与箱子互动时调用此方法
    /// </summary>
    /// <param name="interactor">执行交互的交互者对象</param>
    /// <param name="interactSuccessful">输出参数，指示交互是否成功执行</param>
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        // 触发动态库存显示请求事件，显示箱子的库存界面
        OnDynamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem);
        interactSuccessful = true;
    }

    /// <summary>
    /// 结束与箱子的交互，清理交互状态
    /// </summary>
    public void EndInteraction()
    {
        
    }
}

/// <summary>
/// 用于序列化保存箱子相关信息的数据结构
/// </summary>
[Serializable]
public struct ChestSaveData
{
    /// <summary>
    /// 箱子所持有的库存系统实例
    /// </summary>
    public InventorySystem invSystem;
    
    /// <summary>
    /// 箱子在世界坐标中的位置
    /// </summary>
    public Vector3 position;
    
    /// <summary>
    /// 箱子在世界坐标中的旋转角度
    /// </summary>
    public Quaternion rotation;
    
    /// <summary>
    /// 构造函数，初始化箱子保存数据
    /// </summary>
    /// <param name="invSystem">库存系统实例</param>
    /// <param name="position">箱子的世界坐标位置</param>
    /// <param name="rotation">箱子的世界坐标旋转</param>
    public ChestSaveData(InventorySystem invSystem, Vector3 position, Quaternion rotation)
    {
        this.invSystem = invSystem;
        this.position = position;
        this.rotation = rotation;
    }
}