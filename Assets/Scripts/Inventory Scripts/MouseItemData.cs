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
    /// 玩家对象的变换组件引用，用于获取玩家的位置和旋转信息
    /// </summary>
    private Transform _playerTransform;

    /// <summary>
    /// 物体掉落时相对于玩家位置的偏移距离
    /// </summary>
    public float dropOffset = 3f;

    /// <summary>
    /// 初始化游戏对象组件和状态
    /// </summary>
    /// <remarks>
    /// 此函数在对象创建时自动调用，用于初始化物品显示组件的状态，
    /// 包括设置图标透明度、文本内容以及获取玩家对象的引用。
    /// </remarks>
    private void Awake()
    {
        // 初始化物品图标为透明状态
        itemSprite.color = Color.clear;
        itemSprite.preserveAspect = true;
        
        // 初始化物品数量文本为空
        itemCount.text = "";
        
        // 获取玩家对象的Transform组件引用
        _playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (_playerTransform == null) Debug.Log("Player not found!");
    }

    /// <summary>
    /// 更新鼠标槽位的信息，包括图标、数量等
    /// </summary>
    /// <param name="invSlot">要更新到鼠标槽位的目标库存槽位</param>
    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        assignedInventorySlot.AssignItem(invSlot);
        UpdateMouseSlot();
    }
    
    /// <summary>
    /// 更新鼠标槽位的显示状态
    /// </summary>
    /// <remarks>
    /// 该函数用于更新鼠标槽位中物品的图标显示、堆叠数量显示以及图标颜色
    /// </remarks>
    public void UpdateMouseSlot()
    {
        // 设置物品图标
        itemSprite.sprite = assignedInventorySlot.ItemData.icon;
        
        // 根据堆叠数量决定是否显示具体数值
        if (assignedInventorySlot.StackSize > 1)
            itemCount.text = assignedInventorySlot.StackSize.ToString();
        else 
            itemCount.text = "";
            
        // 恢复图标为正常颜色
        itemSprite.color = Color.white;
    }

    /// <summary>
    /// 每帧更新鼠标位置，并检测是否点击了非UI区域以清空当前槽位
    /// </summary>
    private void Update()
    {
        // 只有在当前分配了有效物品数据时才进行位置同步与交互判断
        if (assignedInventorySlot.ItemData != null) // 如果有物品，则跟随鼠标位置
        {
            // 同步鼠标槽位位置到当前鼠标位置
            transform.position = Mouse.current.position.ReadValue();

            // 检测鼠标左键点击且未指向UI元素的情况
            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                // 如果物品有预制体，则在角色前方实例化该物品
                if (assignedInventorySlot.ItemData.itemPrefab != null)
                {
                    // 计算物品掉落位置，位于角色前方一定距离处
                    Vector3 dropPosition = _playerTransform.position + _playerTransform.forward * dropOffset;
                    dropPosition.y = -0.5f;
        
                    // 实例化物品预制体
                    Instantiate(assignedInventorySlot.ItemData.itemPrefab,
                        dropPosition, Quaternion.identity);
                }

                // 根据堆叠数量决定是减少堆叠还是完全清空槽位
                if (assignedInventorySlot.StackSize > 1)
                {
                    // 减少堆叠数量
                    assignedInventorySlot.AddToStack(-1);
                    // 更新鼠标槽位显示
                    UpdateMouseSlot();
                }
                else
                {
                    // 清空整个槽位
                    ClearSlot();
                }
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