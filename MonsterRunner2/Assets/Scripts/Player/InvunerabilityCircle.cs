using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvunerabilityCircle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyCarDriver carAI = other.GetComponent<enemyCarDriver>();
            if(carAI != null)
            {
                carAI.CarDeath(1);
            }
        }
    }
}
