using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Fireball : AbilitySO
{
    public GameObject bulletPf;
    public Transform bulletSpawnLoc;
    public Transform player;
    public float bulletSpeed;
    public int directionInput;
    public float shootingTimer;
    public float shootingInterval;

    private Vector3[] localDirections = new Vector3[]
    {
        Vector3.forward, // 0 - Forward
        Vector3.right,   // 1 - Right
        Vector3.back,    // 2 - Back
        Vector3.left     // 3 - Left
    };

    public override void LevelUpSkill(int abilityLevel)
    {
        switch (abilityLevel)
        {
            case 1:
                //Nothing
                break;
            case 2:
                shootingInterval = 1.5f;
                break;
            case 3:
                shootingInterval = 1.5f;
                bulletSpeed = 25f;
                break;
            case 4:
                shootingInterval = 1f;
                bulletSpeed = 25f;
                break;
            case 5:
                shootingInterval = 1f;
                bulletSpeed = 35f;
                abilityCD--;
                break;
        }

        Debug.Log("Ability level is:" + abilityLevel);
    }

    public override void AssignVariables(Transform origin, Transform playerPos)
    {
        bulletSpawnLoc = origin;
        player = playerPos;
    }

    public override void Activate()
    {
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval)
        {
            shootingTimer = 0f;

            // Determine the direction to shoot based on the player's orientation
            Vector3 shootDirection = bulletSpawnLoc.TransformVector(localDirections[directionInput]);

            // Create the bullet
            GameObject bullet = Instantiate(bulletPf, bulletSpawnLoc.position, Quaternion.identity);
            Destroy(bullet, 8f);

            // Apply force to the bullet in the chosen direction
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = shootDirection.normalized * bulletSpeed; // Normalize the shoot direction
            }
        }
    }

    public override void Deactive()
    {

    }
}
