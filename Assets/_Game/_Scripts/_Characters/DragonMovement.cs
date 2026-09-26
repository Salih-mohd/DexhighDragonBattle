using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DragonMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -20f;

    private bool isAirborne;
    private Vector3 groundedControllerCenter;

    private CharacterController characterController;
    private float verticalVelocity;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        groundedControllerCenter = characterController.center;
    }

    private void Update()
    {
        HandleGravity();
    }

    public void Move(Vector3 direction)
    {
        direction = Vector3.ClampMagnitude(direction, 1f);

        characterController.Move(
            direction * moveSpeed * Time.deltaTime
        );

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void Move(Vector3 direction, float speed)
    {
        direction = Vector3.ClampMagnitude(direction, 1f);

        characterController.Move(
            direction * speed * Time.deltaTime
        );

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    public void RotateTowards(Vector3 direction)
    {
        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void HandleGravity()
    {

        if (isAirborne)
        {
            verticalVelocity = 0f;
            return;
        }
        if (characterController.isGrounded &&
            verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        characterController.Move(
            Vector3.up *
            verticalVelocity *
            Time.deltaTime
        );
    }

    public void SetAirborne(bool airborne)
    {
        isAirborne = airborne;

        if (airborne)
            verticalVelocity = 0f;
    }

    public void SetColliderVerticalOffset(float offset)
    {
        Vector3 center = groundedControllerCenter;
        center.y += offset;

        characterController.center = center;
    }

    public void ResetColliderCenter()
    {
        characterController.center = groundedControllerCenter;
    }
}