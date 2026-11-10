using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "PaintGame/Gun")]
public class GunData : ScriptableObject
{
    [Tooltip("Stable key used for save-data persistence. Do not change once shipped.")]
    public string id;
    public string displayName;
    [TextArea] public string description;

    [Header("Economy")]
    [Tooltip("Coin cost in the shop. 0 = free / always owned.")]
    public int price;

    [Header("Spray Pattern")]
    [Tooltip("Total angular spread of the spray, in degrees. 0 = a single straight ray.")]
    public float coneAngle = 0f;
    [Tooltip("Number of rays fired per spray tick, spread evenly across coneAngle.")]
    public int rayCount = 1;

    [Header("Cost")]
    [Tooltip("Multiplies GameplayConfig.drainRate. Higher = less paint-efficient.")]
    public float drainMultiplier = 1f;
}
