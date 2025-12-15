using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 商店物品槽位UI类，用于显示商店中的单个物品及其交互按钮。
/// 负责展示物品图标、名称、数量，并处理添加/移除购物车的逻辑。
/// </summary>
public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private ShopSlot assignedItemSlot;
    
    /// <summary>
    /// 获取与此UI关联的商店槽位数据对象
    /// </summary>
    public ShopSlot AssignedItemSlot => assignedItemSlot;

    [SerializeField] private Button addItemToCartButton;
    [SerializeField] private Button removeItemFromCartButton;

    private int _tempAmount;
    
    /// <summary>
    /// 获取该UI元素所属的商店显示组件
    /// </summary>
    public ShopKeeperDisplay ParentDisplay { get; private set; }
    
    /// <summary>
    /// 获取当前商品的价格倍率（加价比例）
    /// </summary>
    public float MarkUp { get; private set; }
    
    /// <summary>
    /// 在Awake阶段初始化UI组件状态并绑定按钮事件监听器
    /// </summary>
    private void Awake()
    {
        // 初始化UI元素为默认空状态
        itemSprite.sprite = null;
        itemSprite.preserveAspect = true;
        itemSprite.color = Color.clear;
        itemName.text = "";
        itemCount.text = "";
        
        // 绑定按钮点击事件
        addItemToCartButton?.onClick.AddListener(AddItemToCart);
        removeItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);
        
        // 查找并设置父级商店显示组件引用
        ParentDisplay = transform.parent.GetComponentInParent<ShopKeeperDisplay>();
    }

    /// <summary>
    /// 初始化商店槽位UI的数据
    /// </summary>
    /// <param name="slot">要分配给此UI的商店槽位数据</param>
    /// <param name="markUp">价格倍率，影响物品最终售价</param>
    public void Init(ShopSlot slot, float markUp)
    {
        assignedItemSlot = slot;
        MarkUp = markUp;
        _tempAmount = slot.StackSize;
        UpdateUISlot();
    }

    /// <summary>
    /// 根据已分配的商店槽位数据更新UI显示内容
    /// 包括物品图标、名称、数量等信息
    /// </summary>
    private void UpdateUISlot()
    {
        if (assignedItemSlot.ItemData != null)
        {
            // 显示物品相关信息
            itemSprite.sprite = assignedItemSlot.ItemData.icon;
            itemSprite.color = Color.white;
            itemCount.text = assignedItemSlot.StackSize.ToString();
            // 计算修改后的价格
            var modifiedPrice = ShopKeeperDisplay.GetModifiedPrice(assignedItemSlot.ItemData, 1, MarkUp);
            
            itemName.text = $"{assignedItemSlot.ItemData.displayName} - {modifiedPrice}G";
        }
        else
        {
            // 清除UI显示内容
            itemSprite.sprite = null;
            itemSprite.color = Color.clear;
            itemName.text = "";
            itemCount.text = "";
        }
    }

    /// <summary>
    /// 将当前物品添加到购物车中。若临时库存不足则不执行操作。
    /// 每次调用会减少一个单位的可购买数量，并通知父级界面进行同步。
    /// </summary>
    private void AddItemToCart()
    {
        if (_tempAmount <= 0) return;
        
        _tempAmount--;
        ParentDisplay.AddItemToCart(this);
        itemCount.text = _tempAmount.ToString();
        
    }

    /// <summary>
    /// 从购物车中移除当前物品。若临时库存等于原始堆叠数则不执行操作。
    /// 每次调用会增加一个单位的可购买数量，并通知父级界面进行同步。
    /// </summary>
    private void RemoveItemFromCart()
    {
        if (_tempAmount == assignedItemSlot.StackSize) return;
        
        _tempAmount++;
        ParentDisplay.RemoveItemFromCart(this);
        itemCount.text = _tempAmount.ToString();
    }
}