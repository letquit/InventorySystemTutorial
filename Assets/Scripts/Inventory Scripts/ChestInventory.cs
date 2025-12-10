using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 箱子库存类，继承自InventoryHolder并实现IInteractable接口，用于管理箱子的物品存储和交互功能
/// </summary>
public class ChestInventory : InventoryHolder, IInteractable
{
    /// <summary>
    /// 交互完成时触发的Unity事件回调
    /// </summary>
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    
    /// <summary>
    /// 处理与箱子的交互逻辑，当玩家与箱子互动时调用此方法
    /// </summary>
    /// <param name="interactor">执行交互的交互者对象</param>
    /// <param name="interactSuccessful">输出参数，指示交互是否成功执行</param>
    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        // 触发动态库存显示请求事件，显示箱子的库存界面
        OnDynamicInventoryDisplayRequested?.Invoke(InventorySystem);
        interactSuccessful = true;
    }

    /// <summary>
    /// 结束与箱子的交互，清理交互状态
    /// </summary>
    public void EndInteraction()
    {
        
    }
}