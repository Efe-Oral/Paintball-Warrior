using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PaintSprayer : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;
    [SerializeField] private CoverageGrid coverageGrid;
    [SerializeField] private PaintTank paintTank;
    [SerializeField] private Transform splatContainer;

    private PlayerMovement playerMovement;
    private float sprayTimer;
    private Mesh splatMesh;
    private Material splatMaterial;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        splatMesh = CreateSplatMesh();

        splatMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        splatMaterial.color = config.paintColor;
    }

    private void Update()
    {
        if (!playerMovement.IsMoving || paintTank.IsEmpty)
        {
            sprayTimer = 0f;
            return;
        }

        paintTank.Drain(config.drainRate * Time.deltaTime);

        sprayTimer -= Time.deltaTime;
        if (sprayTimer > 0f)
        {
            return;
        }

        sprayTimer = config.sprayInterval;
        Spray();
    }

    private void Spray()
    {
        Vector3 groundPos = new Vector3(transform.position.x, coverageGrid.FloorHeight, transform.position.z);
        Vector3 paintPoint = groundPos + playerMovement.MoveDirection.normalized * config.muzzleOffset;

        coverageGrid.MarkCircle(paintPoint, config.paintRadius);
        SpawnSplat(paintPoint);
    }

    private void SpawnSplat(Vector3 groundPos)
    {
        GameObject splat = new GameObject("Splat");
        splat.transform.SetParent(splatContainer, worldPositionStays: false);
        splat.transform.position = groundPos + Vector3.up * 0.01f;
        splat.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

        float coverageMultiplier = Random.Range(config.splatCoverageMultiplierMin, config.splatCoverageMultiplierMax);
        float worldRadius = config.paintRadius * coverageMultiplier;

        // Default primitive Cylinder mesh has radius 0.5 and height 2, so divide the
        // desired world size by those to get the localScale that produces it.
        float xScale = (worldRadius / 0.5f) * Random.Range(0.85f, 1.15f);
        float zScale = (worldRadius / 0.5f) * Random.Range(0.85f, 1.15f);
        float yScale = config.splatThickness / 2f;
        splat.transform.localScale = new Vector3(xScale, yScale, zScale);

        MeshFilter meshFilter = splat.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = splatMesh;

        MeshRenderer meshRenderer = splat.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = splatMaterial;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;
    }

    private static Mesh CreateSplatMesh()
    {
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        Mesh mesh = temp.GetComponent<MeshFilter>().sharedMesh;
        Destroy(temp);
        return mesh;
    }
}
