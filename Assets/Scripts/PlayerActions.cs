using System;
using System.Collections;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerActions : MonoBehaviour
{
    private const string WallTag = "Wall";
    private const string EarthTag = "Earth";
    private const string EnemyTag = "Enemy";
    private float horizontal = 0.0f;

    [SerializeField] private float speed = 2;
    [SerializeField] private float jumpForce = 2;
    [SerializeField] private Transform hangingAllowedHeight;
    [SerializeField] private Transform[] groundCheckTransformObjects;
    [SerializeField] private Transform damageDistanceTransform;
    [SerializeField] private ParticleSystem jumpParticleSystem;

    private Rigidbody2D rb;
    private Animator animator;
    private ParticleSystem grassParticleSystem;
    private bool grounded = false;
    private bool isEarthed = false;
    private bool canDoubleJump = false;
    private bool isHanging = false;
    private Coroutine hangingCoroutine;
    private bool isAttacking = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        grassParticleSystem = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Move();
        Attack();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        transform.localScale = FlipHero();
    }

    public void OnAttackStart()
    {
        isAttacking = true;
    }

    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    private void Move()
    {
        if (isHanging)
        {
            rb.linearVelocity = new Vector2(0, 0);
            return;
        }

        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Math.Abs(horizontal));

        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
    }

    // This method is called from the engine while walk anim frames <3
    public void EmitGrassParticles()
    {
        if (grounded && isEarthed)
        {
            grassParticleSystem.Emit(8);
        }
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

            var jumpDirection = new Vector2(-transform.localScale.x * jumpForce, jumpForce);
            rb.linearVelocity = Vector2.zero;
            StopFalling();
            rb.AddForce(jumpDirection, ForceMode2D.Impulse);
            animator.SetBool("IsHanging", false);

            canDoubleJump = true;
        }
        else if (grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpParticleSystem.Emit(20);
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

            canDoubleJump = false;
        }

        animator.SetTrigger("Jump");
    }

    private void GroundCheck()
    {
        foreach (var groundCheck in groundCheckTransformObjects)
        {
            var hit2D = Physics2D.Linecast(transform.position, groundCheck.position);

            if (hit2D.collider)
            {
                grounded = true;
                animator.SetBool("IsGrounded", true);
                canDoubleJump = true;
                isEarthed = hit2D.collider.gameObject.tag.Equals(EarthTag);
                break;
            }
            animator.SetBool("IsGrounded", false);
            isEarthed = false;
            grounded = false;
        }

    }

    private void TryDamageEnemy()
    {
        var hit2D = Physics2D.Linecast(transform.position, damageDistanceTransform.position);
        if (hit2D.collider == null || !hit2D.collider.CompareTag(EnemyTag)) return;
        var enemy = hit2D.collider.GetComponent<UngaBungaEnemy>();
        if (enemy == null) 
            return;
        Vector2 hitDirection = (enemy.transform.position - transform.position).normalized;
        enemy.TakeDamage(10, hitDirection);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!IsCollisionHappenedWithWall(coll))
            return;

        if (Physics2D.Linecast(transform.position, hangingAllowedHeight.position).collider)
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
        return string.Equals(collGameObj.tag, WallTag);
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

    private void Attack()
    {
        if (!Input.GetMouseButtonDown(0)) 
            return;

        if (isAttacking)
            return;

        animator.SetTrigger("IsAttacking");
        TryDamageEnemy();
    }
}
