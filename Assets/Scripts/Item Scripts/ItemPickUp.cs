using System;
using UnityEngine;

/// <summary>
/// 物品拾取组件类
/// 用于处理玩家拾取物品的逻辑，当玩家进入拾取范围时自动将物品添加到背包中
/// 需要挂载在带有SphereCollider组件的游戏对象上
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public float pickUpRadius = 1f;
    public InventoryItemData itemData;

    private SphereCollider myCollider;

    /// <summary>
    /// 组件初始化方法
    /// 在Awake阶段获取SphereCollider组件并设置为触发器模式，同时配置拾取半径
    /// </summary>
    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = pickUpRadius;
    }
    
    /// <summary>
    /// 触发器进入事件处理方法
    /// 当其他碰撞体进入拾取范围时调用，检测是否为拥有背包的物体，如果是则尝试将物品添加到背包中
    /// 添加成功后销毁当前游戏对象
    /// </summary>
    /// <param name="other">进入触发器范围的碰撞体组件</param>
    private void OnTriggerEnter(Collider other)
    {
        // 获取碰撞体对应游戏对象上的玩家背包组件
        var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
        
        // 如果没有找到背包组件则直接返回
        if (!inventory) return;

        // 尝试将物品添加到背包中，如果添加成功则销毁当前游戏对象
        if (inventory.AddToInventory(itemData, 1))
        {
            Destroy(this.gameObject);
        }
    }
}