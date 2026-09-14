using UnityEngine;
using TMPro;

public class LevelResultUI : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;
    [SerializeField] private LevelController levelController;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TMP_Text loseText;

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
        int percent = Mathf.FloorToInt(coveragePercent * 100f);
        winText.text = "Level Complete!\nCoverage: " + percent + "%\nStars: " + stars + "/3\n\nPress R to Restart";
        winPanel.SetActive(true);
    }

    private void HandleLevelLost(float coveragePercent)
    {
        int percent = Mathf.FloorToInt(coveragePercent * 100f);
        int thresholdPercent = Mathf.FloorToInt(config.coverageThreshold * 100f);
        loseText.text = "Out of Paint!\nCoverage: " + percent + "% (needed " + thresholdPercent + "%)\n\nPress R to Restart";
        losePanel.SetActive(true);
    }
}
