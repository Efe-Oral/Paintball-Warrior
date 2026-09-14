using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameplayConfig config;
    [SerializeField] private InputActionReference moveAction;

    private CharacterController controller;

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
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 motion = new Vector3(input.x, 0f, input.y);

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
