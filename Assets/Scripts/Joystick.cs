using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Joystick : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    private Vector2 touchStartPosition;
    private Vector2 touchCurrentPosition;
    private Vector2 mouseWorldPosition;
    private float cameraXPosition;
    private float mouseXPosition;
    private float mouseYPosition;
    private bool touchStart;
    
    
    void Update()
    {
        cameraXPosition = Camera.main.transform.position.x;
        mouseXPosition = Input.mousePosition.x;
        mouseYPosition = Input.mousePosition.y;
        
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector2(mouseXPosition, mouseYPosition));
    
        if (Input.GetMouseButtonDown(0) && mouseWorldPosition.x < cameraXPosition)
        {
            touchStartPosition = mouseWorldPosition;
            Debug.Log("start");
        }
    
        if (Input.GetMouseButton(0) && mouseWorldPosition.x < cameraXPosition)
        {
            touchStart = true;
            touchCurrentPosition = mouseWorldPosition;
            Debug.Log("continue");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchStart = false;
            Debug.Log("end");
            //player.SetIdleAnimation();
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
            player.SetIdleAnimation();
        }
    }

    private void DetectMovement(Vector2 direction)
    {
        Vector2 directionVector = touchCurrentPosition - touchStartPosition;
        
        if (Vector2.Dot(Vector2.right, direction) > 0.8f && mouseWorldPosition.x < cameraXPosition && directionVector.magnitude <= 1)
        {
            player.SetWalkAnimation();
            player.FlipCharacter(Direction.Right);
        }
        else if (Vector2.Dot(Vector2.right, direction) > 0.8f && mouseWorldPosition.x < cameraXPosition && directionVector.magnitude > 1)
        {
            player.SetRunAnimation();
            player.FlipCharacter(Direction.Right);
        }
        else if (Vector2.Dot(Vector2.left, direction) > 0.8f && mouseWorldPosition.x < cameraXPosition && directionVector.magnitude <= 1)
        {
            player.SetWalkAnimation();
            player.FlipCharacter(Direction.Left);
        }
        else if (Vector2.Dot(Vector2.left, direction) > 0.8f && mouseWorldPosition.x < cameraXPosition && directionVector.magnitude > 1)
        {
            player.SetRunAnimation();
            player.FlipCharacter(Direction.Left);
        }
    }
}
