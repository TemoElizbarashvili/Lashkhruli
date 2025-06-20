using Unity.VisualScripting;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Player Player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.gameObject.CompareTag("Player"))
            return;

        Player.TakeDamage(150);
    }
}
