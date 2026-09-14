using UnityEngine;

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "PaintGame/Gameplay Config")]
public class GameplayConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Coverage Grid")]
    [Tooltip("World-space size of one invisible coverage cell. Controls percentage granularity, not visual quality.")]
    public float cellSize = 0.25f;

    [Header("Spray")]
    [Tooltip("How far ahead of the player the paint lands.")]
    public float muzzleOffset = 0.6f;
    [Tooltip("Radius of floor marked painted per spray tick.")]
    public float paintRadius = 0.7f;
    [Tooltip("Seconds between spray ticks while moving.")]
    public float sprayInterval = 0.08f;

    [Header("Level")]
    [Range(0f, 1f)] public float coverageThreshold = 0.85f;
    [Range(0f, 1f)] public float star2RemainingThreshold = 0.5f;
    [Range(0f, 1f)] public float star3RemainingThreshold = 0.75f;

    [Header("Paint Tank")]
    public float paintTankCapacity = 100f;
    [Tooltip("Units of paint drained per second while actively spraying.")]
    public float drainRate = 10f;

    [Header("Paint Ball")]
    [Tooltip("How fast the thrown paint ball travels toward its target.")]
    public float ballSpeed = 14f;
    [Tooltip("Visual radius of the flying paint ball.")]
    public float ballRadius = 0.15f;
    [Tooltip("Height above the floor the ball travels at before impact.")]
    public float gunHeight = 0.6f;

    [Header("Splat Visuals")]
    public Color paintColor = new Color(0.1f, 0.45f, 0.95f, 1f);
    [Tooltip("Splat visual radius is paintRadius times a value in this range, so it always generously covers the cells it marks.")]
    public float splatCoverageMultiplierMin = 1.1f;
    public float splatCoverageMultiplierMax = 1.4f;
    public float splatThickness = 0.02f;
}
