using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;
    public DemoPlayer playerdata;



    private void Awake()
    {
       
    }
    private void Start()
    {
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    public  void AssignData(DemoPlayer player)
    {
        playerdata = player;
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet hits an enemy
        if (collision.gameObject.CompareTag("Player"))
        {
            playerdata.DamagedByBullet();
            Debug.Log("DamagedByBullet");
            // Destroy the bullet
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("destroy Bullet");
            Destroy(gameObject);
        }
    }
}
