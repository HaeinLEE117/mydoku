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
        Debug.Log("Game data saved successfully.");
        Debug.Log("PlayerPrefs 저장 위치: HKEY_CURRENT_USER\\Software\\" +
          Application.companyName + "\\" + Application.productName);

    }

    public static PlayerData LoadPlayerData()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string jsonData = PlayerPrefs.GetString(SAVE_KEY);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log($"Game data loaded. Current Level: {data.currentLevel}");
            return data;
        }
        else
        {
            Debug.Log("No save data found. Creating new player data.");
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
            Debug.Log("Save data deleted.");
        }
    }

    public static bool HasSaveData()
    {
        return PlayerPrefs.HasKey(SAVE_KEY);
    }
}
