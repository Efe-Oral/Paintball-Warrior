using UnityEngine;

[RequireComponent(typeof(PaintableStructure))]
public class StructureVisual : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;
    [SerializeField] private Renderer targetRenderer;

    private PaintableStructure structure;

    private void Awake()
    {
        structure = GetComponent<PaintableStructure>();
    }

    private void OnEnable()
    {
        structure.Completed += HandleCompleted;
    }

    private void OnDisable()
    {
        structure.Completed -= HandleCompleted;
    }

    private void HandleCompleted()
    {
        targetRenderer.material.color = config.structureDoneColor;
    }
}
