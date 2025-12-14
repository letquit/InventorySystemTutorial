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
    [ReadOnly, SerializeField] private string id;

    [SerializeField]
    private static SerializableDictionary<string, GameObject> idDatabase =
        new SerializableDictionary<string, GameObject>();
    
    /// <summary>
    /// 获取当前对象的唯一标识符
    /// </summary>
    /// <returns>返回表示唯一ID的字符串</returns>
    public string ID => id;

    /// <summary>
    /// Unity生命周期函数，在对象被加载时调用。用于初始化ID数据库并管理游戏对象的注册。
    /// </summary>
    private void Awake()
    {
        // 初始化ID数据库，如果为空则创建新的可序列化字典
        if (idDatabase == null)
            idDatabase = new SerializableDictionary<string, GameObject>();
        
        // 检查当前ID是否已存在于数据库中，如果存在则执行生成逻辑，否则将当前对象添加到数据库中
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
    [ContextMenu("Generate ID")]
    private void Generate()
    {
        id = Guid.NewGuid().ToString();
        idDatabase.Add(id, this.gameObject);
    }
}