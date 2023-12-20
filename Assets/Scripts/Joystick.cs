using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour
{
    private Vector2 touchStartPosition;
    private Vector2 touchCurrentPosition;
    private Vector2 mouseWorldPosition;
    private Vector2 mouseScreenPosition;
    private float cameraXPosition;
    private float mouseXPosition;
    private float mouseYPosition;
    private bool touchStart;

    public bool walkRight;
    public bool runRight;
    public bool walkLeft;
    public bool runLeft;
    public bool isPlayerIdle;

    private float transitionDuration = 1f; 
    private float elapsedTime = 0f;
    public float horizontalForce = 0f;
    


    void Update()
    {
        cameraXPosition = Camera.main.transform.position.x;
        mouseXPosition = Input.mousePosition.x;
        mouseYPosition = Input.mousePosition.y;
        mouseScreenPosition = new Vector2(mouseXPosition, mouseYPosition);

        mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mouseXPosition, mouseYPosition));
        
    
        if (Input.GetMouseButtonDown(0) && mouseWorldPosition.x < cameraXPosition)
        {
            touchStartPosition = mouseScreenPosition;
        }
    
        if (Input.GetMouseButton(0) && mouseWorldPosition.x < cameraXPosition)
        {
            touchStart = true;
            touchCurrentPosition = mouseScreenPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchStart = false;
        }
        
        if (touchStart)
        {
            Debug.DrawLine(touchStartPosition, touchCurrentPosition, Color.red, 2f);
            Vector2 direction = touchCurrentPosition - touchStartPosition;
            direction.Normalize();
            DetectMovement(direction);
        }
        else
        {
            isPlayerIdle = true;
            walkLeft = false;
            walkRight = false;
            runLeft = false;
            runRight = false;
            horizontalForce = 0;
            elapsedTime = 0f;
        }
    }

    private void DetectMovement(Vector2 direction)
    {
        Vector2 directionVector = touchCurrentPosition - touchStartPosition;
        
        if (Vector2.Dot(Vector2.right, direction) > 0.8f && mouseWorldPosition.x < cameraXPosition && directionVector.magnitude <= 50)
        {
            walkRight = true;
            isPlayerIdle = false;
            walkLeft = false;
            runLeft = false;
            runRight = false;
            MoveSmoothly();
        }
        else if (Vector2.Dot(Vector2.right, direction) > 0.8f && mouseWorldPosition.x < cameraXPosition && directionVector.magnitude > 50)
        {
            runRight = true;
            isPlayerIdle = false;
            walkLeft = false;
            walkRight = false;
            runLeft = false;
            MoveSmoothly();
        }
        else if (Vector2.Dot(Vector2.left, direction) > 0.8f && mouseWorldPosition.x < cameraXPosition && directionVector.magnitude <= 50)
        {
            walkLeft = true;
            isPlayerIdle = false;
            walkRight = false;
            runLeft = false;
            runRight = false;
            MoveSmoothly();
        }
        else if (Vector2.Dot(Vector2.left, direction) > 0.8f && mouseWorldPosition.x < cameraXPosition && directionVector.magnitude > 50)
        {
            
            runLeft = true;
            isPlayerIdle = false;
            walkLeft = false;
            walkRight = false;
            runRight = false;
            MoveSmoothly();
        }
    }

    private void MoveSmoothly()
    {
        elapsedTime += Time.deltaTime;
        
        if (elapsedTime > transitionDuration)
            elapsedTime = transitionDuration;
        
        float t = elapsedTime / transitionDuration;
        horizontalForce = Mathf.SmoothStep(0f, 1f, t);
    }
}
