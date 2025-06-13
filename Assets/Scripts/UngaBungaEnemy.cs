using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class UngaBungaEnemy : MonoBehaviour
{
    #region Public Properties

    public int Health = 100;
    public float Speed = 1.5f;
    public float horizontal = 1.0f;
    public GameObject deathEffectPrefab;
    public Transform FrontPoint;

    #endregion

    #region Private Properties

    private Rigidbody2D rb;
    private Animator animator;
    private ParticleSystem bloodParticleSystem;

    #endregion
   
    void Start()
    {
        bloodParticleSystem = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(MakeIdleAnim), 5f, 5f);
    }

    void Update()
    {
        Move();
        CheckForWalls();
    }

    #region Animations
   
    private void MakeIdleAnim()
    {
        animator.SetTrigger("Idle");
    }

    #endregion

    #region Movement

    private void Move()
    {
        rb.linearVelocity = new Vector2(horizontal * Speed, rb.linearVelocity.y);
    }

    private void CheckForWalls()
    {
        var wallLayerMask = LayerMask.GetMask("Earth");

        var hit2D = Physics2D.Linecast(transform.position, FrontPoint.position, wallLayerMask);
        if (hit2D.collider == null)
            return;

        if (!hit2D.collider.CompareTag("Earth"))
            return;

        transform.localScale = FlipAsset();
    }

    private Vector3 FlipAsset()
    {
        horizontal *= -1;
        return new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void StopMoving()
    {
        Speed = 0;
    }

    private void StartMoving()
    {
        Speed = 1.5f;
    }

    #endregion

    #region Combat

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        Health -= damage;
        hitDirection.Normalize();
        var emitParams = new ParticleSystem.EmitParams();

        for (var i = 0; i < 10; i++)
        {
            Vector2 randomDir = Quaternion.Euler(Math.Abs(horizontal), 0, Random.Range(-30f, 30f)) * hitDirection;
            emitParams.velocity = randomDir.normalized * Random.Range(3f, 5f);
            emitParams.startSize = Random.Range(0.1f, 0.2f);
            emitParams.startColor = Color.red;

            bloodParticleSystem.Emit(emitParams, 1);
            animator.SetTrigger("Damage");
        }

        if (Health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    #endregion

}