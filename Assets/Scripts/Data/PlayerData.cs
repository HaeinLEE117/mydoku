using System;

[Serializable]
public class PlayerData
{
    public int currentLevel;
    public int highestLevelUnlocked;
    public int totalStarsCollected;
    public string lastPlayedDate;

    // 레벨별 클리어 정보 (예: 별 개수, 클리어 시간 등)
    public LevelInfo[] levelInfos;

    public PlayerData()
    {
        currentLevel = 1;
        highestLevelUnlocked = 1;
        totalStarsCollected = 0;
        lastPlayedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        levelInfos = new LevelInfo[100]; // 최대 100개 레벨

        for (int i = 0; i < levelInfos.Length; i++)
        {
            levelInfos[i] = new LevelInfo();
        }
    }
}

[Serializable]
public class LevelInfo
{
    public bool isCleared;
    public int stars;
    public int attempts;

    public LevelInfo()
    {
        isCleared = false;
        stars = 0;
        attempts = 0;
    }
}