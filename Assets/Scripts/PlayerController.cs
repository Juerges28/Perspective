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
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float scrollSpeed = 5f;

    public float currentDistance = 3f;
    public float originalDistance;
    public Vector3 originalScale;

    private Rigidbody grabbedRb;


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
        if (!controls.Player.Grab.triggered)
            return;

        if (grabbedObject != null)
        {
            if (grabbedRb != null)
            {
                grabbedRb.isKinematic = false;
                grabbedRb.useGravity = true;
            }

            grabbedObject = null;
            grabbedRb = null;

            return;
        }

        if (ThrowRaycast(out RaycastHit hit))
        {
            grabbedObject = hit.transform;

            currentDistance = Vector3.Distance(playerCamera.transform.position,hit.transform.position);
            originalDistance = currentDistance;
            originalScale = grabbedObject.localScale;

            grabbedRb = grabbedObject.GetComponent<Rigidbody>();

            if (grabbedRb != null)
            {
                grabbedRb.linearVelocity = Vector3.zero;
                grabbedRb.angularVelocity = Vector3.zero;

                grabbedRb.useGravity = false;
                grabbedRb.isKinematic = true;
            }
        }
    }

    private void UpdateGrabbedObject()
    {
        if (grabbedObject == null)
            return;

        Vector2 scroll =
            controls.Player.Scroll.ReadValue<Vector2>();

        currentDistance +=
            scroll.y * scrollSpeed * 0.01f;

        currentDistance = Mathf.Clamp(
            currentDistance,
            minDistance,
            maxDistance
        );

        float scaleFactor =
            currentDistance / originalDistance;

        grabbedObject.localScale =
            originalScale * scaleFactor;

        Vector3 targetPosition =
            playerCamera.transform.position +
            playerCamera.transform.forward *
            currentDistance;

        grabbedObject.position = targetPosition;
    }

    private void OnDrawGizmos()
    {
        if (playerCamera == null)
            return;

        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * grabDistance
        );

        Gizmos.color = Color.green;

        Vector3 targetPosition =
            playerCamera.transform.position +
            playerCamera.transform.forward *
            currentDistance;

        Gizmos.DrawWireSphere(
            targetPosition,
            0.2f
        );
    }
}