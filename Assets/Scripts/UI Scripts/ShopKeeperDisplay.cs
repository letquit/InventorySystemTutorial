using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// 商店界面显示管理器
/// 负责管理商店界面的显示逻辑，包括商品展示、购物车、购买和出售功能
/// </summary>
public class ShopKeeperDisplay : MonoBehaviour
{
    [SerializeField] private ShopSlotUI shopSlotPrefab;
    [SerializeField] private ShoppingCartItemUI shoppingCartItemUIPrefab;

    [SerializeField] private Button buyTab;
    [SerializeField] private Button sellTab;

    [Header("Shopping Cart")] 
    [SerializeField] private TextMeshProUGUI basketTotalText;
    [SerializeField] private TextMeshProUGUI playerGoldText;
    [SerializeField] private TextMeshProUGUI shopGoldText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyButtonText;

    [Header("Item Preview Section")] 
    [SerializeField] private Image itemPreviewSprite;
    [SerializeField] private TextMeshProUGUI itemPreviewName;
    [SerializeField] private TextMeshProUGUI itemPreviewDescription;

    [SerializeField] private GameObject itemListContentPanel;
    [SerializeField] private GameObject shoppingCartContentPanel;

    private int _basketTotal;

    private ShopSystem _shopSystem;
    private PlayerInventoryHolder _playerInventoryHolder;
    
    private Dictionary<InventoryItemData, int> _shoppingCart = new Dictionary<InventoryItemData, int>();

    private Dictionary<InventoryItemData, ShoppingCartItemUI> _shoppingCartUI =
        new Dictionary<InventoryItemData, ShoppingCartItemUI>();
    
    /// <summary>
    /// 显示商店窗口并初始化相关系统引用
    /// </summary>
    /// <param name="shopSystem">当前商店系统的实例</param>
    /// <param name="playerInventoryHolder">玩家背包持有者实例</param>
    public void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventoryHolder)
    {
        _shopSystem = shopSystem;
        _playerInventoryHolder = playerInventoryHolder;
        
        RefreshDisplay();
    }
    
    /// <summary>
    /// 刷新整个商店界面显示内容
    /// 包括清空现有UI元素、重置购物车状态以及重新加载商店库存
    /// </summary>
    private void RefreshDisplay()
    {
        ClearSlots();

        basketTotalText.enabled = false;
        buyButton.gameObject.SetActive(false);
        _basketTotal = 0;
        playerGoldText.text = $"Player Gold: {_playerInventoryHolder.PrimaryInventorySystem.Gold}";
        shopGoldText.text = $"Shop Gold: {_shopSystem.AvailableGold}";
        
        DisplayShopInventory();
    }

    /// <summary>
    /// 清除所有已创建的商品槽位与购物车UI项，并销毁对应的游戏对象
    /// 同时重置内部数据结构以便后续刷新使用
    /// </summary>
    private void ClearSlots()
    {
        _shoppingCart = new Dictionary<InventoryItemData, int>();
        _shoppingCartUI = new Dictionary<InventoryItemData, ShoppingCartItemUI>();
        
        // 销毁商店物品列表中的所有子项
        foreach (var item in itemListContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        // 销毁购物车面板中的所有子项
        foreach (var item in shoppingCartContentPanel.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// 展示商店库存中所有的有效商品到界面上
    /// 每个商品通过 ShopSlotUI 预制体进行实例化并初始化
    /// </summary>
    private void DisplayShopInventory()
    {
        foreach (var item in _shopSystem.ShopInventory)
        {
            if (item.ItemData == null) continue;

            var shopSlot = Instantiate(shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.Init(item, _shopSystem.BuyMarkUp);
        }
    }

    /// <summary>
    /// 用于展示玩家可售出的物品列表
    /// 当前方法为空实现，待后续补充具体逻辑
    /// </summary>
    private void DisplayPlayerInventory()
    {
        
    }
}