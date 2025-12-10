using System;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 交互器类，用于处理玩家与场景中可交互对象的交互逻辑
/// </summary>
public class Interactor : MonoBehaviour
{
    public Transform interactionPoint;
    public LayerMask interactionLayer;
    public float interactionPointRadius = 1f;
    public bool IsInteracting { get; private set; }

    /// <summary>
    /// 每帧更新检测交互逻辑
    /// 检测交互点周围的碰撞体，当按下E键时尝试与可交互对象进行交互
    /// </summary>
    private void Update()
    {
        // 检测交互点周围指定图层的碰撞体
        var colliders = Physics.OverlapSphere(interactionPoint.position, interactionPointRadius, interactionLayer);

        // 当按下E键时，遍历所有检测到的碰撞体并尝试交互
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null) StartInteraction(interactable);
            }
        }
    }
    
    /// <summary>
    /// 开始与指定的可交互对象进行交互
    /// </summary>
    /// <param name="interactable">要交互的可交互对象接口</param>
    private void StartInteraction(IInteractable interactable)
    {
        interactable.Interact(this, out bool interactSuccessful);
        IsInteracting = true;
    }
    
    /// <summary>
    /// 结束当前的交互过程
    /// </summary>
    private void EndInteraction()
    {
        IsInteracting = false;
    }
}