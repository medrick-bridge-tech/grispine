using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RunStateCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera runStateCamera;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private PlayerController player;
    
    
    private void OnEnable()
    {
        Register(runStateCamera);
        player.OnRunState += SwitchToRunStateCamera;
    }
    
    private void OnDisable()
    {
        player.OnRunState -= SwitchToRunStateCamera;
    }
    
    private void Register(CinemachineVirtualCamera camera)
    {
        cameraSwitcher.Cameras.Add(runStateCamera);
    }
    
    private void SwitchToRunStateCamera()
    {
        cameraSwitcher.SwitchCamera(runStateCamera);
    }
}
