using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 玩家物品栏类，继承自InventoryHolder并实现IInteractable接口
/// 负责管理 chests（箱子）的物品存储功能，并支持与玩家的交互操作
/// </summary>
[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    /// <summary>
    /// 交互完成时触发的Unity事件回调
    /// </summary>
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    /// <summary>
    /// 在游戏对象启动时执行初始化操作
    /// 创建当前chest的保存数据并将其添加到全局保存管理器中
    /// </summary>
    private void Start()
    {
        // 创建包含当前物品栏系统、位置和旋转信息的保存数据
        var chestSaveData = new InventorySaveData(this.primaryInventorySystem, this.transform.position, this.transform.rotation);
        // 使用唯一ID将chest数据添加到保存字典中
        SaveGameManager.Data.ChestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }

    /// <summary>
    /// 从保存数据中加载物品栏信息
    /// 根据唯一ID查找对应的chest数据并恢复其状态
    /// </summary>
    /// <param name="data">包含所有保存数据的对象</param>
    protected override void LoadInventory(SaveData data)
    {
        // 尝试根据唯一ID获取对应的chest保存数据
        if (data.ChestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
        {
            // 恢复物品栏系统、位置和旋转信息
            this.primaryInventorySystem = chestData.invSystem;
            this.transform.position = chestData.position;
            this.transform.rotation = chestData.rotation;
        }
    }

    /// <summary>
    /// 处理与其他游戏对象的交互逻辑
    /// 当玩家与chest交互时显示其物品栏界面
    /// </summary>
    /// <param name="interactor">发起交互的对象（通常是玩家）</param>
    /// <param name="interactSuccessful">输出参数，指示交互是否成功执行</param>
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        // 触发动态物品栏显示请求，展示当前chest的物品栏
        OnDynamicInventoryDisplayRequested?.Invoke(PrimaryInventorySystem, 0);
        interactSuccessful = true;
    }

    /// <summary>
    /// 结束当前交互过程
    /// 可用于清理交互相关的资源或状态
    /// </summary>
    public void EndInteraction()
    {
        
    }
}