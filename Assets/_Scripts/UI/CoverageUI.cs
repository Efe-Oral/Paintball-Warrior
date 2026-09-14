using UnityEngine;
using TMPro;

public class CoverageUI : MonoBehaviour
{
    [SerializeField] private CoverageGrid coverageGrid;
    [SerializeField] private TMP_Text label;

    private void Start()
    {
        HandleCoverageChanged(coverageGrid.CoveragePercent);
    }

    private void OnEnable()
    {
        coverageGrid.CoverageChanged += HandleCoverageChanged;
    }

    private void OnDisable()
    {
        coverageGrid.CoverageChanged -= HandleCoverageChanged;
    }

    private void HandleCoverageChanged(float percent)
    {
        label.text = Mathf.FloorToInt(percent * 100f) + "%";
    }
}
