using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 鼠标物品数据管理类，用于控制鼠标悬停时显示的物品图标和数量信息
/// </summary>
public class MouseItemData : MonoBehaviour
{
    /// <summary>
    /// 物品图标显示组件
    /// </summary>
    public Image itemSprite;
    
    /// <summary>
    /// 物品数量文本显示组件
    /// </summary>
    public TextMeshProUGUI itemCount;

    /// <summary>
    /// 在对象唤醒时初始化物品图标和数量显示
    /// 将物品图标设置为透明，数量文本设置为空字符串
    /// </summary>
    private void Awake()
    {
        // 初始化物品图标为透明状态
        itemSprite.color = Color.clear;
        // 初始化物品数量文本为空
        itemCount.text = "";
    }
}

