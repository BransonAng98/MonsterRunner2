using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityMovementAI;

public class Detector : MonoBehaviour
{
    public float detectionRadius;
    public float decelerationRate;

    [SerializeField] private bool playerDetected = false;
    [SerializeField] private bool isSeeking = false;
    [SerializeField] private float currentSpeed;

    public SeekUnit seekUnit;
    public Rigidbody rb;

    private void Start()
    {
        seekUnit = GetComponentInParent<SeekUnit>();
        rb = GetComponentInParent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Check if player is detected before seeking
        if (playerDetected)
        {
            seekUnit.StartSeeking();
        }
        else
        {
            Decelerate();
        }
    }

    private void OnTriggerStay(Collider other)
    {  
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            // Calculate distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, other.transform.position);

            // Check if player is within detection radius
            if (distanceToPlayer <= detectionRadius)
            {
                playerDetected = true; // Set flag to true when player is detected
            }
        }

    }

    // OnTriggerExit is called when another collider exits the trigger
    void OnTriggerExit(Collider other)
    {
        // Check if the collider is the player
        if (other.CompareTag("Player"))
        {
            playerDetected = false; // Reset flag to false when player exits detection radius
        }
    }

    void Decelerate()
    {
  Vector3 currentVelocity = rb.velocity;
    float currentSpeed = currentVelocity.magnitude;
    float targetSpeed = Mathf.Lerp(currentSpeed, 0f, Time.fixedDeltaTime * decelerationRate);
    rb.velocity = currentVelocity.normalized * targetSpeed;
    }

    // Draw the detection radius gizmo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}