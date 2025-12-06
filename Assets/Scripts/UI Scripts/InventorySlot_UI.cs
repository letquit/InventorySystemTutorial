using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 库存槽位UI类，负责显示和管理单个库存槽位的可视化界面
/// </summary>
public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;
    
    /// <summary>
    /// 获取已分配的库存槽位
    /// </summary>
    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    
    /// <summary>
    /// 获取父级库存显示组件
    /// </summary>
    public InventoryDisplay ParentDisplay { get; private set; }

    /// <summary>
    /// 组件唤醒时初始化UI槽位
    /// 清空槽位显示并绑定点击事件
    /// </summary>
    private void Awake()
    {
        ClearSlot();
        
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);
        
        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    /// <summary>
    /// 初始化UI槽位，将指定的库存槽位与UI关联
    /// </summary>
    /// <param name="slot">要关联的库存槽位数据</param>
    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }
    
    /// <summary>
    /// 更新UI槽位显示，根据库存槽位数据更新图标和数量
    /// </summary>
    /// <param name="slot">用于更新显示的库存槽位数据</param>
    public void UpdateUISlot(InventorySlot slot)
    {
        // 如果槽位有物品数据则显示图标和数量
        if (slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.icon;
            itemSprite.color = Color.white;
            
            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = "";
        }
        else
        {
            ClearSlot();
        }
    }

    /// <summary>
    /// 更新当前已分配槽位的UI显示
    /// </summary>
    public void UpdateSlot()
    {
        if (assignedInventorySlot != null) UpdateUISlot(assignedInventorySlot);
    }

    /// <summary>
    /// 清空槽位显示，重置图标、颜色和文本
    /// </summary>
    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }
    
    /// <summary>
    /// UI槽位点击事件处理方法，通知父级显示组件槽位被点击
    /// </summary>
    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}