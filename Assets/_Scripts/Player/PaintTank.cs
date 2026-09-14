using UnityEngine;

public class PaintTank : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;

    public float Current { get; private set; }
    public float Capacity => config.paintTankCapacity;
    public float NormalizedRemaining => Capacity > 0f ? Current / Capacity : 0f;
    public bool IsEmpty => Current <= 0f;

    private void Awake()
    {
        Current = config.paintTankCapacity;
    }

    public void Drain(float amount)
    {
        Current = Mathf.Max(0f, Current - amount);
    }
}
