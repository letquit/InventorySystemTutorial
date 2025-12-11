using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// 库存用户界面控制器，负责管理箱子库存面板和背包库存面板的显示和交互
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay chestPanel;
    public DynamicInventoryDisplay playerBackpackPanel;

    /// <summary>
    /// 在对象唤醒时执行，初始化箱子库存面板和背包库存面板为隐藏状态
    /// </summary>
    private void Awake()
    {
        chestPanel.gameObject.SetActive(false);
        playerBackpackPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 当脚本启用时注册箱子和背包显示事件监听器
    /// </summary>
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested += DisplayPlayerBackpack;
    }
    
    /// <summary>
    /// 当脚本禁用时注销箱子和背包显示事件监听器
    /// </summary>
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerBackpackDisplayRequested -= DisplayPlayerBackpack;
    }

    /// <summary>
    /// 每帧检查输入，处理库箱子存面板和背包库存面板的关闭操作
    /// </summary>
    private void Update()
    {
        // 当箱子库存面板处于激活状态且按下ESC键时，隐藏箱子库存面板
        if (chestPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            chestPanel.gameObject.SetActive(false);
        
        // 检查玩家背包库存面板是否处于激活状态，如果是且按下了ESC键，则关闭背包库存面板
        if (playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            playerBackpackPanel.gameObject.SetActive(false);

    }
    
    /// <summary>
    /// 显示指定的箱子库存系统内容
    /// </summary>
    /// <param name="invToDisplay">需要显示的箱子库存系统实例</param>
    private void DisplayInventory(InventorySystem invToDisplay)
    {
        chestPanel.gameObject.SetActive(true);
        chestPanel.RefreshDynamicInventory(invToDisplay);
    }
    
    /// <summary>
    /// 显示玩家背包库存面板，并刷新其内容
    /// </summary>
    /// <param name="invToDisplay">需要显示在玩家背包中的库存系统实例</param>
    private void DisplayPlayerBackpack(InventorySystem invToDisplay)
    {
        playerBackpackPanel.gameObject.SetActive(true);
        playerBackpackPanel.RefreshDynamicInventory(invToDisplay);
    }
}