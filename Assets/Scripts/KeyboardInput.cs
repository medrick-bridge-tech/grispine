using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    public bool inputLeft, inputRight, inputRun, inputJump, inputFire;
    //private float horizontalAxisValue;
    
    
    void Update()
    {
        UpdateInputValues();
    }
    
    private void UpdateInputValues()
    {
        //horizontalAxisValue = Input.GetAxis("Horizontal");
        inputLeft = Input.GetKey("left");
        inputRight = Input.GetKey("right");
        inputRun = Input.GetKey("left shift");
        inputJump = Input.GetKeyDown("space");
        inputFire = Input.GetKeyDown("left ctrl");
    }
}
