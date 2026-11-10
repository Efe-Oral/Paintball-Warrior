using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelResultUI : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;
    [SerializeField] private LevelController levelController;
    [SerializeField] private LevelChain levelChain;
    [SerializeField] private int levelIndex;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private Button winRetryButton;
    [SerializeField] private Button winNextButton;
    [SerializeField] private Button winMenuButton;

    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMP_Text loseText;
    [SerializeField] private Button loseRetryButton;
    [SerializeField] private Button loseMenuButton;

    private void Awake()
    {
        winRetryButton.onClick.AddListener(Retry);
        winMenuButton.onClick.AddListener(GoToMenu);
        loseRetryButton.onClick.AddListener(Retry);
        loseMenuButton.onClick.AddListener(GoToMenu);

        bool hasNextLevel = winNextButton != null && levelChain != null && levelIndex + 1 < levelChain.levels.Length;
        if (winNextButton != null)
        {
            winNextButton.gameObject.SetActive(hasNextLevel);
        }
        if (hasNextLevel)
        {
            winNextButton.onClick.AddListener(GoToNextLevel);
        }
    }

    private void OnEnable()
    {
        levelController.LevelWon += HandleLevelWon;
        levelController.LevelLost += HandleLevelLost;
    }

    private void OnDisable()
    {
        levelController.LevelWon -= HandleLevelWon;
        levelController.LevelLost -= HandleLevelLost;
    }

    private void HandleLevelWon(int stars, float coveragePercent)
    {
        LevelProgress.ReportResult(levelIndex, stars);

        int coinsEarned = config.coinsPerStar * stars;
        CoinWallet.Add(coinsEarned);

        int percent = Mathf.FloorToInt(coveragePercent * 100f);
        winText.text = "Level Complete!\nCoverage: " + percent + "%\nStars: " + stars + "/3\nCoins: +" + coinsEarned;
        winPanel.SetActive(true);
    }

    private void HandleLevelLost(float coveragePercent)
    {
        int percent = Mathf.FloorToInt(coveragePercent * 100f);
        int thresholdPercent = Mathf.FloorToInt(config.coverageThreshold * 100f);
        loseText.text = "Out of Paint!\nCoverage: " + percent + "% (needed " + thresholdPercent + "%)";
        losePanel.SetActive(true);
    }

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void GoToNextLevel()
    {
        SceneManager.LoadScene(levelChain.levels[levelIndex + 1].sceneName);
    }
}
