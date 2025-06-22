using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public Animator Animator;
    public ParticleSystem IdleParticleSystem;
    public AudioSource HitSound;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void Destroy()
    {
        if (IdleParticleSystem.isPlaying)
        {
            IdleParticleSystem.Stop();
        }
        Animator.SetTrigger("Destroy");
    }

    public void Destroyed()
    {
        Destroy(gameObject);
    }

    public void PlayHitSound()
        => Helpers.PlayAudioSafely(HitSound);
}
