using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 定义可交互对象的接口，用于统一处理游戏中的交互逻辑
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// 获取或设置交互完成时触发的事件回调
    /// </summary>
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    
    /// <summary>
    /// 执行交互操作的核心方法
    /// </summary>
    /// <param name="interactor">执行交互的交互者对象</param>
    /// <param name="interactSuccessful">输出参数，指示交互是否成功执行</param>
    public void Interact(Interactor interactor, out bool interactSuccessful);
    
    /// <summary>
    /// 结束当前交互操作，用于清理交互状态或资源
    /// </summary>
    public void EndInteraction();
}