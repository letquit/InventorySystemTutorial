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
            Debug.Log($"{item.itemData.displayName}: {item.amount}");
            shopSystem.AddToShop(item.itemData, item.amount);
        }
    }

    /// <summary>
    /// 交互完成事件回调
    /// 当与其他可交互对象完成交互时触发
    /// </summary>
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    
    /// <summary>
    /// 处理与交互者的交互逻辑
    /// </summary>
    /// <param name="interactor">发起交互的对象</param>
    /// <param name="interactSuccessful">输出参数，指示交互是否成功</param>
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 结束当前交互过程
    /// 清理交互状态并通知相关系统交互已结束
    /// </summary>
    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}