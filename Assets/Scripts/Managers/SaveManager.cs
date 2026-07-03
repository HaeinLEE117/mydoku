using UnityEngine;

public class SaveManager
{
    private const string SAVE_KEY = "PlayerData";

    //TODO: Change saver location from registry PlayerData to file with json format. target flatform: Android.

    public static void SavePlayerData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
    }

    public static PlayerData LoadPlayerData()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            return data;
        }
        else
        {
            PlayerData pd = new PlayerData();
            SavePlayerData(pd);
            return pd;
        }
    }

    public static void DeleteSaveData()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            PlayerPrefs.DeleteKey(SAVE_KEY);
            PlayerPrefs.Save();
        }
    }

    public static bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }
}
