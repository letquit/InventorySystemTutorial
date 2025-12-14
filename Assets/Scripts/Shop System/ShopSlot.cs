using System;
using UnityEngine;

/// <summary>
/// 商店槽位类，继承自物品槽位基类
/// 用于表示商店中的物品槽位，包含物品信息和交易相关功能
/// </summary>
[Serializable]
public class ShopSlot : ItemSlot
{
    /// <summary>
    /// 商店槽位构造函数
    /// 初始化商店槽位并清空槽位内容
    /// </summary>
    public ShopSlot()
    {
        ClearSlot();
    }
}