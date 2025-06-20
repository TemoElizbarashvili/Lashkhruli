using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    public Transform TeleportDestination;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            TeleportPlayer(coll.transform);
        }
    }

    private void TeleportPlayer(Transform playerTransform)
    {
        if (TeleportDestination != null)
        {
            playerTransform.position = TeleportDestination.position;
        }
    }
}
