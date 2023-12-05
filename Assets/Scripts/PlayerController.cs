using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine;
using Spine.Unity;
using UnityEngine;
using AnimationState = Spine.AnimationState;
using Debug = UnityEngine.Debug;

public enum Direction
{
    Left,
    Right
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    
    private SkeletonAnimation skeletonAnimation;
    private Rigidbody2D rb;
    private float horizontalAxisValue;
    private bool inputLeft, inputRight, inputRun, inputJump, inputFire;

    private const string idleAnimation = "idle";
    private const string walkAnimation = "walk";
    private const string runAnimation = "run";
    private const string jumpAnimation = "jump";
    private const string fireAnimation = "shoot";

    private const int movementTrack = 0;
    private const int actionTrack = 1;

    public event Action OnIdleState;
    public event Action OnWalkState;
    public event Action OnRunState;
    
    public float raycastDistance = 0.1f;
    public LayerMask groundLayer;
    public bool isGrounded;
    public bool isJumping;
    public Animator colliderAnimator;
    private int jumpCount = 0;
    private int maxJumpCount = 2;
    [SerializeField] private KeyboardInput keyboardInput;


    private void Awake()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        rb = GetComponentInChildren<Rigidbody2D>();
        isGrounded = true;
    }

    private void Start()
    {
        skeletonAnimation.AnimationState.Complete += OnAnimationComplete;
        skeletonAnimation.AnimationState.SetAnimation(movementTrack, idleAnimation, true);
    }

    private void Update()
    {
        CheckGround();
        //UpdateInputValues();
        UpdateHorizontalMovement();
        UpdateFacing();
        UpdateAnimationState();
    }
    
    private void UpdateInputValues()
    {
        horizontalAxisValue = Input.GetAxis("Horizontal");
        inputLeft = Input.GetKey("left");
        inputRight = Input.GetKey("right");
        inputRun = Input.GetKey("left shift");
        inputJump = Input.GetKeyDown("space");
        inputFire = Input.GetKeyDown("left ctrl");
    }

    private void UpdateHorizontalMovement()
    {
        if (skeletonAnimation.AnimationName == walkAnimation)
        {
            rb.velocity = new Vector2(horizontalAxisValue * walkSpeed, rb.velocity.y);
            transform.position = new Vector2(rb.position.x, rb.position.y);
        }
        else if (skeletonAnimation.AnimationName == runAnimation)
        {
            rb.velocity = new Vector2(horizontalAxisValue * runSpeed, rb.velocity.y);
            transform.position = new Vector2(rb.position.x, rb.position.y);
        }
        else if (skeletonAnimation.AnimationName == jumpAnimation)
        {
            rb.velocity = new Vector2(horizontalAxisValue * walkSpeed, 0);
            transform.position = new Vector2(rb.position.x, 0);
        }
        else if (skeletonAnimation.AnimationName == idleAnimation)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void UpdateFacing()
    {
        if (inputLeft)
        {
            FlipCharacter(Direction.Left);
        }
        else if (inputRight)
        {
            FlipCharacter(Direction.Right);
        }
    }

    public void FlipCharacter(Direction direction)
    {
        if (direction == Direction.Left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (direction == Direction.Right)
        {
            transform.localScale = Vector3.one;
        }
    }

    private void UpdateAnimationState()
    {
        if (isGrounded && !isJumping && (keyboardInput.inputLeft || keyboardInput.inputRight) && inputRun)
        {
            SetRunAnimation();
            OnRunState?.Invoke();
        }
        else if (isGrounded && !isJumping && (keyboardInput.inputLeft || keyboardInput.inputRight))
        {
            SetWalkAnimation();
            OnWalkState?.Invoke();
        }
        else if (isGrounded && !isJumping && false)
        {
            SetIdleAnimation();
            OnIdleState?.Invoke();
        }
        
        if (inputJump && jumpCount < maxJumpCount)
        {
            SetJumpAnimation();
            jumpCount++;
            isJumping = true;
            colliderAnimator.SetBool("isPlayerJumping", true);
        }
        
        if (inputFire)
        {
            SetFireAnimation();
        }
    }
    
    private void CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.transform.position, Vector2.down, raycastDistance, groundLayer);
        Debug.DrawRay(rb.transform.position, Vector2.down * raycastDistance, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("Player is grounded");
            isGrounded = true;
        }
        else
        {
            Debug.Log("Player is not grounded");
            isGrounded = false;
        }
    }

    public void SetRunAnimation()
    {
        if (skeletonAnimation.AnimationName != runAnimation && !isJumping)
        {
            skeletonAnimation.AnimationState.SetAnimation(movementTrack, runAnimation, true);
        }
    }
    
    public void SetWalkAnimation()
    {
        if (skeletonAnimation.AnimationName != walkAnimation && !isJumping)
        {
            skeletonAnimation.AnimationState.SetAnimation(movementTrack, walkAnimation, true);
        }
    }
    
    public void SetIdleAnimation()
    {
        if (skeletonAnimation.AnimationName != idleAnimation && !isJumping)
        {
            skeletonAnimation.AnimationState.SetAnimation(movementTrack, idleAnimation, true);
        }
    }
    
    public void SetJumpAnimation()
    {
        if (skeletonAnimation.AnimationName != jumpAnimation)
        {
            skeletonAnimation.AnimationState.SetAnimation(movementTrack, jumpAnimation, false);
        }
    }
    
    public void SetFireAnimation()
    {
        if (skeletonAnimation.AnimationName != fireAnimation)
        {
            skeletonAnimation.AnimationState.SetAnimation(actionTrack, fireAnimation, false);
        }
    }

    private void OnAnimationComplete(TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == jumpAnimation)
        {
            isJumping = false;
            colliderAnimator.SetBool("isPlayerJumping", false);
            SetIdleAnimation();
        }
    }
    
    private void OnDestroy()
    {
        skeletonAnimation.AnimationState.Complete -= OnAnimationComplete;
    }
}
