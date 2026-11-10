using UnityEngine;

[CreateAssetMenu(fileName = "WeaponRoster", menuName = "PaintGame/Weapon Roster")]
public class WeaponRoster : ScriptableObject
{
    public GunData[] guns;
}
