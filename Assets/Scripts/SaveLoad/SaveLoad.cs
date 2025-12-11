using System.IO;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 保存和加载游戏数据的工具类
/// </summary>
public static class SaveLoad
{
    /// <summary>
    /// 游戏保存时触发的事件
    /// </summary>
    public static UnityAction OnSaveGame;
    
    /// <summary>
    /// 游戏加载时触发的事件
    /// </summary>
    public static UnityAction<SaveData> OnLoadGame;

    private static string directory = "/SaveData/";
    private static string fileName = "SaveGame.sav";

    /// <summary>
    /// 保存游戏数据到文件
    /// </summary>
    /// <param name="data">要保存的游戏数据对象</param>
    /// <returns>保存操作是否成功完成</returns>
    public static bool Save(SaveData data)
    {
        OnSaveGame?.Invoke();

        string dir = Application.persistentDataPath + directory;

        // 将保存目录路径复制到系统剪贴板中
        // TODO: 记得删除
        GUIUtility.systemCopyBuffer = dir;
        
        // 检查并创建保存数据的目录
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        // 将数据序列化为JSON格式并写入文件
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(dir + fileName, json);
        
        Debug.Log("Saving game");
        
        return true;
    }

    /// <summary>
    /// 从文件加载游戏数据
    /// </summary>
    /// <returns>加载的游戏数据对象，如果文件不存在则返回新的空数据对象</returns>
    public static SaveData Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        SaveData data = new SaveData();

        // 检查保存文件是否存在
        if (File.Exists(fullPath))
        {
            // 读取并反序列化保存的数据
            string json = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<SaveData>(json);
            
            OnLoadGame?.Invoke(data);
        }
        else
        {
            Debug.Log("Save file does not exist!");
        }
        
        return data;
    }

    /// <summary>
    /// 删除已保存的游戏数据文件
    /// </summary>
    public static void DeleteSaveData()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        
        // 如果保存文件存在则删除它
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}