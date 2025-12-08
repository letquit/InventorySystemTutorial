using System;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
    /// 表示分配给当前对象的库存槽位
    /// </summary>
    public InventorySlot assignedInventorySlot;

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

    /// <summary>
    /// 更新鼠标槽位的信息，包括图标、数量等
    /// </summary>
    /// <param name="invSlot">要更新到鼠标槽位的目标库存槽位</param>
    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        assignedInventorySlot.AssignItem(invSlot);
        itemSprite.sprite = invSlot.ItemData.icon;
        
        // 如果堆叠数量大于1则显示具体数值，否则隐藏数量显示
        if (invSlot.StackSize > 1) 
            itemCount.text = invSlot.StackSize.ToString();
        else 
            itemCount.text = "";
            
        itemSprite.color = Color.white;
    }

    /// <summary>
    /// 每帧更新鼠标位置，并检测是否点击了非UI区域以清空当前槽位
    /// </summary>
    private void Update()
    {
        // 只有在当前分配了有效物品数据时才进行位置同步与交互判断
        if (assignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            // 当按下鼠标左键且未指向任何UI元素时清除当前槽位内容
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ClearSlot();
            }
        }
    }

    /// <summary>
    /// 清除当前鼠标槽位的内容并重置相关显示组件的状态
    /// </summary>
    public void ClearSlot()
    {
        assignedInventorySlot.ClearSlot();
        itemCount.text = "";
        itemSprite.color = Color.clear;
        itemSprite.sprite = null;
    }

    /// <summary>
    /// 判断当前鼠标指针是否位于任意一个UI对象之上
    /// </summary>
    /// <returns>如果鼠标指针在UI上则返回true，否则返回false</returns>
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}