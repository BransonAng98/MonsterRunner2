using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyCarDriver hitEntity = collision.gameObject.GetComponent<enemyCarDriver>();
            //Kill enemy
            hitEntity.isDead = true;
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
