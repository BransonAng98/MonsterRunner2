using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGunnerScript : MonoBehaviour
{
    public float detectionRange = 10f;
    public float raycastDuration = 0.5f; // Duration for which the raycast line stays active
    public GameObject bulletPrefab; // Reference to the bullet prefab
    public Transform firePoint; // Point from where the bullet will be instantiated
    public DemoPlayer playerdata;

    [SerializeField]private Vector3 lastKnownPosition;
    [SerializeField] private bool playerDetected;
    private bool isRaycastActive = false;
    private float raycastTimer;

    private void Update()
    {
        if (playerdata != null && !playerdata.isDead)
        {
            if (isRaycastActive)
            {
                // Update the raycast timer
                raycastTimer -= Time.deltaTime;
                if (raycastTimer <= 0)
                {
                    // Raycast duration expired, resume tracking
                    isRaycastActive = false;
                }
            }
            else
            {
                // Check if the player is within the detection range
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        playerDetected = true;
                        lastKnownPosition = hit.collider.transform.position;
                        Debug.Log("Player detected at " + lastKnownPosition);

                        // Start the raycast line
                        isRaycastActive = true;
                        raycastTimer = raycastDuration;

                        // Instantiate the bullet
                        FireBullet();
                    }
                }
            }
        }
    }

    private void FireBullet()
    {
        // Instantiate the bullet at the firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<EnemyBulletScript>().AssignData(playerdata);

        // Calculate the direction to the last known position
        Vector3 direction = (lastKnownPosition - firePoint.position).normalized;

        // Set the bullet's velocity or direction
        bullet.GetComponent<Rigidbody>().velocity = direction * bullet.GetComponent<EnemyBulletScript>().speed;
    }

    private void OnDrawGizmos()
    {
        // Draw a visual representation of the detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw a line to the last known position if a player was detected
        if (playerDetected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, lastKnownPosition);
        }
    }
}