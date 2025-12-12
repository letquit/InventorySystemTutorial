using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// 库存UI控制器类，负责管理游戏中库存界面的显示和交互
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    [FormerlySerializedAs("chestPanel")] public DynamicInventoryDisplay inventoryPanel;
    public DynamicInventoryDisplay playerBackpackPanel;

    /// <summary>
    /// 在对象唤醒时执行初始化操作，将库存面板和背包面板设置为非激活状态
    /// </summary>
    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
        playerBackpackPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 当组件启用时注册事件监听器，监听动态库存显示请求事件
    /// </summary>
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
    }
    
    /// <summary>
    /// 当组件禁用时注销事件监听器，避免内存泄漏
    /// </summary>
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
    }

    /// <summary>
    /// 每帧检查输入，处理ESC键按下时关闭激活的库存面板
    /// </summary>
    private void Update()
    {
        // 检查ESC键是否被按下，如果库存面板处于激活状态则将其关闭
        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            inventoryPanel.gameObject.SetActive(false);
        
        // 检查ESC键是否被按下，如果玩家背包面板处于激活状态则将其关闭
        if (playerBackpackPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            playerBackpackPanel.gameObject.SetActive(false);

    }
    
    /// <summary>
    /// 显示指定的库存系统界面
    /// </summary>
    /// <param name="invToDisplay">需要显示的库存系统实例</param>
    /// <param name="offset">库存显示的偏移量</param>
    private void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
}