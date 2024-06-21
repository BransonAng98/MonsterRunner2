using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

    public GameObject pickupEffect;

    public void Pickup()
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);
    }
}
