using System;
using UnityEngine;

/// <summary>
/// UniqueID类用于为游戏对象分配唯一的标识符
/// 该类可以在编辑器模式下运行，确保每个游戏对象都有一个唯一的GUID
/// </summary>
[Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string id = Guid.NewGuid().ToString();

    [SerializeField]
    private static SerializableDictionary<string, GameObject> idDatabase =
        new SerializableDictionary<string, GameObject>();
    
    /// <summary>
    /// 获取当前对象的唯一标识符
    /// </summary>
    /// <returns>返回表示唯一ID的字符串</returns>
    public string ID => id;

    /// <summary>
    /// 当脚本在编辑器中被验证时调用
    /// 检查当前ID是否已存在于数据库中，如果存在则生成新的ID
    /// </summary>
    private void OnValidate()
    {
        if (idDatabase.ContainsKey(id)) Generate();
        else idDatabase.Add(id, this.gameObject);
    }
    
    /// <summary>
    /// 当对象被销毁时调用
    /// 从ID数据库中移除当前对象的ID记录
    /// </summary>
    private void OnDestroy()
    {
        if (idDatabase.ContainsKey(id)) idDatabase.Remove(id);
    }
    
    /// <summary>
    /// 生成新的唯一标识符并将其添加到数据库中
    /// </summary>
    private void Generate()
    {
        id = Guid.NewGuid().ToString();
        idDatabase.Add(id, this.gameObject);
    }
}