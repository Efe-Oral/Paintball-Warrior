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
    private Mesh ballMesh;
    private Material paintMaterial;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        splatMesh = CreatePrimitiveMesh(PrimitiveType.Cylinder);
        ballMesh = CreatePrimitiveMesh(PrimitiveType.Sphere);

        paintMaterial = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        paintMaterial.color = config.paintColor;
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
        Vector3 direction = playerMovement.MoveDirection.normalized;
        Vector3 muzzlePos = groundPos + direction * config.gunForwardOffset + Vector3.up * config.gunHeight;

        // Default: nothing in the way, paint lands on the floor at max range.
        Vector3 flightTarget = muzzlePos + direction * config.muzzleOffset;
        Vector3 impactPoint = groundPos + direction * (config.gunForwardOffset + config.muzzleOffset);
        Vector3 impactNormal = Vector3.up;
        PaintableStructure hitStructure = null;

        if (Physics.Raycast(muzzlePos, direction, out RaycastHit hit, config.muzzleOffset))
        {
            PaintableStructure structure = hit.collider.GetComponentInParent<PaintableStructure>();
            if (structure != null)
            {
                hitStructure = structure;
                flightTarget = hit.point;
                impactPoint = hit.point;
                impactNormal = hit.normal;
            }
        }

        SpawnPaintBall(muzzlePos, flightTarget, impactPoint, impactNormal, hitStructure);
    }

    private void SpawnPaintBall(Vector3 from, Vector3 flightTarget, Vector3 impactPoint, Vector3 impactNormal, PaintableStructure structure)
    {
        GameObject ball = new GameObject("PaintBall");
        ball.transform.position = from;
        ball.transform.localScale = Vector3.one * config.ballRadius * 2f;

        MeshFilter meshFilter = ball.AddComponent<MeshFilter>();
        meshFilter.sharedMesh = ballMesh;

        MeshRenderer meshRenderer = ball.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = paintMaterial;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;

        PaintBall paintBall = ball.AddComponent<PaintBall>();
        paintBall.Launch(from, flightTarget, impactPoint, impactNormal, config.ballSpeed, coverageGrid, config.paintRadius, structure, SpawnSplat);
    }

    private void SpawnSplat(Vector3 position, Vector3 normal)
    {
        GameObject splat = new GameObject("Splat");
        splat.transform.SetParent(splatContainer, worldPositionStays: false);
        splat.transform.position = position + normal * 0.015f;

        float randomSpin = Random.Range(0f, 360f);
        splat.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal) * Quaternion.Euler(0f, randomSpin, 0f);

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
        meshRenderer.sharedMaterial = paintMaterial;
        meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        meshRenderer.receiveShadows = false;
    }

    private static Mesh CreatePrimitiveMesh(PrimitiveType type)
    {
        GameObject temp = GameObject.CreatePrimitive(type);
        Mesh mesh = temp.GetComponent<MeshFilter>().sharedMesh;
        Destroy(temp);
        return mesh;
    }
}
