using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可序列化的字典类，继承自Dictionary并实现Unity的序列化回调接口
/// 用于在Unity编辑器中保存和加载字典数据
/// </summary>
/// <typeparam name="TKey">字典键的类型</typeparam>
/// <typeparam name="TValue">字典值的类型</typeparam>
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();

    [SerializeField] private List<TValue> values = new List<TValue>();

    /// <summary>
    /// 在序列化之前调用，将字典中的键值对保存到列表中
    /// </summary>
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    /// <summary>
    /// 在反序列化之后调用，从列表中加载数据到字典
    /// </summary>
    public void OnAfterDeserialize()
    {
        this.Clear();

        // 检查键和值列表的数量是否匹配
        if (keys.Count != values.Count)
            throw new System.Exception(string.Format(
                "there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        // 将键值对添加到字典中
        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}