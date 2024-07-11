using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WaterTrigger : MonoBehaviour
{
    public DemoPlayer demoPlayer;
    public CinemachineVirtualCamera virtualCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = false; // Disable the VCam to stop following the player
            demoPlayer.KillPlayer();
        }
    }
}
