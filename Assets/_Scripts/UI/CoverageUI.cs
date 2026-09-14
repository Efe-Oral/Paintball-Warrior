using UnityEngine;
using TMPro;

public class CoverageUI : MonoBehaviour
{
    [SerializeField] private CoverageGrid coverageGrid;
    [SerializeField] private TMP_Text label;

    private void Update()
    {
        int percent = Mathf.FloorToInt(coverageGrid.CoveragePercent * 100f);
        label.text = percent + "%";
    }
}
