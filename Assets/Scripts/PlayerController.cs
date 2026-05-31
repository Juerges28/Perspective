using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 0.1f;

    [Header("Grab")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float grabDistance;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform grabbedObject;


    private CharacterController controller;
    private InputSystem_Actions controls;

    private float verticalVelocity;
    private float yaw;

    public Vector2 LookInput { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.Enable();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        LookInput = controls.Player.Look.ReadValue<Vector2>();

        RotatePlayer();
        Move();
        Grab();
        UpdateGrabbedObject();
    }

    private void RotatePlayer()
    {
        yaw += LookInput.x * mouseSensitivity;

        transform.rotation =
            Quaternion.Euler(0f, yaw, 0f);
    }

    private void Move()
    {
        Vector2 moveInput =
            controls.Player.Move.ReadValue<Vector2>();

        Vector3 moveDirection =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (controls.Player.Jump.triggered)
            {
                verticalVelocity =
                    Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity =
            moveDirection * moveSpeed +
            Vector3.up * verticalVelocity;

        controller.Move(
            velocity * Time.deltaTime
        );
    }

    private bool ThrowRaycast(out RaycastHit hitInfo)
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        return Physics.Raycast(
            ray,
            out hitInfo,
            grabDistance,
            interactableLayer
        );
    }

    private void Grab()
    {
        if (ThrowRaycast(out RaycastHit hit))
        {
            grabbedObject = hit.transform;
        }
    }

    private void UpdateGrabbedObject()
    {
        if (grabbedObject == null)
            return;
        float currentDistance = 3f;
        grabbedObject.position = playerCamera.transform.position + playerCamera.transform.forward * currentDistance;

    }

    void OnDrawGizmos()
    {
        if (playerCamera == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * grabDistance
        );
    }
}