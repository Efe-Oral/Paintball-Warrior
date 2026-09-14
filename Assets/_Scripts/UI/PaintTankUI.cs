using UnityEngine;
using TMPro;

public class PaintTankUI : MonoBehaviour
{
    [SerializeField] private PaintTank paintTank;
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        HandlePaintChanged(paintTank.Current, paintTank.Capacity);
    }

    private void OnEnable()
    {
        paintTank.PaintChanged += HandlePaintChanged;
    }

    private void OnDisable()
    {
        paintTank.PaintChanged -= HandlePaintChanged;
    }

    private void HandlePaintChanged(float current, float capacity)
    {
        int percent = Mathf.CeilToInt(capacity > 0f ? current / capacity * 100f : 0f);
        label.text = "Paint: " + percent + "%";
    }
}
