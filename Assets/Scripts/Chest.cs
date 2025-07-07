using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    public ParticleSystem ChestParticleSystem;
    public AudioSource IdleAudioSource;
    public AudioSource OpenAudioSource;

    private bool isPlayerNear = false;
    private bool isOpened = false;
    private Animator animator;

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
        Destroy(IdleAudioSource);
        ChestParticleSystem.Stop();
        isOpened = true;
        var foundedMoney = Random.Range(30, 70);
        EconomyManager.AddCoins(foundedMoney);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    public void PlayOpenAudio()
    {
        OpenAudioSource.Play();
    }
}
