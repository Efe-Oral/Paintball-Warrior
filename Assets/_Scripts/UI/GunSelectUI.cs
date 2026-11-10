using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GunSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private GunData[] availableGuns;
    [SerializeField] private Button[] gunButtons;
    [SerializeField] private TMP_Text[] gunNameLabels;
    [SerializeField] private Image[] gunSelectionHighlights;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button backButton;

    private string pendingSceneName;

    private void Awake()
    {
        for (int i = 0; i < gunButtons.Length; i++)
        {
            int index = i;
            gunButtons[i].onClick.AddListener(() => SelectGun(index));
            gunNameLabels[i].text = availableGuns[i].displayName;
        }

        playButton.onClick.AddListener(Play);
        backButton.onClick.AddListener(Close);
        panelRoot.SetActive(false);
    }

    public void Open(string sceneName)
    {
        pendingSceneName = sceneName;
        SelectGun(0);
        panelRoot.SetActive(true);
    }

    private void SelectGun(int index)
    {
        GunSelection.SelectedGun = availableGuns[index];
        descriptionText.text = availableGuns[index].description;

        for (int i = 0; i < gunSelectionHighlights.Length; i++)
        {
            gunSelectionHighlights[i].enabled = i == index;
        }
    }

    private void Play()
    {
        SceneManager.LoadScene(pendingSceneName);
    }

    private void Close()
    {
        panelRoot.SetActive(false);
    }
}
