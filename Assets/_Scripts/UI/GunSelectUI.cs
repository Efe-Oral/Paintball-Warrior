using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GunSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private WeaponRoster weaponRoster;
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
        }

        playButton.onClick.AddListener(Play);
        backButton.onClick.AddListener(Close);
        panelRoot.SetActive(false);
    }

    public void Open(string sceneName)
    {
        pendingSceneName = sceneName;
        RefreshButtons();
        SelectGun(0);
        panelRoot.SetActive(true);
    }

    private void RefreshButtons()
    {
        GunData[] guns = weaponRoster.guns;
        for (int i = 0; i < gunButtons.Length && i < guns.Length; i++)
        {
            bool owned = GunOwnership.IsOwned(guns[i]);
            gunButtons[i].interactable = owned;
            gunNameLabels[i].text = owned ? guns[i].displayName : guns[i].displayName + "\n(Locked)";
        }
    }

    private void SelectGun(int index)
    {
        GunData gun = weaponRoster.guns[index];
        GunSelection.SelectedGun = gun;
        descriptionText.text = gun.description;

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
