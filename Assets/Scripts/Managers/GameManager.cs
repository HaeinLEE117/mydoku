using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [Header("UI Panels")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;

    [Header("Loading UI")]
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TextMeshProUGUI loadingText;

    [Header("Game State")]
    private bool _isPlaying = false;
    private bool _isInitialized = false;

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
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(false);

        // 초기화 작업들
        yield return StartCoroutine(LoadResources());

        // 로딩 완료
        _isInitialized = true;

        // 메인 메뉴로 전환
        yield return new WaitForSeconds(0.5f);
        loadingPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
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

    public void StartGame()
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        _isPlaying = true;
    }

    public void BackToMenu()
    {
        gamePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        _isPlaying = false;
    }

    public bool IsPlaying => _isPlaying;
}
