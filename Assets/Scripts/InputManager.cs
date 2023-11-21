using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public delegate void StartTouch(Vector2 position, float time);
    public event StartTouch OnStartTouch;

    public delegate void EndTouch(Vector2 position, float time);
    public event EndTouch OnEndTouch;
    
    private TouchControls touchControls;


    private void Awake()
    {
        touchControls = new TouchControls();
    }

    private void OnEnable()
    {
        touchControls.Enable();
    }

    private void OnDisable()
    {
        touchControls.Disable();
    }

    void Start()
    {
        touchControls.Touch.PrimaryContact.started += context => StartTouchPrimary(context);
        touchControls.Touch.PrimaryContact.canceled += context => EndTouchPrimary(context);
    }

    private void StartTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
        {
            OnStartTouch(Camera.main.ScreenToWorldPoint(touchControls.Touch.PrimaryPosition.ReadValue<Vector2>()),
                (float) context.startTime);
        }
    }

    private void EndTouchPrimary(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
        {
            OnEndTouch(Camera.main.ScreenToWorldPoint(touchControls.Touch.PrimaryPosition.ReadValue<Vector2>()),
                (float) context.time);
        }
    }
}
