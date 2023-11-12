using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera idleStateCamera;
    [SerializeField] private CinemachineVirtualCamera runStateCamera;
    [SerializeField] private PlayerController player;
    
    private List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();


    private void OnEnable()
    {
        Register(idleStateCamera);
        Register(runStateCamera);
        
        SwitchCamera(idleStateCamera);
    
        player.OnIdleState += SwitchToIdleStateCamera;
        player.OnRunState += SwitchToRunStateCamera;
    }
    
    private void OnDisable()
    {
        player.OnIdleState -= SwitchToIdleStateCamera;
        player.OnRunState -= SwitchToRunStateCamera;
    }

    public void SwitchToIdleStateCamera()
    {
        SwitchCamera(idleStateCamera);
    }
    
    public void SwitchToRunStateCamera()
    {
        SwitchCamera(runStateCamera);
    }
    
    public void SwitchCamera(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;

        foreach (var cam in cameras)
        {
            if (cam != camera && cam.Priority != 0)
            {
                cam.Priority = 0;
            }
        }
    }

    public void Register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }
}
