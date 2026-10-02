using System;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    TouchingDirection touchingDirections;
    Damageable damageable;

    public float CurrentMoveSpeed{
        get{
            if(IsMoving && !touchingDirections.IsOnWall &&CanMove){
                if(touchingDirections.IsGrounded){

                    if(IsRunning){
                        return runSpeed;
                    }else{
                        return walkSpeed;
                    }
                }
                else{
                    //Air move
                    return airWalkSpeed;
                }
            }
            else{
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { get {
        return _isMoving;
    }
    private set{
        _isMoving = value;
        animator.SetBool(AnimationStrings.isMoving,value);
        } 
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning{
        get{
            return _isRunning;
        }
        set{
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning,value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight {
        get { return _isFacingRight; }
        private set {
            if (_isFacingRight != value) {
                // Flip the player properly without messing up scale
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (value ? 1 : -1), 
                                                   transform.localScale.y, 
                                                   transform.localScale.z);
            }
            _isFacingRight = value;
        }
    }


    public bool CanMove {
        get {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool isAlive{get{
        return animator.GetBool(AnimationStrings.isAlive);
        }
    }



    

    Rigidbody2D rb;
    Animator animator;


    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirection>();
        damageable = GetComponent<Damageable>();
    }



    private void FixedUpdate() {
        // Explicitly check for ground detection using Physics2D
        bool isGrounded = touchingDirections.IsGrounded;
        

        animator.SetBool(AnimationStrings.isGrounded, isGrounded);

        if (!damageable.LockVelocity) {
            rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocityY);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.linearVelocityY);
    }

  

    public void OnMove(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>();

        if(isAlive){
            IsMoving = moveInput!=Vector2.zero;

            SetFacingDirection(moveInput);
        }else{
            IsMoving = false;
        }
    }
    private void SetFacingDirection(Vector2 moveInput){
        if(moveInput.x>0 && !IsFacingRight){
            // Face Right
            IsFacingRight = true;
        }else if (moveInput.x<0 && IsFacingRight){
            // Face Left
            IsFacingRight = false;
        }
    }
    public void OnRun(InputAction.CallbackContext context){
        if (context.started){
            IsRunning = true;
        }
        else if(context.canceled){
                IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context){
        // TO DO check if alive as well
        if(context.started && touchingDirections.IsGrounded && isAlive){
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context){
        if(context.started){
            animator.SetTrigger(AnimationStrings.attackTrigger); 
        }
    }

    public void OnHit(int damage, Vector2 knockback){
        rb.linearVelocity = new Vector2(knockback.x, rb.linearVelocityY + knockback.y);
    }


    public void OnRangedAttack(InputAction.CallbackContext context){
        if(context.started){
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger); 
        }
    }    


}


