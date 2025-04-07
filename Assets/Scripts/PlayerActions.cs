using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerActions : MonoBehaviour
{
    private const string WallTag = "Wall";
    private float horizontal = 0.0f;

    [SerializeField] private float speed = 2;
    [SerializeField] private float jumpForce = 2;
    [SerializeField] private float wallVerticalJumpForce;
    [SerializeField] private float wallHorizontalJumpForce;
    [SerializeField] private Transform hangingAllowedHeight;
    [SerializeField] private Transform[] groundCheckTransformObjects;

    private Rigidbody2D rb;
    private Animator animator;
    private bool grounded = false;
    private bool canDoubleJump = false;
    private bool isHanging = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    private void Move()
    {
        if (isHanging) return;

        horizontal = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Math.Abs(horizontal));
        rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocity.y);
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
            var jumpDirection = new Vector2((-transform.localScale.x) * wallHorizontalJumpForce, wallVerticalJumpForce);
            rb.gravityScale = 1f;

            rb.AddForce(jumpDirection, ForceMode2D.Impulse);
            canDoubleJump = true;
        }
        else if (grounded)
        {
            rb.AddForce(new Vector2(rb.linearVelocityX, jumpForce));
            canDoubleJump = true;

        }
        else if (canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
            rb.AddForce(new Vector2(rb.linearVelocityX, jumpForce));
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
                break;
            }
            animator.SetBool("IsGrounded", false);
            grounded = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!IsCollisionHappenedWithWall(coll))
            return;

        if (Physics2D.Linecast(transform.position, hangingAllowedHeight.position).collider)
            return;

        isHanging = true;
        animator.SetBool("IsHanging", true);
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (!IsCollisionHappenedWithWall(coll))
            return;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        //TODO: make a timer to start increasing gravity and fall character
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (!IsCollisionHappenedWithWall(coll))
            return;

        rb.gravityScale = 1f;
        isHanging = false;
        animator.SetBool("IsHanging", false);
    }

    private bool IsCollisionHappenedWithWall(Collision2D coll)
    {
        var collGameObj = coll.gameObject;
        return string.Equals(collGameObj.tag, WallTag);
    }
}
