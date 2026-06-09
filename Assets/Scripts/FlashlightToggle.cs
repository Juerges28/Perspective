using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightToggle : MonoBehaviour
{
    [SerializeField] private GameObject flashlightObject;

    private bool isOn = true;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
        {
            isOn = !isOn;
            flashlightObject.SetActive(isOn);
        }
    }
}