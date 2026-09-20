using System;
using UnityEngine;

public class PaintBall : MonoBehaviour
{
    private Vector3 flightTarget;
    private Vector3 impactPoint;
    private Vector3 impactNormal;
    private float speed;
    private CoverageGrid coverageGrid;
    private float paintRadius;
    private PaintableStructure structure;
    private Action<Vector3, Vector3> onImpact;

    public void Launch(
        Vector3 from,
        Vector3 target,
        Vector3 impact,
        Vector3 normal,
        float travelSpeed,
        CoverageGrid grid,
        float radius,
        PaintableStructure hitStructure,
        Action<Vector3, Vector3> impactCallback)
    {
        transform.position = from;
        flightTarget = target;
        impactPoint = impact;
        impactNormal = normal;
        speed = travelSpeed;
        coverageGrid = grid;
        paintRadius = radius;
        structure = hitStructure;
        onImpact = impactCallback;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, flightTarget, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, flightTarget) <= 0.01f)
        {
            Impact();
        }
    }

    private void Impact()
    {
        if (structure != null)
        {
            structure.Hit(impactPoint);
        }
        else
        {
            coverageGrid.MarkCircle(impactPoint, paintRadius);
        }

        onImpact?.Invoke(impactPoint, impactNormal);
        Destroy(gameObject);
    }
}
