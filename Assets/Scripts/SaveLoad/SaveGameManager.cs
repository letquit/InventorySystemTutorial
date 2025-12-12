using UnityEngine;

/// <summary>
/// 存档管理器类，负责游戏数据的保存和加载功能
/// </summary>
public class SaveGameManager : MonoBehaviour
{
    /// <summary>
    /// 静态存档数据实例，用于全局访问当前游戏数据
    /// </summary>
    public static SaveData Data;
    
    /// <summary>
    /// Unity生命周期函数，在对象创建时自动调用
    /// 负责初始化加载存档数据并注册数据加载事件监听器
    /// </summary>
    private void Awake()
    {
        Data = new SaveData();
        SaveLoad.OnLoadGame += LoadData;
    }

    /// <summary>
    /// 删除游戏存档数据
    /// 调用SaveLoad类的删除方法清除所有保存的数据
    /// </summary>
    public void DeleteData()
    {
        SaveLoad.DeleteSaveData();
    }

    /// <summary>
    /// 保存游戏数据到存储设备
    /// 将当前内存中的数据序列化并持久化保存
    /// </summary>
    public static void SaveData()
    {
        var saveData = Data;
        
        SaveLoad.Save(saveData);
    }
    
    /// <summary>
    /// 加载指定的游戏数据到内存中
    /// </summary>
    /// <param name="data">要加载的存档数据对象</param>
    public static void LoadData(SaveData data)
    {
        Data = data; 
    }

    /// <summary>
    /// 尝试从存储设备加载游戏数据
    /// 触发SaveLoad类的加载流程
    /// </summary>
    public static void TryLoadData()
    {
        SaveLoad.Load();
    }
}