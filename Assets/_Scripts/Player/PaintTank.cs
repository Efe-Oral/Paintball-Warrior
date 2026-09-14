using System;
using UnityEngine;

public class PaintTank : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;

    public event Action<float, float> PaintChanged;
    public event Action Emptied;

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
        if (IsEmpty)
        {
            return;
        }

        float previous = Current;
        Current = Mathf.Max(0f, Current - amount);

        if (Mathf.Approximately(previous, Current))
        {
            return;
        }

        PaintChanged?.Invoke(Current, Capacity);

        if (IsEmpty)
        {
            Emptied?.Invoke();
        }
    }
}
