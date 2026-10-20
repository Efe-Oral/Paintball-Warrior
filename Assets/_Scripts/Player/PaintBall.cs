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
    private bool blocked;
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
        bool hitBlocked,
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
        blocked = hitBlocked;
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
        else if (!blocked)
        {
            // Nothing in the way — paint reached the open floor.
            coverageGrid.MarkCircle(impactPoint, paintRadius);
        }
        // else: hit a plain (non-paintable) obstacle. It blocked the paint — leave a
        // splat for feedback via onImpact below, but no coverage is credited anywhere.

        onImpact?.Invoke(impactPoint, impactNormal);
        Destroy(gameObject);
    }
}
