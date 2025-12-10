using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

/// <summary>
/// 库存用户界面控制器，负责管理库存面板的显示和交互
/// </summary>
public class InventoryUIController : MonoBehaviour
{
    public DynamicInventoryDisplay inventoryPanel;

    /// <summary>
    /// 在对象唤醒时执行，初始化库存面板为隐藏状态
    /// </summary>
    private void Awake()
    {
        inventoryPanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// 当脚本启用时注册库存显示事件监听器
    /// </summary>
    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
    }
    
    /// <summary>
    /// 当脚本禁用时注销库存显示事件监听器
    /// </summary>
    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
    }

    /// <summary>
    /// 每帧检查输入，处理库存面板的关闭操作
    /// </summary>
    private void Update()
    {
        // 当库存面板处于激活状态且按下ESC键时，隐藏库存面板
        if (inventoryPanel.gameObject.activeInHierarchy && Keyboard.current.escapeKey.wasPressedThisFrame)
            inventoryPanel.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// 显示指定的库存系统内容
    /// </summary>
    /// <param name="invToDisplay">需要显示的库存系统实例</param>
    private void DisplayInventory(InventorySystem invToDisplay)
    {
        inventoryPanel.gameObject.SetActive(true);
        inventoryPanel.RefreshDynamicInventory(invToDisplay);
    }
}