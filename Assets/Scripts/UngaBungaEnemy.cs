using System.Collections;
using UnityEngine;

public class UngaBungaEnemy : MonoBehaviour
{
    [SerializeField] public int Health = 100;
    private ParticleSystem bloodParticleSystem;
    [SerializeField] private GameObject deathEffectPrefab; // Optional death effect prefab

    void Start()
    {
        bloodParticleSystem = GetComponent<ParticleSystem>();
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        Health -= damage;
        StartCoroutine(EmitBloodDelayed(hitDirection)); // Delay properly using coroutine

        if (Health <= 0)
        {
            Die();
        }
    }

    private IEnumerator EmitBloodDelayed(Vector2 direction)
    {
        yield return new WaitForSeconds(0.35f);

        direction.Normalize();
        var emitParams = new ParticleSystem.EmitParams();

        for (var i = 0; i < 10; i++)
        {
            Vector2 randomDir = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * direction;
            emitParams.velocity = randomDir.normalized * Random.Range(3f, 5f);
            emitParams.startSize = Random.Range(0.1f, 0.2f);
            emitParams.startColor = Color.red;

            bloodParticleSystem.Emit(emitParams, 1);
        }
    }

    public void Die()
    {
        // Optional: spawn death particles
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy enemy after playing effects
    }
}