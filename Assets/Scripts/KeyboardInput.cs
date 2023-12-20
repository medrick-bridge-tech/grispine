using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    
    public bool inputLeft, inputRight, inputRun, inputJump, inputFire;
    public float horizontalAxisValue;
    
    
    void Update()
    {
        UpdateInputValues();
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
}
