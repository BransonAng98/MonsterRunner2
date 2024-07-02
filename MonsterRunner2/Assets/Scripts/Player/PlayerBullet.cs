using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameObject bulletImpactVFX;
    [SerializeField] private MeshRenderer renderer;

    private void Start()
    {
        bulletImpactVFX.SetActive(false);
        renderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemyCarDriver hitEntity = collision.gameObject.GetComponent<enemyCarDriver>();
            //Kill enemy
            hitEntity.CarDeath(1);
            renderer.enabled = false;
            bulletImpactVFX.SetActive(true);
            Destroy(gameObject, 2f);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
