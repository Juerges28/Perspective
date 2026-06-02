using UnityEngine;
public class PandaNPC : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 1f;

    private void Update()
    {
        if (_camera == null || _player == null)
            return;

        // Seguir al jugador solo en X y Z
        Vector3 targetPosition = new Vector3(
            _player.position.x,
            transform.position.y,
            _player.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            _speed * Time.deltaTime
        );

        // Mirar hacia el frente de la cámara
        Vector3 lookDirection = _camera.transform.forward;
        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }
}