using System;
using UnityEngine;

public class PaintBall : MonoBehaviour
{
    private Vector3 targetPosition;
    private float speed;
    private CoverageGrid coverageGrid;
    private float paintRadius;
    private Action<Vector3> onImpact;

    public void Launch(Vector3 from, Vector3 to, float travelSpeed, CoverageGrid grid, float radius, Action<Vector3> impactCallback)
    {
        transform.position = from;
        targetPosition = to;
        speed = travelSpeed;
        coverageGrid = grid;
        paintRadius = radius;
        onImpact = impactCallback;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= 0.01f)
        {
            Impact();
        }
    }

    private void Impact()
    {
        coverageGrid.MarkCircle(targetPosition, paintRadius);
        onImpact?.Invoke(targetPosition);
        Destroy(gameObject);
    }
}
