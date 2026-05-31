using UnityEngine;

public class GrabController : MonoBehaviour
{
    public Camera playerCamera;
    public float grabDistance;
    public LayerMask interactableLayer;
    private InputSystem_Actions controls;

    void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ThrowRaycast()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Physics.Raycast(ray, out RaycastHit hitInfo, grabDistance, interactableLayer);
    }
}
