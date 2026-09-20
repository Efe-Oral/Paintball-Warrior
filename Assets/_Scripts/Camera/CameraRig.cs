using UnityEngine;

[ExecuteAlways]
public class CameraRig : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;

    private void OnEnable()
    {
        Apply();
    }

    [ContextMenu("Apply View Mode")]
    private void Apply()
    {
        if (config == null)
        {
            return;
        }

        float yaw = config.viewMode == CameraViewMode.Isometric ? config.isometricYaw : 0f;
        Quaternion rotation = Quaternion.Euler(config.cameraPitch, yaw, 0f);

        transform.rotation = rotation;
        transform.position = config.cameraFocusPoint - rotation * Vector3.forward * config.cameraDistance;
    }
}
