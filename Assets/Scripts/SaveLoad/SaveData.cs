using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存档数据类，用于保存游戏中的各种状态信息
/// </summary>
public class SaveData
{
    /// <summary>
    /// 已收集物品列表，存储物品的唯一标识符
    /// </summary>
    public List<string> CollectedItems;
    
    /// <summary>
    /// 激活物品字典，键为物品ID，值为物品拾取保存数据
    /// </summary>
    public SerializableDictionary<string, ItemPickUpSaveData> ActiveItems;
    
    /// <summary>
    /// 箱子字典，键为箱子ID，值为箱子保存数据
    /// </summary>
    public SerializableDictionary<string, InventorySaveData> ChestDictionary;
    
    /// <summary>
    /// 玩家背包保存数据
    /// </summary>
    public InventorySaveData PlayerInventory;
    
    /// <summary>
    /// 初始化存档数据对象，创建各个数据集合的实例
    /// </summary>
    public SaveData()
    {
        // 初始化已收集物品列表
        CollectedItems = new List<string>();
        // 初始化激活物品字典
        ActiveItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        // 初始化箱子字典
        ChestDictionary = new SerializableDictionary<string, InventorySaveData>();
        // 初始化玩家背包数据
        PlayerInventory = new InventorySaveData();
    }
}