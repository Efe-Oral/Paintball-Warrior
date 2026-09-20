using System;
using UnityEngine;

public class PaintableStructure : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;

    public event Action<float> CoverageChanged;
    public event Action Completed;

    private bool[] painted;
    private int paintedCount;

    public float CoveragePercent { get; private set; }
    public bool IsComplete { get; private set; }

    private void Awake()
    {
        painted = new bool[Mathf.Max(4, config.structureSegments)];
    }

    // Marks the segment of this structure facing worldPoint as painted.
    // Callers are expected to pass a point that actually landed on this structure's surface.
    public void Hit(Vector3 worldPoint)
    {
        Vector3 delta = worldPoint - transform.position;
        delta.y = 0f;
        if (delta.sqrMagnitude < 0.0001f)
        {
            delta = Vector3.forward;
        }

        float angle = Mathf.Atan2(delta.z, delta.x);
        if (angle < 0f)
        {
            angle += Mathf.PI * 2f;
        }

        int segment = Mathf.Clamp(Mathf.FloorToInt(angle / (Mathf.PI * 2f) * painted.Length), 0, painted.Length - 1);

        if (painted[segment])
        {
            return;
        }

        painted[segment] = true;
        paintedCount++;
        CoveragePercent = (float)paintedCount / painted.Length;
        CoverageChanged?.Invoke(CoveragePercent);

        if (!IsComplete && paintedCount >= painted.Length)
        {
            IsComplete = true;
            Completed?.Invoke();
        }
    }
}
