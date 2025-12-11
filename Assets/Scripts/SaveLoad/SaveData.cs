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
    public SerializableDictionary<string, ChestSaveData> ChestDictionary;
    
    /// <summary>
    /// 初始化存档数据对象，创建各个数据集合的实例
    /// </summary>
    public SaveData()
    {
        CollectedItems = new List<string>();
        ActiveItems = new SerializableDictionary<string, ItemPickUpSaveData>();
        ChestDictionary = new SerializableDictionary<string, ChestSaveData>();
    }
}