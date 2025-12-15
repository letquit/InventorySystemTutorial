using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// 商店管理员类，负责管理商店系统的交互和物品库存
/// 需要UniqueID组件来标识商店的唯一性
/// 实现IInteractable接口以支持玩家交互
/// </summary>
[RequireComponent(typeof(UniqueID))]
public class ShopKeeper : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopItemList shopItemsHeld;
    [SerializeField] private ShopSystem shopSystem;

    private ShopSaveData _shopSaveData;
    
    /// <summary>
    /// 商店窗口请求事件的委托定义
    /// </summary>
    public static UnityAction<ShopSystem, PlayerInventoryHolder> OnShopWindowRequested;

    private string _id;

    /// <summary>
    /// 在Awake阶段初始化商店系统，设置商品库存
    /// 根据配置的商品列表创建商店系统实例，并将初始商品添加到商店中
    /// </summary>
    private void Awake()
    {
        // 创建商店系统实例，传入商品数量、最大金币限制和买卖价格倍率
        shopSystem = new ShopSystem(shopItemsHeld.Items.Count, shopItemsHeld.MaxAllowedGold, shopItemsHeld.BuyMarkUp,
            shopItemsHeld.SellMarkUp);

        // 将配置的商品列表添加到商店系统中
        foreach (var item in shopItemsHeld.Items)
        {
            shopSystem.AddToShop(item.itemData, item.amount);
        }
        
        _id = GetComponent<UniqueID>().ID;
        _shopSaveData = new ShopSaveData(shopSystem);
    }

    /// <summary>
    /// 在Start阶段检查并添加商店数据到游戏存档管理器中
    /// 如果当前商店尚未在存档数据中注册，则将其添加到商店管理员字典中
    /// </summary>
    private void Start()
    {
        if (!SaveGameManager.Data.ShopKeeperDictionary.ContainsKey(_id))
            SaveGameManager.Data.ShopKeeperDictionary.Add(_id, _shopSaveData);
    }

    /// <summary>
    /// 在组件启用时订阅游戏加载事件
    /// 当游戏加载事件触发时，调用LoadInventory方法来恢复商店库存数据
    /// </summary>
    private void OnEnable()
    {
        SaveLoad.OnLoadGame += LoadInventory;
    }
    
    /// <summary>
    /// 在组件禁用时取消订阅游戏加载事件
    /// 防止在组件被销毁后仍然响应游戏加载事件
    /// </summary>
    private void OnDisable()
    {
        SaveLoad.OnLoadGame -= LoadInventory;
    }

    /// <summary>
    /// 从存档数据中加载商店库存信息
    /// 根据唯一标识符查找对应的商店保存数据，并恢复商店系统状态
    /// </summary>
    /// <param name="data">包含所有存档数据的对象</param>
    private void LoadInventory(SaveData data)
    {
        // 尝试从存档数据中获取当前商店的保存数据，如果不存在则直接返回
        if (!data.ShopKeeperDictionary.TryGetValue(_id, out ShopSaveData shopSaveData)) return;
        
        // 使用存档数据更新当前商店的保存数据和商店系统实例
        _shopSaveData = shopSaveData;
        shopSystem = shopSaveData.shopSystem;
    }

    /// <summary>
    /// 交互完成事件回调
    /// 当与其他可交互对象完成交互时触发
    /// </summary>
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    
    /// <summary>
    /// 处理交互器与当前对象的交互操作，主要用于打开商店窗口
    /// </summary>
    /// <param name="interactor">发起交互的交互器对象</param>
    /// <param name="interactSuccessful">输出参数，表示交互是否成功执行</param>
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        // 获取交互器上的玩家背包组件
        var playerInv = interactor.GetComponent<PlayerInventoryHolder>();

        if (playerInv != null)
        {
            // 触发商店窗口请求事件，传递商店系统和玩家背包信息
            OnShopWindowRequested?.Invoke(shopSystem, playerInv);
            interactSuccessful = true;
        }
        else
        {
            interactSuccessful = false;
            Debug.LogError("Player inventory not found");
        }
    }

    /// <summary>
    /// 结束当前交互过程
    /// 清理交互状态并通知相关系统交互已结束
    /// </summary>
    public void EndInteraction()
    {
        
    }
}

/// <summary>
/// 商店系统保存数据类，用于序列化和保存商店系统的状态信息
/// </summary>
[Serializable]
public class ShopSaveData
{
    /// <summary>
    /// 关联的商店系统实例
    /// </summary>
    public ShopSystem shopSystem;

    /// <summary>
    /// 初始化商店保存数据实例
    /// </summary>
    /// <param name="shopSystem">要保存的商店系统对象</param>
    public ShopSaveData(ShopSystem shopSystem)
    {
        this.shopSystem = shopSystem;
    }
}