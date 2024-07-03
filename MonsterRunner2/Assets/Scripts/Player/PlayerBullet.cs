using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameObject bulletImpactVFX;
    public Material[] matList;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private TrailRenderer trail;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        trail = GetComponent<TrailRenderer>();
        //trail.enabled = false;
        RandomizeSkin();
    }

    void RandomizeSkin()
    {
        if (matList != null && matList.Length > 0)
        {
            int randomIndex = Random.Range(0, matList.Length);
            renderer.material = matList[randomIndex];
            trail.material.color = matList[randomIndex].color;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            renderer.enabled = false;
            bulletImpactVFX.SetActive(true);
            Destroy(gameObject, 2f);
        }

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

        if (collision.gameObject.layer == 3)
        {
            trail.enabled = true;
        }
    }
}
