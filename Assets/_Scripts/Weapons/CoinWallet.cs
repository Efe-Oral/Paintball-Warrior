using UnityEngine;

public static class CoinWallet
{
    private const string BalanceKey = "coin_balance";

    public static int GetBalance()
    {
        return PlayerPrefs.GetInt(BalanceKey, 0);
    }

    public static void Add(int amount)
    {
        PlayerPrefs.SetInt(BalanceKey, GetBalance() + amount);
        PlayerPrefs.Save();
    }

    public static bool TrySpend(int amount)
    {
        int balance = GetBalance();
        if (balance < amount)
        {
            return false;
        }

        PlayerPrefs.SetInt(BalanceKey, balance - amount);
        PlayerPrefs.Save();
        return true;
    }
}
