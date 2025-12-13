using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 物品数据库类，用于管理和存储游戏中的所有物品数据
/// 继承自ScriptableObject，可以在Unity编辑器中创建资产文件
/// </summary>
[CreateAssetMenu(menuName = "Inventory System/Item Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<InventoryItemData> _itemDatabase;

    /// <summary>
    /// 设置物品ID并构建物品数据库
    /// 该方法会从Resources文件夹加载所有InventoryItemData资源，
    /// 并根据ID规则重新整理和分配ID，最终构建完整的物品数据库
    /// 可通过Unity编辑器的上下文菜单调用此方法
    /// </summary>
    [ContextMenu("Set IDs")]
    public void SetItemIDs()
    {
        _itemDatabase = new List<InventoryItemData>();

        // 从Resources文件夹加载所有物品数据，并按ID排序
        var foundItems = Resources.LoadAll<InventoryItemData>("ItemData").OrderBy(i => i.id).ToList();

        // 将物品分为三类：ID在有效范围内的、ID超出范围的、没有ID的
        var hasIDInRange = foundItems.Where(i => i.id != -1 && i.id < foundItems.Count).OrderBy(i => i.id).ToList();
        var hasIDNotInRange = foundItems.Where(i => i.id != -1 && i.id >= foundItems.Count).OrderBy(i => i.id).ToList();
        var noID = foundItems.Where(i => i.id <= -1).ToList();
        
        // 重新分配ID并构建数据库
        var index = 0;
        for (int i = 0; i < foundItems.Count; i++)
        {
            InventoryItemData itemToAdd;
            itemToAdd = hasIDInRange.Find(d => d.id == i);

            if (itemToAdd != null)
            {
                _itemDatabase.Add(itemToAdd);
            }
            else if (index < noID.Count)
            {
                noID[index].id = i;
                itemToAdd = noID[index];
                index++;
                _itemDatabase.Add(itemToAdd);
            }
        }

        // 将ID超出范围的物品添加到数据库末尾
        foreach (var item in hasIDNotInRange)
        {
            _itemDatabase.Add(item);
        }
    }

    /// <summary>
    /// 根据ID获取对应的物品数据
    /// </summary>
    /// <param name="id">要查找的物品ID</param>
    /// <returns>找到的物品数据，如果未找到则返回null</returns>
    public InventoryItemData GetItem(int id)
    {
        return _itemDatabase.Find(i => i.id == id);
    }
}