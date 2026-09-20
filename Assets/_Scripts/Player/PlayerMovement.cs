using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;
    [SerializeField] private InputActionReference moveAction;

    private CharacterController controller;
    private Transform cameraTransform;

    public bool IsMoving { get; private set; }
    public Vector3 MoveDirection { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        Vector2 input = moveAction.action.ReadValue<Vector2>();
        float cameraYaw = cameraTransform != null ? cameraTransform.eulerAngles.y : 0f;
        Vector3 motion = Quaternion.Euler(0f, cameraYaw, 0f) * new Vector3(input.x, 0f, input.y);

        if (motion.sqrMagnitude > 1f)
        {
            motion.Normalize();
        }

        IsMoving = motion.sqrMagnitude > 0.0001f;

        if (IsMoving)
        {
            transform.forward = motion;
            MoveDirection = motion;
        }

        controller.Move(motion * config.moveSpeed * Time.deltaTime);
    }
}
