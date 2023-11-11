using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Spine;
using Spine.Unity;
using UnityEngine;
using Debug = UnityEngine.Debug;

enum Direction
{
    Left,
    Right
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    
    private SkeletonAnimation skeletonAnimation;
    private Rigidbody2D rb;
    private float horizontalAxisValue;
    private bool inputLeft, inputRight, inputJump, inputFire, isGrounded;

    private const string idleAnimation = "idle";
    private const string runAnimation = "run";
    private const string jumpAnimation = "jump";
    private const string fireAnimation = "shoot";

    private const int movementTrack = 0;
    private const int actionTrack = 1;

    public event Action OnIdleState;
    public event Action OnRunState;


    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = true;
    }

    private void Start()
    {
        skeletonAnimation.AnimationState.Complete += OnAnimationComplete;
        skeletonAnimation.AnimationState.SetAnimation(movementTrack, idleAnimation, true);
    }

    private void Update()
    {
        UpdateInputValues();
        UpdateHorizontalMovement();
        UpdateFacing();
        UpdateAnimationState();
    }
    
    private void UpdateInputValues()
    {
        horizontalAxisValue = Input.GetAxis("Horizontal");
        inputLeft = Input.GetKey("left");
        inputRight = Input.GetKey("right");
        inputJump = Input.GetKeyDown("space");
        inputFire = Input.GetKeyDown("left shift");
    }

    private void UpdateHorizontalMovement()
    {
        rb.velocity = new Vector2(horizontalAxisValue * moveSpeed, rb.velocity.y);
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

    private void FlipCharacter(Direction direction)
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
        if ((inputLeft || inputRight) && isGrounded)
        {
            SetRunAnimation();
            OnRunState?.Invoke();
        }
        else if (isGrounded)
        {
            SetIdleAnimation();
            OnIdleState?.Invoke();
        }
        
        if (inputJump)
        {
            SetJumpAnimation();
            isGrounded = false;
        }
        
        if (inputFire)
        {
            SetFireAnimation();
        }
    }

    private void SetRunAnimation()
    {
        if (skeletonAnimation.AnimationName != runAnimation)
        {
            skeletonAnimation.AnimationState.SetAnimation(movementTrack, runAnimation, true);
        }
    }
    
    private void SetIdleAnimation()
    {
        if (skeletonAnimation.AnimationName != idleAnimation)
        {
            skeletonAnimation.AnimationState.SetAnimation(movementTrack, idleAnimation, true);
        }
    }
    
    private void SetJumpAnimation()
    {
        if (skeletonAnimation.AnimationName != jumpAnimation)
        {
            skeletonAnimation.AnimationState.SetAnimation(movementTrack, jumpAnimation, false);
        }
    }
    
    private void SetFireAnimation()
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
            SetIdleAnimation();
            isGrounded = true;
        }
    }
    
    private void OnDestroy()
    {
        skeletonAnimation.AnimationState.Complete -= OnAnimationComplete;
    }
}
