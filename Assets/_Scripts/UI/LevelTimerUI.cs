using UnityEngine;
using TMPro;

public class LevelTimerUI : MonoBehaviour
{
    [SerializeField] private LevelTimer timer;
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        HandleTimeChanged(timer.Remaining);
    }

    private void OnEnable()
    {
        timer.TimeChanged += HandleTimeChanged;
    }

    private void OnDisable()
    {
        timer.TimeChanged -= HandleTimeChanged;
    }

    private void HandleTimeChanged(float remaining)
    {
        int totalSeconds = Mathf.CeilToInt(remaining);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        label.text = minutes + ":" + seconds.ToString("00");
    }
}
