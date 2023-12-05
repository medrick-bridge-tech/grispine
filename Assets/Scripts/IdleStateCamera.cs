using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class IdleStateCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera idleStateCamera;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private PlayerController player;
    
    
    private void OnEnable()
    {
        Register(idleStateCamera);
        cameraSwitcher.SwitchCamera(idleStateCamera);
        player.OnIdleState += SwitchToIdleStateCamera;
    }
    
    private void OnDisable()
    {
        player.OnIdleState -= SwitchToIdleStateCamera;
    }
    
    private void Register(CinemachineVirtualCamera camera)
    {
        cameraSwitcher.Cameras.Add(idleStateCamera);
    }
    
    private void SwitchToIdleStateCamera()
    {
        cameraSwitcher.SwitchCamera(idleStateCamera);
    }
}
