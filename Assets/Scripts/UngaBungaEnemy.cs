using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UngaBungaEnemy : MonoBehaviour
{
    #region Public Properties

    public int Health = 100;
    public float Speed = 1.5f;
    public float Horizontal = 1.0f;
    public GameObject DeathEffectPrefab;
    public Transform FrontPoint;
    public AudioSource DeathSound;
    public Transform[] EnemyRecognitionBorders;
    public AudioSource AxeSwing;
    public AudioSource AxeHit;
    public AudioSource Shout;

    #endregion

    #region Private Properties

    private Rigidbody2D rb;
    private Animator animator;
    private ParticleSystem bloodParticleSystem;
    private float currentSpeed;
    private Transform player;
    private bool canAttack = true;


    #endregion

    void Start()
    {
        bloodParticleSystem = GetComponent<ParticleSystem>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(MakeIdleAnim), 5f, 7.5f);
        currentSpeed = Speed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        Move();
        CheckForWalls();
        TryToAttack();
    }

    #region Animations

    private void MakeIdleAnim()
    {
        if (!IsPlayerNearby())
        {
            animator.SetTrigger("Idle");
        }
    }

    public void MakeShout()
        => Helpers.PlayAudioSafely(Shout);

    #endregion

    #region Movement

    private void Move()
    {
        rb.linearVelocity = new Vector2(Horizontal * currentSpeed, rb.linearVelocity.y);
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

    private Vector3 FlipAsset(FlipDirection? direction = null)
    {
        var scale = transform.localScale;
        switch (direction)
        {
            case FlipDirection.Left:
                if (Horizontal > 0)
                    Horizontal = -1;
                scale.x = -Mathf.Abs(scale.x);
                break;
            case FlipDirection.Right:
                if (Horizontal < 0)
                    Horizontal = 1;
                scale.x = Mathf.Abs(scale.x);
                break;
            case null:
                Horizontal *= -1;
                scale.x *= -1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
        return scale;
    }

    private void StopMoving()
    {
        currentSpeed = 0;
    }

    private void StartMoving()
    {
        currentSpeed = Speed;
        canAttack = true;
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
            Vector2 randomDir = Quaternion.Euler(Math.Abs(Horizontal), 0, Random.Range(-30f, 30f)) * hitDirection;
            emitParams.velocity = randomDir.normalized * Random.Range(3f, 5f);
            emitParams.startSize = Random.Range(0.1f, 0.2f);
            emitParams.startColor = Color.red;

            bloodParticleSystem.Emit(emitParams, 1);
            animator.SetTrigger("Damage");
        }

        if (Health > 0)
        {
            return;
        }
        Die();
    }

    public void Die()
    {
        if (DeathEffectPrefab != null)
        {
            Instantiate(DeathEffectPrefab, transform.position, Quaternion.identity);
        }
        if (DeathSound != null)
        {
            var audioObj = Instantiate(DeathSound, transform.position, Quaternion.identity);
            if (audioObj != null)
            {
                Helpers.PlayAudioSafely(audioObj);
            }
        }
        Destroy(gameObject);
    }

    public bool IsPlayerNearby()
    {
        var playerLayer = LayerMask.GetMask("Player");
        var blockingLayers = LayerMask.GetMask("Earth");
        return (from border in EnemyRecognitionBorders
                select Physics2D.LinecastAll(transform.position, border.position, playerLayer | blockingLayers)
            into hit
                where hit.Any(h => h.collider?.gameObject.tag == "Player")
                select hit.Length <= 1 || hit.FirstOrDefault().collider.gameObject.tag == "Player").FirstOrDefault();
    }

    public bool IsPlayerInAttackRange()
    {
        var hit2D = Physics2D.Linecast(transform.position, FrontPoint.position, LayerMask.GetMask("Player"));
        return hit2D.collider?.gameObject.tag == "Player";
    }

    private void ChasePlayer()
    {
        var targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * (currentSpeed + 0.3f));
        transform.localScale = FlipAsset(player.position.x < transform.position.x ? FlipDirection.Left : FlipDirection.Right);
    }

    private void SetCanAttackToFalse()
    {
        canAttack = false;
        currentSpeed = 0;
    }

    private void TryToAttack()
    {
        if (!IsPlayerNearby())
            return;

        if (IsPlayerInAttackRange())
        {
            if (!canAttack)
            {
                return;
            }
            animator.SetTrigger("Attack");
        }
        else
        {
            ChasePlayer();
        }
    }

    public void TryDamageEnemy()
    {
        var playerLayerMask = LayerMask.GetMask("Player");

        var hit2D = Physics2D.Linecast(transform.position, FrontPoint.position, playerLayerMask);
        if (hit2D.collider == null || !hit2D.collider.CompareTag("Player"))
        {
            Helpers.PlayAudioSafely(AxeSwing);
            return;
        }

        var playerInstance = hit2D.collider.GetComponent<Player>();
        playerInstance.TakeDamage(35);
        Helpers.PlayAudioSafely(AxeSwing);
        Helpers.PlayAudioSafely(AxeHit);
    }

    #endregion
}

public enum FlipDirection
{
    Left = -1,
    Right = 1
}