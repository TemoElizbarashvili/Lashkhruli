using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerActions : MonoBehaviour
{
    private float horizontal = 0.0f;

    [SerializeField] private float speed = 2;
    [SerializeField] private float jumpForce = 2;
    [SerializeField] private Transform groundCheckTransformComponent;

    private Rigidbody2D _rigidbody2D;
    private bool grounded = false;
    private bool canDoubleJump = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
        horizontal = Input.GetAxis("Horizontal");
        _rigidbody2D.linearVelocity = new Vector2(horizontal * speed, _rigidbody2D.linearVelocity.y);
    }

    private Vector3 FlipHero()
         => horizontal switch
        {
            > 0.1f =>  new Vector3(1, transform.localScale.y, transform.localScale.z),
            < -0.1f =>  new Vector3(-1, transform.localScale.y, transform.localScale.z),
            _ =>  transform.localScale
        };

    private void Jump()
    {
        if (grounded)
        {
            _rigidbody2D.AddForce(new Vector2(_rigidbody2D.linearVelocityX, jumpForce));
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocityX, 0);
            _rigidbody2D.AddForce(new Vector2(_rigidbody2D.linearVelocityX, jumpForce));
            canDoubleJump = false;
        }
    }

    private void GroundCheck()
    {
        var hit2D = Physics2D.Linecast(transform.position, groundCheckTransformComponent.position);
        if (hit2D.collider)
        {
            grounded = true;
            canDoubleJump = true;
        }
        else
        {
            grounded = false;
        }

        
    }
}
