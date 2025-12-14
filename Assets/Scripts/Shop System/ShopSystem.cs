using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// 商店系统类，用于管理商店的库存、金币和商品买卖功能
/// </summary>
[Serializable]
public class ShopSystem
{
    [SerializeField] private List<ShopSlot> shopInventory;
    [SerializeField] private int availableGold;
    [SerializeField] private float buyMarkUp;
    [SerializeField] private float sellMarkUp;
    
    /// <summary>
    /// 初始化商店系统
    /// </summary>
    /// <param name="size">商店槽位数量</param>
    /// <param name="gold">商店初始拥有的金币数量</param>
    /// <param name="buyMarkUp">购买时的价格倍率</param>
    /// <param name="sellMarkUp">出售时的价格倍率</param>
    public ShopSystem(int size, int gold, float buyMarkUp, float sellMarkUp)
    {
        availableGold = gold;
        this.buyMarkUp = buyMarkUp;
        this.sellMarkUp = sellMarkUp;
        
        SetShopSize(size);
    }

    /// <summary>
    /// 设置商店槽位数量
    /// </summary>
    /// <param name="size">要设置的槽位数量</param>
    public void SetShopSize(int size)
    {
        shopInventory = new List<ShopSlot>(size);
        for (int i = 0; i < size; i++)
        {
            shopInventory.Add(new ShopSlot());
        }
    }

    /// <summary>
    /// 向商店添加商品
    /// </summary>
    /// <param name="data">要添加的商品数据</param>
    /// <param name="amount">要添加的商品数量</param>
    public void AddToShop(InventoryItemData data, int amount)
    {
        // 如果商店已包含该商品，则增加该商品的堆叠数量
        if (ContainsItem(data, out ShopSlot shopSlot))
        {
            shopSlot.AddToStack(amount);
        }else
        {
            // 获取一个空闲槽位并分配商品
            var freeSlot = GetFreeSlot();
            freeSlot.AssignItem(data, amount);
        }

        // // 获取一个空闲槽位并分配商品
        // var freeSlot = GetFreeSlot();
        // freeSlot.AssignItem(data, amount);
    }

    /// <summary>
    /// 获取一个空闲的商店槽位
    /// </summary>
    /// <returns>空闲的商店槽位，如果没有则创建一个新的</returns>
    private ShopSlot GetFreeSlot()
    {
        var freeSlot = shopInventory.FirstOrDefault(i => i.ItemData == null);

        // 如果没有找到空闲槽位，则创建一个新的槽位
        if (freeSlot == null)
        {
            freeSlot = new ShopSlot();
            shopInventory.Add(freeSlot);
        }
        
        return freeSlot;
    }
    
    /// <summary>
    /// 检查商店是否包含指定商品
    /// </summary>
    /// <param name="itemToAdd">要检查的商品数据</param>
    /// <param name="shopSlot">输出参数，如果找到则返回对应的商店槽位</param>
    /// <returns>如果商店包含该商品则返回true，否则返回false</returns>
    public bool ContainsItem(InventoryItemData itemToAdd, out ShopSlot shopSlot)
    {
        shopSlot = shopInventory.Find(i => i.ItemData == itemToAdd);
        return shopSlot != null;
    }
}