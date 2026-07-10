using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [Header("UI Panels")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject gamePanel;

    [Header("Loading UI")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Main UI")]
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Game State")]
    private bool _isPlaying = false;
    private bool _isInitialized = false;

    [Header("Player Data")]
    private PlayerData _playerData;
    public PlayerData PlayerData => _playerData;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (!_isInitialized)
        {
            StartCoroutine(InitializeGame());
        }
    }

    private IEnumerator InitializeGame()
    {
        // 로딩 패널만 활성화
        loadingPanel.SetActive(true);
        mainPanel.SetActive(false);
        gamePanel.SetActive(false);

        // 초기화 작업들
        yield return StartCoroutine(LoadResources());

        // 플레이어 데이터 로드
        LoadPlayerData();

        // 로딩 완료
        _isInitialized = true;

        // 메인 메뉴로 전환
        yield return new WaitForSeconds(0.5f);
        loadingPanel.SetActive(false);
        mainPanel.SetActive(true);

        // 레벨 UI 업데이트
        UpdateLevelUI();
    }

    private IEnumerator LoadResources()
    {
        float progress = 0f;

        // 1단계: 게임 데이터 초기화 (30%)
        UpdateLoadingUI(0f, "Initializing...");
        yield return new WaitForSeconds(0.3f);
        progress = 0.3f;
        UpdateLoadingUI(progress, "Loading Game Data...");

        // 2단계: 스도쿠 퍼즐 데이터 로드 (30% -> 60%)
        yield return new WaitForSeconds(0.4f);
        progress = 0.6f;
        UpdateLoadingUI(progress, "Loading Puzzles...");

        // 3단계: 사운드 및 리소스 로드 (60% -> 90%)
        yield return new WaitForSeconds(0.3f);
        progress = 0.9f;
        UpdateLoadingUI(progress, "Loading Resources...");

        // 4단계: 저장된 게임 데이터 로드 (90% -> 100%)
        yield return new WaitForSeconds(0.2f);
        progress = 1f;
        UpdateLoadingUI(progress, "Complete!");

        yield return new WaitForSeconds(0.3f);
    }

    private void UpdateLoadingUI(float progress, string message)
    {
        if (loadingBar != null)
        {
            loadingBar.value = progress;
        }

        if (loadingText != null)
        {
            loadingText.text = $"{message} {Mathf.RoundToInt(progress * 100)}%";
        }
    }

    #region Player Data Management

    private void LoadPlayerData()
    {
        _playerData = SaveManager.LoadPlayerData();
        Debug.Log($"Player data loaded. Current Level: {_playerData.currentLevel}, Highest Level: {_playerData.highestLevelUnlocked}");
        UpdateLevelUI();
    }

    private void SavePlayerData()
    {
        if (_playerData != null)
        {
            _playerData.lastPlayedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SaveManager.SavePlayerData(_playerData);
        }
    }

    private void UpdateLevelUI()
    {
        if (levelText != null && _playerData != null)
        {
            levelText.text = $"Level {_playerData.currentLevel}";
        }
    }

    public void SetCurrentLevel(int levelNumber)
    {
        _playerData.currentLevel = levelNumber;
        SavePlayerData();
        UpdateLevelUI();
    }

    public int GetCurrentLevel()
    {
        return _playerData.currentLevel;
    }

    public void UnlockLevel(int levelNumber)
    {
        if (levelNumber > _playerData.highestLevelUnlocked)
        {
            _playerData.highestLevelUnlocked = levelNumber;
            SavePlayerData();
            Debug.Log($"Level {levelNumber} unlocked!");
        }
    }

    public bool IsLevelUnlocked(int levelNumber)
    {
        return levelNumber <= _playerData.highestLevelUnlocked;
    }

    public void CompleteLevel(int levelNumber, int stars, float clearTime)
    {
        if (levelNumber >= 0 && levelNumber < _playerData.levelInfos.Length)
        {
            LevelInfo info = _playerData.levelInfos[levelNumber - 1];
            info.isCleared = true;
            info.attempts++;

            // 더 높은 별점을 받았다면 업데이트
            if (stars > info.stars)
            {
                info.stars = stars;
            }

            // 다음 레벨 언락
            UnlockLevel(levelNumber + 1);

            SavePlayerData();
            UpdateLevelUI();
            Debug.Log($"Level {levelNumber} completed! Stars: {stars}, Time: {clearTime:F2}s");
        }
    }

    public LevelInfo GetLevelInfo(int levelNumber)
    {
        if (levelNumber >= 1 && levelNumber <= _playerData.levelInfos.Length)
        {
            return _playerData.levelInfos[levelNumber - 1];
        }
        return null;
    }

    public void ResetAllProgress()
    {
        SaveManager.DeleteSaveData();
        _playerData = new PlayerData();
        SavePlayerData();
        UpdateLevelUI();
        Debug.Log("All progress has been reset.");
    }

    #endregion

    #region Panel Management

    public void StartGame()
    {
        mainPanel.SetActive(false);
        gamePanel.SetActive(true);
        _isPlaying = true;

        Debug.Log($"Starting game at level {_playerData.currentLevel}");
        // 여기서 현재 레벨의 퍼즐을 로드하는 로직 추가
    }

    public void BackToMenu()
    {
        gamePanel.SetActive(false);
        mainPanel.SetActive(true);
        _isPlaying = false;

        // 메뉴로 돌아갈 때 데이터 저장
        SavePlayerData();
        UpdateLevelUI();
    }

    #endregion

    public bool IsPlaying => _isPlaying;

    private void OnApplicationQuit()
    {
        // 게임 종료 시 데이터 저장
        SavePlayerData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            // 앱이 백그라운드로 갈 때 데이터 저장 (모바일용)
            SavePlayerData();
        }
    }
}
