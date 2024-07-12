using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WaterTrigger : MonoBehaviour
{
    public DemoPlayer demoPlayer;
    public enemyCarDriver enemyCarDriver;
    public CinemachineVirtualCamera virtualCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = false; // Disable the VCam to stop following the player
            demoPlayer.KillPlayer();
        }
        if (other.CompareTag("Enemy"))
        {
            enemyCarDriver.CarDeath(0);
        }
    }
}
