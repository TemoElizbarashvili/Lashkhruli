using System;
using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{

    #region Public variables

    public float Speed = 2;
    public float JumpForce = 2;
    public Transform HangingAllowedHeight;
    public Transform[] GroundCheckTransformObjects;
    public Transform DamageDistanceTransform;
    public ParticleSystem JumpParticleSystem;
    public int MaxHealth = 100;
    public int CurrentHealth;
    public HealthBar HealthBar;
    public AudioSource SwordSwingSound;
    public AudioSource SwordHitSound;
    public CheckPointSystem CheckPointSystem;

    #endregion

    #region Private variables

    private const string WallTag = "Wall";
    private const string EarthTag = "Earth";
    private const string EnemyTag = "Enemy";
    private float horizontal = 0.0f;
    private Rigidbody2D rb;
    private Animator animator;
    private ParticleSystem grassParticleSystem;
    private SpriteRenderer spriteRenderer;
    private bool grounded = false;
    private bool isEarthed = false;
    private bool canDoubleJump = false;
    private bool isHanging = false;
    private Coroutine hangingCoroutine;

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -1;
        animator = GetComponent<Animator>();
        grassParticleSystem = GetComponent<ParticleSystem>();
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        transform.localScale = FlipHero();
    }

    #region Movement

    private void Move()
    {
        if (isHanging)
        {
            rb.linearVelocity = new Vector2(0, 0);
            return;
        }

        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Math.Abs(horizontal));

        rb.linearVelocity = new Vector2(horizontal * Speed, rb.linearVelocity.y);
    }

    private Vector3 FlipHero()
        => horizontal switch
        {
            > 0.1f => new Vector3(1, transform.localScale.y, transform.localScale.z),
            < -0.1f => new Vector3(-1, transform.localScale.y, transform.localScale.z),
            _ => transform.localScale
        };

    private void Jump()
    {
        if (isHanging)
        {
            isHanging = false;
            rb.gravityScale = 1f;

            var jumpDirection = new Vector2(-transform.localScale.x * JumpForce, JumpForce);
            rb.linearVelocity = Vector2.zero;
            StopFalling();
            rb.AddForce(jumpDirection, ForceMode2D.Impulse);
            animator.SetBool("IsHanging", false);

            canDoubleJump = true;
        }
        else if (isEarthed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
            JumpParticleSystem.Emit(20);
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);

            canDoubleJump = false;
        }

        animator.SetTrigger("Jump");
    }

    // Wall hang 
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!IsCollisionHappenedWithWall(coll))
            return;

        var earthLayerMask = LayerMask.GetMask("Earth");

        if (Physics2D.Linecast(transform.position, HangingAllowedHeight.position, earthLayerMask).collider != null)
            return;

        EnterWallHang();
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (!IsCollisionHappenedWithWall(coll))
            return;

        MaintainWallHang();
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (!IsCollisionHappenedWithWall(coll))
            return;

        ExitWallHang();
    }

    private void EnterWallHang()
    {
        if (isHanging)
            return;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        isHanging = true;
        animator.SetBool("IsHanging", true);
        hangingCoroutine ??= StartCoroutine(IncreaseGravityOverTime());
    }

    private void MaintainWallHang()
    {
        if (!isHanging)
            return;

        rb.linearVelocity = Vector2.zero;
    }

    private void ExitWallHang()
    {
        if (!isHanging)
            return;

        isHanging = false;
        rb.gravityScale = 1f;
        animator.SetBool("IsHanging", false);

        if (hangingCoroutine == null)
            return;

        StopFalling();
    }

    private bool IsCollisionHappenedWithWall(Collision2D coll)
    {
        var collGameObj = coll.gameObject;
        return collGameObj.CompareTag(WallTag);
    }

    private IEnumerator IncreaseGravityOverTime()
    {
        yield return new WaitForSeconds(1f);

        const float targetGravity = 0.3f;
        const float duration = 3f;
        const float startGravity = 0f;
        var time = 0f;

        while (time < duration && Math.Abs(rb.gravityScale) < 1)
        {
            time += Time.deltaTime;
            rb.gravityScale = Mathf.Lerp(startGravity, targetGravity, time / duration);
            yield return null;
        }

        rb.gravityScale = targetGravity;
        animator.SetTrigger("Fall");
        ExitWallHang();
    }

    private void StopFalling()
    {
        if (hangingCoroutine == null)
            return;

        StopCoroutine(hangingCoroutine);
        hangingCoroutine = null;
    }

    private void GroundCheck()
    {
        var earthLayerMask = LayerMask.GetMask("Earth");

        foreach (var groundCheck in GroundCheckTransformObjects)
        {
            var hit2D = Physics2D.Linecast(transform.position, groundCheck.position, earthLayerMask);

            if (hit2D.collider != null)
            {
                grounded = true;
                animator.SetBool("IsGrounded", true);
                canDoubleJump = true;
                isEarthed = hit2D.collider.gameObject.CompareTag(EarthTag);
                break;
            }
            animator.SetBool("IsGrounded", false);
            isEarthed = false;
            grounded = false;
        }

    }

    #endregion

    #region Combat

    public void TryDamageEnemy()
    {
        var enemyLayerMask = LayerMask.GetMask("Enemy");

        var hit2D = Physics2D.Linecast(transform.position, DamageDistanceTransform.position, enemyLayerMask);
        if (hit2D.collider == null || !hit2D.collider.CompareTag(EnemyTag))
        {
            var healthLayerMask = LayerMask.GetMask("Health");
            var hit = Physics2D.Linecast(transform.position, DamageDistanceTransform.position, healthLayerMask);
            if (hit.collider == null || !hit.collider.CompareTag("Health"))
            {
                Helpers.PlayAudioSafely(SwordSwingSound);
                return;
            }
            if (hit.collider.CompareTag("Health"))
            {
                hit.collider.GetComponent<HealthItem>().Destroy();
                RecoverHealth(35);
            }

            Helpers.PlayAudioSafely(SwordSwingSound);
            return;
        }

        var enemy = hit2D.collider.GetComponent<UngaBungaEnemy>();
        Vector2 hitDirection = (enemy.transform.position - transform.position).normalized;
        if (enemy.Horizontal < 0)
        {
            hitDirection.x *= -1;
        }
        enemy.TakeDamage(20, hitDirection);
        Helpers.PlayAudioSafely(SwordSwingSound);
        Helpers.PlayAudioSafely(SwordHitSound);
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("TakeDamage");
        CurrentHealth -= damage;
        HealthBar.SetHealth(CurrentHealth);
        if (CurrentHealth > 0)
            return;

        CheckPointSystem.RespawnPlayer();
        CurrentHealth = MaxHealth;
        HealthBar.SetHealth(CurrentHealth);
    }

    public void RecoverHealth(int health)
    {
        CurrentHealth += health;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;
        HealthBar.SetHealth(CurrentHealth);
    }

    #endregion

    #region Particles

    // This method is called from the engine while walk anim frames <3
    public void EmitGrassParticles()
    {
        if (grounded && isEarthed)
        {
            grassParticleSystem.Emit(8);
        }
    }

    #endregion

    #region CheckPoints

    void OnTriggerEnter2D(Collider2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "CheckPoint":
                var checkPoint = coll.gameObject;
                CheckPointSystem.SetCheckPoint(checkPoint);
                var sprite = coll.gameObject.GetComponent<SpriteRenderer>();
                if (sprite.enabled)
                    return;

                sprite.enabled = true;
                var audio = coll.gameObject.GetComponent<AudioSource>();
                Helpers.PlayAudioSafely(audio);
                break;
            case "Damage":
                if (CurrentHealth == MaxHealth)
                {
                    TakeDamage(35);
                }
                Destroy(coll.gameObject);
                break;
        }
    }

    #endregion

}
