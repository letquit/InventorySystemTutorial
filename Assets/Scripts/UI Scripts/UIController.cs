using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// UI控制器类，负责管理游戏中的用户界面显示逻辑
/// </summary>
public class UIController : MonoBehaviour
{
    [SerializeField] private ShopKeeperDisplay shopKeeperDisplay;

    /// <summary>
    /// 在对象唤醒时执行初始化操作，将商店显示界面设置为非激活状态
    /// </summary>
    private void Awake()
    {
        // 初始化时隐藏商店显示界面
        shopKeeperDisplay.gameObject.SetActive(false);
    }

    /// <summary>
    /// 当脚本启用时注册事件监听器，监听商店窗口请求事件
    /// </summary>
    private void OnEnable()
    {
        // 订阅商店窗口请求事件
        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
    }
    
    /// <summary>
    /// 当脚本禁用时取消事件监听器，防止内存泄漏
    /// </summary>
    private void OnDisable()
    {
        // 取消订阅商店窗口请求事件
        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
    }

    /// <summary>
    /// 每帧检查键盘输入，当按下ESC键时隐藏商店界面
    /// </summary>
    private void Update()
    {
        // 检测ESC键是否在当前帧被按下，如果是则关闭商店显示界面
        if (Keyboard.current.escapeKey.wasPressedThisFrame) shopKeeperDisplay.gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示商店窗口界面
    /// </summary>
    /// <param name="shopSystem">商店系统实例，包含商店的商品信息</param>
    /// <param name="playerInventory">玩家背包持有者，用于处理玩家物品交互</param>
    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory)
    {
        // 激活商店显示界面
        shopKeeperDisplay.gameObject.SetActive(true);
        // 调用显示商店窗口方法
        shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory);
    }
}