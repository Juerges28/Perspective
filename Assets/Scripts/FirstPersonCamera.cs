using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private float sensitivity = 0.1f;

    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;

    private float pitch;

    private void Update()
    {
        Vector2 lookInput = player.LookInput;

        pitch -= lookInput.y * sensitivity;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
        );

        transform.localRotation =
            Quaternion.Euler(
                pitch,
                0f,
                0f
            );
    }
}