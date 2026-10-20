using UnityEngine;

public class BackpackTankVisual : MonoBehaviour
{
    [SerializeField] private PaintTank paintTank;
    [SerializeField] private Transform liquid;

    [Tooltip("Local Y of the tank's interior floor - the liquid's bottom edge stays pinned here as it drains.")]
    [SerializeField] private float bottomLocalY = -0.11f;
    [Tooltip("Half-height of the liquid cylinder when the tank is completely full.")]
    [SerializeField] private float maxHalfHeight = 0.11f;
    [SerializeField] private float minHalfHeight = 0.005f;

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
        float normalized = capacity > 0f ? current / capacity : 0f;
        float halfHeight = Mathf.Lerp(minHalfHeight, maxHalfHeight, normalized);

        Vector3 scale = liquid.localScale;
        scale.y = halfHeight;
        liquid.localScale = scale;

        Vector3 position = liquid.localPosition;
        position.y = bottomLocalY + halfHeight;
        liquid.localPosition = position;
    }
}
