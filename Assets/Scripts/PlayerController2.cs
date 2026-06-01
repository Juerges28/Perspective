using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController2 : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 2f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private CharacterController _controller;
    private float _verticalVelocity;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed) horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed) horizontal += 1f;

        if (Keyboard.current.sKey.isPressed) vertical -= 1f;
        if (Keyboard.current.wKey.isPressed) vertical += 1f;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection =
            forward * vertical +
            right * horizontal;

        moveDirection.Normalize();

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.rotation =
                Quaternion.LookRotation(moveDirection);
        }

        if (_controller.isGrounded)
        {
            if (_verticalVelocity < 0f)
                _verticalVelocity = -2f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                _verticalVelocity =
                    Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        _verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity =
            moveDirection * moveSpeed +
            Vector3.up * _verticalVelocity;

        _controller.Move(velocity * Time.deltaTime);
    }
}