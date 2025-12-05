using UnityEngine;

/// <summary>
/// 库存物品数据类，用于定义游戏中库存系统中每个物品的基础属性
/// 继承自ScriptableObject，可以通过Unity编辑器创建资产文件
/// </summary>
[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    /// <summary>
    /// 物品唯一标识符
    /// 用于区分不同物品的ID值
    /// </summary>
    public int id;

    /// <summary>
    /// 物品显示名称
    /// 在游戏界面中向玩家展示的物品名称
    /// </summary>
    public string displayName;

    /// <summary>
    /// 物品详细描述信息
    /// 使用TextArea特性提供多行文本编辑区域，用于描述物品的功能和用途
    /// </summary>
    [TextArea(4, 4)]
    public string description;

    /// <summary>
    /// 物品图标精灵
    /// 在UI界面中显示的物品图标图像
    /// </summary>
    public Sprite icon;

    /// <summary>
    /// 物品最大堆叠数量
    /// 定义单个物品槽位中该物品最多可以堆叠的数量
    /// </summary>
    public int maxStackSize;
}