using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public static bool IsPlayerInSafeZone = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.name + " | Tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            IsPlayerInSafeZone = true;
            Debug.Log(">>> PLAYER SAFE ZONE İÇİNDE");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInSafeZone = false;
            Debug.Log(">>> PLAYER SAFE ZONE DIŞINDA");
        }
    }
}
