using System;
using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float minSwipeDistance = 0.2f;
    [SerializeField] private float maxSwipeTime = 1f;
    [SerializeField, Range(0f, 1f)] private float directionThreshold = 0.8f;
    
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float startTime;
    private float endTime;
    

    private void OnEnable()
    {
        inputManager.OnStartTouch += StartSwipe;
        inputManager.OnEndTouch += EndSwipe;
    }
    
    private void OnDisable()
    {
        inputManager.OnStartTouch -= StartSwipe;
        inputManager.OnEndTouch -= EndSwipe;
    }

    private void StartSwipe(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;
    }

    private void EndSwipe(Vector2 position, float time)
    {
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector2.Distance(startPosition, endPosition) >= minSwipeDistance && (endTime - startTime) <= maxSwipeTime && startPosition.x > Camera.main.transform.position.x)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 2f);
            Vector2 direction = endPosition - startPosition;
            direction.Normalize();
            SwipeDirection(direction);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
            player.isJumping = true;
            player.SetJumpAnimation();
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
            player.SetFireAnimation();
        }
    }
}
