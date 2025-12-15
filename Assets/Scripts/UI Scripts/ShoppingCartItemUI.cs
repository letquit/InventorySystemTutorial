using TMPro;
using UnityEngine;

/// <summary>
/// 购物车物品UI显示类
/// 负责在用户界面中展示购物车中单个物品的信息和交互功能
/// 继承自Unity的MonoBehaviour基类，可挂载到游戏对象上使用
/// </summary>
public class ShoppingCartItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText;
    
    /// <summary>
    /// 设置购物车物品的显示文本
    /// </summary>
    /// <param name="newString">要设置的新文本内容</param>
    public void SetItemText(string newString)
    {
        itemText.text = newString;
    }
}