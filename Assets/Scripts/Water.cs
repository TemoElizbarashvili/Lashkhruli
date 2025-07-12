using UnityEngine;

public class Water : MonoBehaviour
{
    private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.gameObject.CompareTag("Player"))
            return;

        player.TakeDamage(100000);
    }
}
