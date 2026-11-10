using System;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;

    public event Action<float> TimeChanged;
    public event Action TimedOut;

    public float Remaining { get; private set; }

    private bool stopped;
    private int lastDisplayedSecond;

    private void Awake()
    {
        Remaining = config.levelTimeLimit;
        lastDisplayedSecond = Mathf.CeilToInt(Remaining);
    }

    private void Update()
    {
        if (stopped || Remaining <= 0f)
        {
            return;
        }

        Remaining = Mathf.Max(0f, Remaining - Time.deltaTime);

        int displaySecond = Mathf.CeilToInt(Remaining);
        if (displaySecond != lastDisplayedSecond)
        {
            lastDisplayedSecond = displaySecond;
            TimeChanged?.Invoke(Remaining);
        }

        if (Remaining <= 0f)
        {
            TimedOut?.Invoke();
        }
    }

    public void Stop()
    {
        stopped = true;
    }
}
