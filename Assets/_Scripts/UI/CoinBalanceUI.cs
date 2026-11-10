using UnityEngine;
using TMPro;

public class CoinBalanceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        label.text = CoinWallet.GetBalance() + " Coins";
    }
}
