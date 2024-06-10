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
        Vector3.forward,
        Vector3.right,
        Vector3.back,
        Vector3.left,
    };

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
            // Determine the direction to shoot based on the switch
            Vector3 shootDirection = bulletSpawnLoc.TransformDirection(localDirections[directionInput]);

            // Create the bullet
            GameObject bullet = Instantiate(bulletPf, bulletSpawnLoc.position, Quaternion.identity);
            Destroy(bullet, 8f);
            // Apply force to the bullet in the chosen direction
            bullet.GetComponent<Rigidbody>().AddForce(shootDirection * bulletSpeed, ForceMode.VelocityChange);
        }
    }
}
