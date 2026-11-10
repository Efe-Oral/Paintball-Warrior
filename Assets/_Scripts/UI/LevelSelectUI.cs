using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private LevelChain levelChain;
    [SerializeField] private RectTransform listParent;
    [SerializeField] private GameObject buttonTemplate;

    private void Start()
    {
        buttonTemplate.SetActive(false);

        for (int i = 0; i < levelChain.levels.Length; i++)
        {
            CreateEntry(i);
        }
    }

    private void CreateEntry(int levelIndex)
    {
        LevelData level = levelChain.levels[levelIndex];
        bool unlocked = LevelProgress.IsUnlocked(levelIndex);
        int stars = LevelProgress.GetStars(levelIndex);

        GameObject entry = Instantiate(buttonTemplate, listParent);
        entry.SetActive(true);

        foreach (TMP_Text label in entry.GetComponentsInChildren<TMP_Text>())
        {
            if (label.name == "NameLabel")
            {
                label.text = level.displayName;
            }
            else if (label.name == "StarsLabel")
            {
                label.text = unlocked ? BuildStarString(stars) : "LOCKED";
            }
        }

        Button button = entry.GetComponent<Button>();
        button.interactable = unlocked;
        if (unlocked)
        {
            string sceneName = level.sceneName;
            button.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
        }

        CanvasGroup group = entry.GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.alpha = unlocked ? 1f : 0.5f;
        }
    }

    private static string BuildStarString(int stars)
    {
        string result = string.Empty;
        for (int i = 0; i < 3; i++)
        {
            result += i < stars ? "★" : "☆";
        }
        return result;
    }
}
