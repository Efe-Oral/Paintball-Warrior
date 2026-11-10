using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject panelRoot;
    [SerializeField] private WeaponRoster weaponRoster;
    [SerializeField] private TMP_Text coinBalanceText;
    [SerializeField] private RectTransform listParent;
    [SerializeField] private GameObject entryTemplate;
    [SerializeField] private Button backButton;
    [SerializeField] private CoinBalanceUI coinBalanceUI;

    private void Awake()
    {
        backButton.onClick.AddListener(Close);
        panelRoot.SetActive(false);
    }

    public void Open()
    {
        Refresh();
        panelRoot.SetActive(true);
    }

    private void Refresh()
    {
        coinBalanceText.text = CoinWallet.GetBalance() + " Coins";

        for (int i = listParent.childCount - 1; i >= 0; i--)
        {
            Transform child = listParent.GetChild(i);
            if (child.gameObject != entryTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        entryTemplate.SetActive(false);

        foreach (GunData gun in weaponRoster.guns)
        {
            CreateEntry(gun);
        }
    }

    private void CreateEntry(GunData gun)
    {
        GameObject entry = Instantiate(entryTemplate, listParent);
        entry.SetActive(true);

        TMP_Text nameLabel = null;
        TMP_Text priceLabel = null;
        foreach (TMP_Text label in entry.GetComponentsInChildren<TMP_Text>())
        {
            if (label.name == "ShopNameLabel")
            {
                nameLabel = label;
            }
            else if (label.name == "ShopPriceLabel")
            {
                priceLabel = label;
            }
        }

        nameLabel.text = gun.displayName;

        Button buyButton = entry.GetComponentInChildren<Button>();
        bool owned = GunOwnership.IsOwned(gun);

        if (owned)
        {
            priceLabel.text = "Owned";
            buyButton.interactable = false;
        }
        else
        {
            priceLabel.text = gun.price + " coins";
            buyButton.interactable = CoinWallet.GetBalance() >= gun.price;
            buyButton.onClick.AddListener(() => Buy(gun));
        }
    }

    private void Buy(GunData gun)
    {
        if (CoinWallet.TrySpend(gun.price))
        {
            GunOwnership.Unlock(gun);
            Refresh();
            coinBalanceUI?.Refresh();
        }
    }

    private void Close()
    {
        panelRoot.SetActive(false);
    }
}
