using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]
public class Samurai : MonoBehaviour
{
    public float walkAcceleration = 4f;
    public float maxSpeed = 4.5f;
    public float chaseSpeed = 6f; 

    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public DetectionZone playerDetectionZone;

    Damageable damageable;
    Animator animator;
    Rigidbody2D rb;
    TouchingDirection touchingDirection;

    public enum WalkableDirection { Right, Left };
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;
    private Transform player;

    private float flipCooldown = 1.5f;
    private float lastFlipTime;
    private float flipWaitTime = 2f; 
    private bool isWaitingToFlip = false; 
    private bool isFlipping = false;  
    private bool isChasing = false;
    private bool isAttacking = false;  
    private bool isInvincible = false;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value && !isAttacking) // ✅ Prevent flipping while attacking
            {
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                walkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            }
            _walkDirection = value;
        }
    }

    private bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);

            // ✅ Stop movement and trigger attack animation when target detected
            if (value)
            {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
            }
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);

    public float AttackCooldown
    {
        get => animator.GetFloat(AnimationStrings.attackCooldown);
        private set => animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirection = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
        isChasing = playerDetectionZone.detectedColliders.Count > 0;
        isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        animator.Update(Time.fixedDeltaTime);
        if (!damageable.LockVelocity)
        {
            // ✅ Stop movement if Samurai has a target
            if (HasTarget)
            {
                rb.linearVelocity = Vector2.zero; // Stop moving while attacking
                return; // Exit movement logic
            }

            if (isChasing && player != null)
            {
                ChasePlayer();
            }
            else if (CanMove && touchingDirection.IsGrounded)
            {
                Patrol();
            }
            else
            {
                rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocityX, 0, walkStopRate), rb.linearVelocityY);
            }
        }
    }

    private void Patrol()
    {
        if (touchingDirection.IsGrounded && touchingDirection.IsOnWall)
        {
            if (!isFlipping && !isAttacking) // ✅ Prevent flipping when attacking
            {
                if (isChasing)
                {
                    if (!isWaitingToFlip)
                    {
                        StartCoroutine(WaitThenFlip());
                    }
                }
                else
                {
                    FlipDirection();
                }
            }
        }

        rb.linearVelocity = new Vector2(walkDirectionVector.x * maxSpeed, rb.linearVelocityY);
    }

    private void ChasePlayer()
    {
        if (isAttacking) return; // ✅ Prevent movement when attacking

        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed * 1.2f, rb.linearVelocityY);

        // Face the player
        WalkDirection = (direction.x > 0) ? WalkableDirection.Right : WalkableDirection.Left;
    }

    private IEnumerator WaitThenFlip()
    {
        if (!gameObject.activeInHierarchy || isAttacking) yield break; // ✅ Prevent flipping when attacking

        isWaitingToFlip = true;
        yield return new WaitForSeconds(flipWaitTime);

        if (isChasing && touchingDirection.IsGrounded && touchingDirection.IsOnWall && gameObject.activeInHierarchy && !isAttacking)
        {
            FlipDirection();
        }

        isWaitingToFlip = false;
    }

    private void FlipDirection()
    {
        if (!HasTarget && Time.time - lastFlipTime > flipCooldown) // ✅ Prevent flipping when attacking
        {
            isFlipping = true;
            WalkDirection = (WalkDirection == WalkableDirection.Right) ? WalkableDirection.Left : WalkableDirection.Right;
            rb.linearVelocity = new Vector2(walkDirectionVector.x * 2, rb.linearVelocityY);
            lastFlipTime = Time.time;
            StartCoroutine(ResetFlippingFlag());
        }
    }

    private IEnumerator ResetFlippingFlag()
    {
        yield return new WaitForSeconds(3f);
        isFlipping = false;
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        if (isInvincible) return; // ✅ Ignore damage if invincible

        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocityY + knockback.y);
        
        StartCoroutine(InvincibilityTimer()); // ✅ Start invincibility

       
    }


    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;  // ✅ Make Samurai invincible
        yield return new WaitForSeconds(3f);  // ✅ Wait for 3 seconds
        isInvincible = false; // ✅ End invincibility
    }
    public void OnCliffDetected()
    {
        if (!isChasing && touchingDirection.IsGrounded)
        {
            FlipDirection();
        }
        else if (isChasing)
        {
            if (!isWaitingToFlip)
            {
                StartCoroutine(WaitThenFlip());
            }
        }
    }
}
