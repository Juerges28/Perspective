using UnityEngine;

public class Teletransport : MonoBehaviour
{
    private float moveX = -5;
    private float moveZ = -5;
    private float moveY = 7;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            other.transform.position += new Vector3(moveX, moveY, moveZ);
            cc.enabled = true;
        }
    }
}
