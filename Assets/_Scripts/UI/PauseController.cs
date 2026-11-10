using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PaintSprayer paintSprayer;
    [SerializeField] private LevelTimer timer;
    [SerializeField] private LevelController levelController;

    [SerializeField] private Button pauseButton;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button menuButton;

    private void Awake()
    {
        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Resume);
        retryButton.onClick.AddListener(Retry);
        menuButton.onClick.AddListener(GoToMenu);

        levelController.LevelWon += HandleLevelWon;
        levelController.LevelLost += HandleLevelLost;

        pausePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        levelController.LevelWon -= HandleLevelWon;
        levelController.LevelLost -= HandleLevelLost;
    }

    private void Pause()
    {
        playerMovement.enabled = false;
        paintSprayer.enabled = false;
        timer.enabled = false;
        pausePanel.SetActive(true);
    }

    private void Resume()
    {
        playerMovement.enabled = true;
        paintSprayer.enabled = true;
        timer.enabled = true;
        pausePanel.SetActive(false);
    }

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void HandleLevelWon(int stars, float coveragePercent)
    {
        pauseButton.gameObject.SetActive(false);
    }

    private void HandleLevelLost(float coveragePercent)
    {
        pauseButton.gameObject.SetActive(false);
    }
}
