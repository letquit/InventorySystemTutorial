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
    [FormerlySerializedAs("shoppingCartItemUIPrefab")] [SerializeField] private ShoppingCartItemUI shoppingCartItemPrefab;

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

    private bool _isSelling;

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
    /// 刷新整个商店界面的状态，包括按钮文本、事件监听、总金额等信息。
    /// 根据是否处于销售模式切换 Buy/Sell 按钮行为及文字描述。
    /// </summary>
    private void RefreshDisplay()
    {
        // 更新购买/出售按钮的文本和点击事件
        if (buyButton != null)
        {
            buyButtonText.text = _isSelling ? "Sell Items" : "Buy Items";
            buyButton.onClick.RemoveAllListeners();
            if (_isSelling) buyButton.onClick.AddListener(SellItems);
            else buyButton.onClick.AddListener(BuyItems);
        }
        
        // 清空物品槽位和预览信息
        ClearSlots();
        ClearItemPreview();

        // 重置购物篮状态和金额显示
        basketTotalText.enabled = false;
        buyButton.gameObject.SetActive(false);
        _basketTotal = 0;
        
        // 更新玩家和商店的金币显示
        playerGoldText.text = $"Player Gold: {_playerInventoryHolder.PrimaryInventorySystem.Gold}G";
        shopGoldText.text = $"Shop Gold: {_shopSystem.AvailableGold}G";
        
        // 根据当前模式显示对应的库存信息
        if (_isSelling) DisplayPlayerInventory();
        else DisplayShopInventory();
    }

    /// <summary>
    /// 处理玩家向商店出售物品的操作
    /// </summary>
    /// <remarks>
    /// 该方法首先检查商店是否有足够的金币来购买购物车中的物品，
    /// 如果有足够资金，则遍历购物车中的每种物品，计算修改后的价格，
    /// 然后执行物品出售操作，将获得的金币添加到玩家库存中，
    /// 并从玩家库存中移除已出售的物品，最后刷新显示界面。
    /// </remarks>
    private void SellItems()
    {
        // 检查商店是否有足够的金币购买购物车中的物品
        if (_shopSystem.AvailableGold < _basketTotal) return;

        // 遍历购物车中的所有物品并进行出售处理
        foreach (var kvp in _shoppingCart)
        {
            var price = GetModifiedPrice(kvp.Key, kvp.Value, _shopSystem.SellMarkUp);
            
            _shopSystem.SellItem(kvp.Key, kvp.Value, price);

            _playerInventoryHolder.PrimaryInventorySystem.GainGold(price);
            _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(kvp.Key, kvp.Value);
        }
        
        // 刷新界面显示
        RefreshDisplay();
    }

    /// <summary>
    /// 执行从商店购买物品的流程：检查金币余额与背包空间，
    /// 若条件满足则执行交易操作，并更新双方资源状态后刷新界面。
    /// </summary>
    private void BuyItems()
    {
        // 检查玩家金币余额
        if (_playerInventoryHolder.PrimaryInventorySystem.Gold < _basketTotal) return;

        // 检查玩家背包空间
        if (!_playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;

        foreach (var kvp in _shoppingCart)
        {
            // 从商店中购买物品
            _shopSystem.PurchaseItem(kvp.Key, kvp.Value);

            // 添加到玩家背包
            for (int i = 0; i < kvp.Value; i++)
            {
                _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(kvp.Key, 1);
            }
        }

        // 更新商店金币余额和玩家的金币数量
        _shopSystem.GainGold(_basketTotal);
        _playerInventoryHolder.PrimaryInventorySystem.SpendGold(_basketTotal);
        
        RefreshDisplay();
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
    /// </summary>
    private void DisplayPlayerInventory()
    {
        // 遍历玩家库存中的所有物品
        foreach (var item in _playerInventoryHolder.PrimaryInventorySystem.GetAllItemsHeld())
        {
            var tempSlot = new ShopSlot();
            tempSlot.AssignItem(item.Key, item.Value);

            // 实例化商店槽位预制体并初始化
            var shopSlot = Instantiate(shopSlotPrefab, itemListContentPanel.transform);
            shopSlot.Init(tempSlot, _shopSystem.SellMarkUp);
        }
    }

    /// <summary>
    /// 将指定商品从购物车中移除一次数量。若该商品在购物车中数量归零，则将其完全删除。
    /// 同步更新UI显示以及总价计算。
    /// </summary>
    /// <param name="shopSlotUI">触发此操作的商店槽位UI组件</param>
    public void RemoveItemFromCart(ShopSlotUI shopSlotUI)
    {
        // 获取物品数据和价格
        var data = shopSlotUI.AssignedItemSlot.ItemData;
        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);
        
        if (_shoppingCart.ContainsKey(data))
        {
            // 减少数量
            _shoppingCart[data]--;
            var newString = $"{data.displayName} ({price}G) x{_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);

            // 若数量为零，则删移除该物品
            if (_shoppingCart[data] <= 0)
            {
                _shoppingCart.Remove(data);
                var tempObj = _shoppingCartUI[data].gameObject;
                _shoppingCartUI.Remove(data);
                Destroy(tempObj);
            }
        }

        // 更新总价
        _basketTotal -= price;
        basketTotalText.text = $"Total: {_basketTotal}G";

        // 如果总价为零，则隐藏总价文本和购买按钮
        if (_basketTotal <= 0 && basketTotalText.IsActive())
        {
            basketTotalText.enabled = false;
            buyButton.gameObject.SetActive(false);
            ClearItemPreview();
            return;
        }
        
        CheckCartVsAvailableGold();
    }

    /// <summary>
    /// 清空预览区域的内容
    /// </summary>
    private void ClearItemPreview()
    {
        // 清空物品预览图像
        itemPreviewSprite.sprite = null;
        itemPreviewSprite.color = Color.clear;
        
        // 清空物品名称和描述文本
        itemPreviewName.text = "";
        itemPreviewDescription.text = "";
    }

    /// <summary>
    /// 将指定商品添加至购物车一次数量。如果该商品已在购物车中存在，则仅增加其计数；
    /// 否则新建一个购物车条目并同步更新UI。
    /// 同时更新总价并判断是否超出可用资金或背包容量限制。
    /// </summary>
    /// <param name="shopSlotUI">触发此操作的商店槽位UI组件</param>
    public void AddItemToCart(ShopSlotUI shopSlotUI)
    {
        // 获取物品数据和价格
        var data = shopSlotUI.AssignedItemSlot.ItemData;

        UpdateItemPreview(shopSlotUI);

        var price = GetModifiedPrice(data, 1, shopSlotUI.MarkUp);
        
        if (_shoppingCart.ContainsKey(data))
        {
            _shoppingCart[data]++;
            var newString = $"{data.displayName} ({price}G) x{_shoppingCart[data]}";
            _shoppingCartUI[data].SetItemText(newString);
        }
        else
        {
            _shoppingCart.Add(data, 1);

            var shoppingCartTextObj = Instantiate(shoppingCartItemPrefab, shoppingCartContentPanel.transform);
            var newString = $"{data.displayName} ({price}G) x1";
            shoppingCartTextObj.SetItemText(newString);
            _shoppingCartUI.Add(data, shoppingCartTextObj);
        }
        
        _basketTotal += price;
        basketTotalText.text = $"Total: {_basketTotal}G";

        if (_basketTotal > 0 && !basketTotalText.IsActive())
        {
            basketTotalText.enabled = true;
            buyButton.gameObject.SetActive(true);
        }

        CheckCartVsAvailableGold();
    }
    
    /// <summary>
    /// 检查当前购物车内总价是否超过可用资金或背包剩余空间。
    /// 若超限则将总价文本设为红色以提示用户。
    /// </summary>
    private void CheckCartVsAvailableGold()
    {
        // 根据 _isSelling 状态选择检查对象（商店金币或玩家金币）
        var goldToCheck = _isSelling ? _shopSystem.AvailableGold : _playerInventoryHolder.PrimaryInventorySystem.Gold;
        basketTotalText.color = _basketTotal > goldToCheck ? Color.red : Color.white;
        
        // 如果当前是出售模式且背包剩余空间不足，则提示玩家背包已满
        if (_isSelling || _playerInventoryHolder.PrimaryInventorySystem.CheckInventoryRemaining(_shoppingCart)) return;

        basketTotalText.text = "Not enough room in inventory.";
        basketTotalText.color = Color.red;
    }

    /// <summary>
    /// 计算某件物品根据倍率调整后的价格
    /// </summary>
    /// <param name="data">要计算价格的物品数据</param>
    /// <param name="amount">物品的数量</param>
    /// <param name="markUp">价格上浮比例</param>
    /// <returns>经过上浮处理后的整型价格数值</returns>
    public static int GetModifiedPrice(InventoryItemData data, int amount, float markUp)
    {
        // 计算基础价值 = 物品单价 × 数量
        var baseValue = data.goldValue * amount;

        // 返回上浮调整后的价格（向下取整）
        return Mathf.FloorToInt(baseValue + baseValue * markUp);
    }

    /// <summary>
    /// 更新商品详情预览区的信息
    /// </summary>
    /// <param name="shopSlotUI">提供预览信息来源的商店槽位UI组件</param>
    private void UpdateItemPreview(ShopSlotUI shopSlotUI)
    {
        // 获取商店槽位关联的物品数据
        var data = shopSlotUI.AssignedItemSlot.ItemData;
        
        // 更新预览区域的图标、名称和描述信息
        itemPreviewSprite.sprite = data.icon;
        itemPreviewSprite.color = Color.white;
        itemPreviewName.text = data.displayName;
        itemPreviewDescription.text = data.description;
    }

    /// <summary>
    /// 处理购买标签按钮点击事件，切换到购买模式并刷新显示
    /// </summary>
    public void OnBuyTebPressed()
    {
        _isSelling = false;
        RefreshDisplay();
    }

    /// <summary>
    /// 处理出售标签按钮点击事件，切换到出售模式并刷新显示
    /// </summary>
    public void OnSellTabPressed()
    {
        _isSelling = true;
        RefreshDisplay();
    }
}