using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isPlayerNear = false;
    private bool isOpened = false;
    private Animator animator;

    [SerializeField] private int rewardPoint = 10;
    [SerializeField] private ParticleSystem chestParticleSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.F) && !isOpened)
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        animator.SetTrigger("Open");
        isOpened = true;
        chestParticleSystem.Stop();
        //TODO: add points to player <3
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            isPlayerNear = false;
        }
    }
}
