using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public float knockbackForce = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Monster"))

            if (collision.gameObject.CompareTag("Monster"))
            {
                // Handle collision with the monster
                // Apply knockback or any other relevant behavior
                Rigidbody rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = collision.transform.position - transform.position;
                    float forceMagnitude = 50f;
                    float upwardForce = 10f; // Adjust this value to control the height of the arc
                    Vector3 forceDirection = direction + Vector3.up * upwardForce; // Add an upward component to the direction
                    rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
                }
            }
            else if (collision.gameObject.CompareTag("Car"))
            {
                // Handle collision with the car
                // For example, you might damage the car or trigger some other effect
                Debug.Log("Car collided with obstacle.");
            }
    }
}

