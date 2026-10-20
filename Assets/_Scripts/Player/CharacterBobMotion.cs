using UnityEngine;

// Placeholder "motion" until real walk/idle animation clips exist for this rig (there
// currently are none in the project). Bobs the character model while moving so it
// doesn't look like a frozen mannequin sliding across the floor.
public class CharacterBobMotion : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform modelRoot;
    [SerializeField] private float bobAmplitude = 0.05f;
    [SerializeField] private float bobSpeed = 8f;
    [SerializeField] private float returnSpeed = 6f;

    private float bobTimer;
    private Vector3 baseLocalPosition;

    private void Awake()
    {
        baseLocalPosition = modelRoot.localPosition;
    }

    private void Update()
    {
        if (playerMovement.IsMoving)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float offset = Mathf.Abs(Mathf.Sin(bobTimer)) * bobAmplitude;
            modelRoot.localPosition = baseLocalPosition + Vector3.up * offset;
        }
        else
        {
            bobTimer = 0f;
            modelRoot.localPosition = Vector3.MoveTowards(modelRoot.localPosition, baseLocalPosition, returnSpeed * Time.deltaTime);
        }
    }
}
