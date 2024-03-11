using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public LayerMask enemyLayer;
    public PlayerSO playerData;

    [SerializeField] float damage;

    private void Start()
    {
        damage = playerData.damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            //Damage enemy logic here, use the damage float
        }
    }
}
