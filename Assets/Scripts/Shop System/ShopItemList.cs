using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商店物品列表脚本化对象类
/// 用于存储商店的物品库存信息，包括物品列表、金币上限和买卖标记
/// 可通过Unity编辑器的Assets/Create/Shop System/Shop Item List菜单创建实例
/// </summary>
[CreateAssetMenu(menuName = "Shop System/Shop Item List")]
public class ShopItemList : ScriptableObject
{
    /// <summary>
    /// 商店中的物品列表
    /// </summary>
    [SerializeField] private List<ShopInventoryItem> items;
    
    /// <summary>
    /// 商店允许的最大金币数量
    /// </summary>
    [SerializeField] private int maxAllowedGold;
    
    /// <summary>
    /// 出售物品时的价格标记，用于计算出售价格
    /// </summary>
    [SerializeField] private float sellMarkUp;
    
    /// <summary>
    /// 购买物品时的价格标记，用于计算购买价格
    /// </summary>
    [SerializeField] private float buyMarkUp;

    /// <summary>
    /// 获取商店物品列表的只读属性
    /// </summary>
    public List<ShopInventoryItem> Items => items;
    
    /// <summary>
    /// 获取商店最大允许金币数量的只读属性
    /// </summary>
    public int MaxAllowedGold => maxAllowedGold;
    
    /// <summary>
    /// 获取出售标记的只读属性
    /// </summary>
    public float SellMarkUp => sellMarkUp;
    
    /// <summary>
    /// 获取购买标记的只读属性
    /// </summary>
    public float BuyMarkUp => buyMarkUp;
}

/// <summary>
/// 商店库存物品结构体
/// 用于表示商店中单个物品的数据，包括物品信息和数量
/// </summary>
[Serializable]
public struct ShopInventoryItem
{
    /// <summary>
    /// 物品数据引用
    /// </summary>
    public InventoryItemData itemData;
    
    /// <summary>
    /// 物品数量
    /// </summary>
    public int amount;
}