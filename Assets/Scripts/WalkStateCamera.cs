using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WalkStateCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera walkStateCamera;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private PlayerController player;
    
    
    private void OnEnable()
    {
        Register(walkStateCamera);
        player.OnWalkState += SwitchToWalkStateCamera;
    }
    
    private void OnDisable()
    {
        player.OnWalkState -= SwitchToWalkStateCamera;
    }
    
    private void Register(CinemachineVirtualCamera camera)
    {
        cameraSwitcher.Cameras.Add(walkStateCamera);
    }
    
    private void SwitchToWalkStateCamera()
    {
        cameraSwitcher.SwitchCamera(walkStateCamera);
    }
}
