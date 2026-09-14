using UnityEngine;
using TMPro;

public class PaintTankUI : MonoBehaviour
{
    [SerializeField] private PaintTank paintTank;
    [SerializeField] private TMP_Text label;

    private void Update()
    {
        int percent = Mathf.CeilToInt(paintTank.NormalizedRemaining * 100f);
        label.text = "Paint: " + percent + "%";
    }
}
