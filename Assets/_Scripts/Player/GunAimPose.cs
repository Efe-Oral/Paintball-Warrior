using UnityEngine;

// No animation clips exist for this rig, so the arm has no "aim forward" pose of its
// own. This procedurally rotates the hand bone each frame so the gun's own muzzle axis
// (its local forward, matching the barrel direction on the imported mesh) points where
// the player is facing, instead of sitting frozen in the rig's bind pose.
public class GunAimPose : MonoBehaviour
{
    [SerializeField] private Transform aimOrigin;
    [SerializeField] private Transform aimBone;
    [SerializeField] private Transform muzzleReference;
    [SerializeField] private float aimDistance = 5f;
    [SerializeField] private Vector3 aimOffset = new Vector3(0f, 1.3f, 0f);

    private Vector3 localBoneDirection;

    private void Start()
    {
        localBoneDirection = aimBone.InverseTransformDirection(muzzleReference.forward);
    }

    private void LateUpdate()
    {
        Vector3 targetPoint = aimOrigin.position + aimOffset + aimOrigin.forward * aimDistance;
        Vector3 desiredWorldDirection = (targetPoint - muzzleReference.position).normalized;
        Vector3 currentWorldDirection = aimBone.TransformDirection(localBoneDirection);

        Quaternion correction = Quaternion.FromToRotation(currentWorldDirection, desiredWorldDirection);
        aimBone.rotation = correction * aimBone.rotation;
    }
}
