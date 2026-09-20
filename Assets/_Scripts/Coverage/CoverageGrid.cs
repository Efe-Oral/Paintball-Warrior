using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CoverageGrid : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;

    public event Action<float> CoverageChanged;

    private bool[] painted;
    private int columns;
    private int rows;
    private float cellSize;
    private Vector3 originWorld;
    private int paintedCount;

    public float CoveragePercent { get; private set; }
    public float FloorHeight => originWorld.y;

    private void Awake()
    {
        Bounds bounds = GetComponent<Renderer>().bounds;
        cellSize = config.cellSize;
        columns = Mathf.Max(1, Mathf.CeilToInt(bounds.size.x / cellSize));
        rows = Mathf.Max(1, Mathf.CeilToInt(bounds.size.z / cellSize));
        painted = new bool[columns * rows];
        originWorld = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
    }

    public void MarkCircle(Vector3 worldPos, float radius)
    {
        int minCol = Mathf.Clamp(WorldToColumn(worldPos.x - radius), 0, columns - 1);
        int maxCol = Mathf.Clamp(WorldToColumn(worldPos.x + radius), 0, columns - 1);
        int minRow = Mathf.Clamp(WorldToRow(worldPos.z - radius), 0, rows - 1);
        int maxRow = Mathf.Clamp(WorldToRow(worldPos.z + radius), 0, rows - 1);

        float sqrRadius = radius * radius;
        int newlyPainted = 0;

        for (int row = minRow; row <= maxRow; row++)
        {
            for (int col = minCol; col <= maxCol; col++)
            {
                int index = row * columns + col;
                if (painted[index])
                {
                    continue;
                }

                Vector3 cellCenter = CellCenter(col, row);
                float dx = cellCenter.x - worldPos.x;
                float dz = cellCenter.z - worldPos.z;
                if (dx * dx + dz * dz <= sqrRadius)
                {
                    painted[index] = true;
                    paintedCount++;
                    newlyPainted++;
                }
            }
        }

        if (newlyPainted == 0)
        {
            return;
        }

        CoveragePercent = (float)paintedCount / painted.Length;
        CoverageChanged?.Invoke(CoveragePercent);
    }

    private int WorldToColumn(float worldX)
    {
        return Mathf.FloorToInt((worldX - originWorld.x) / cellSize);
    }

    private int WorldToRow(float worldZ)
    {
        return Mathf.FloorToInt((worldZ - originWorld.z) / cellSize);
    }

    private Vector3 CellCenter(int col, int row)
    {
        float x = originWorld.x + (col + 0.5f) * cellSize;
        float z = originWorld.z + (row + 0.5f) * cellSize;
        return new Vector3(x, originWorld.y, z);
    }
}
