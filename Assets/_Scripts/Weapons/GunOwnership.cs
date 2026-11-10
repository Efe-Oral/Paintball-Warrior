using UnityEngine;

public static class GunOwnership
{
    private const string OwnedKeyPrefix = "gun_owned_";

    public static bool IsOwned(GunData gun)
    {
        return gun.price <= 0 || PlayerPrefs.GetInt(OwnedKeyPrefix + gun.id, 0) == 1;
    }

    public static void Unlock(GunData gun)
    {
        PlayerPrefs.SetInt(OwnedKeyPrefix + gun.id, 1);
        PlayerPrefs.Save();
    }
}
