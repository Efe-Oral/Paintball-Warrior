using UnityEngine;

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "PaintGame/Gameplay Config")]
public class GameplayConfig : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed = 6f;
}
