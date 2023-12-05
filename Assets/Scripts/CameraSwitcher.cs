using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    private List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    public List<CinemachineVirtualCamera> Cameras
    {
        get => cameras;
        set => cameras = value;
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
}
