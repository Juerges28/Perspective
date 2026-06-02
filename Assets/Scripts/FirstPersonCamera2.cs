using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera2 : MonoBehaviour
{
    [SerializeField] private Transform _playerTarget;
    [SerializeField] private float _mouseSensitivity = 250f;
    [SerializeField] private float _eyeHeight = 0.8f;

    private float _xRotation = 0f;
    private float _yRotation = 0f;
    // Start 
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update 
    void Update()
    {
        // Seguir al jugador
        Vector3 headPosition = _playerTarget.position + new Vector3(0f, _eyeHeight, 0f);
        Vector3 forwardOffset = _playerTarget.forward * 0.5f;
        transform.position = headPosition + forwardOffset;

        // Obtener movimiento de mouse
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * _mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * _mouseSensitivity * Time.deltaTime;
        // Calcular rotaciones
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f); //limitador
        _yRotation += mouseX;
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);

        // Ver a donde mirar
        _playerTarget.rotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}
