using UnityEngine;
using System.IO;

public class SaveManager
{
    private const string SAVE_FILE_NAME = "PlayerData.json";
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    public static void SavePlayerData(PlayerData data)
    {
        try
        {
            string jsonData = JsonUtility.ToJson(data, true); // true = prettyPrint (읽기 쉽게)
            File.WriteAllText(SaveFilePath, jsonData);
            Debug.Log($"Game data saved successfully to: {SaveFilePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save player data: {e.Message}");
        }
    }

    public static PlayerData LoadPlayerData()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                string jsonData = File.ReadAllText(SaveFilePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
                Debug.Log($"Game data loaded from: {SaveFilePath}");
                return data;
            }
            else
            {
                Debug.Log("No save file found. Creating new player data.");
                PlayerData newData = new PlayerData();
                SavePlayerData(newData);
                return newData;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load player data: {e.Message}");
            return new PlayerData();
        }
    }

    public static void DeleteSaveData()
    {
        try
        {
            if (File.Exists(SaveFilePath))
            {
                File.Delete(SaveFilePath);
                Debug.Log("Save data deleted successfully.");
            }
            else
            {
                Debug.Log("No save file to delete.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to delete save data: {e.Message}");
        }
    }

    public static bool HasSaveData()
    {
        return File.Exists(SaveFilePath);
    }

    public static string GetSaveFilePath()
    {
        return SaveFilePath;
    }
}
