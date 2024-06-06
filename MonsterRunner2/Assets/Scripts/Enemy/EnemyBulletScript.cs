using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 5f;
   

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

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hits an enemy
        if (other.CompareTag("Enemy"))
        {
            // Apply damage to the enemy (assuming the enemy has a script with a TakeDamage method)
         

            // Instantiate an impact effect if one is assigned
          

            // Destroy the bullet
            Destroy(gameObject);
        }
        else
        {
            // Optional: handle other collisions, such as with walls or obstacles
          
            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
