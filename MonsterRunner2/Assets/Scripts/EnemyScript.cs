using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private float speed; // Adjust this to control the speed of the enemy
    public float rotationSpeed;
    public Rigidbody rb;
    public Transform player;
    public LayerMask groundLayer; // Define the ground layer in the Unity Editor
    public bool isGrounded;

    [SerializeField] private float targetRotationAngle;
    [SerializeField] private bool isTurningRight; // Indicates if the enemy is turning right
    [SerializeField] private bool isTurningLeft; // Indicates if the enemy is turning left
    [SerializeField] private bool walkingStraight; // Indicates if the enemy is turning left
    [SerializeField] private bool isDead = false; // Flag to track if the enemy is dead
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private float cooldownDuration = 3f; // Duration of cooldown after colliding with player

    public GameObject bloodSplatter;
    public bool CanMove;
    public float detectionRadius; // Radius within which the enemy detects the player

    // Minimum distance between enemies
    public float minDistanceBetweenEnemies = 2f;

    // Obstacle avoidance
    public float avoidDistance = 3f; // Distance to check for obstacles
    public float avoidForce = 10f; // Force to avoid obstacles

    // List to store nearby enemies
    private List<Transform> nearbyEnemies = new List<Transform>();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assumes player has "Player" tag
        groundLayer = LayerMask.GetMask("Ground");
        CanMove = false; // Initially, don't move until player is detected
    }

    void Update()
    {
        if (player != null)
        {
            CheckGrounded();

            distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < detectionRadius && !isDead) // Check if the enemy is alive before moving
            {
                CanMove = true; // Start chasing the player
            }

            if (isGrounded && CanMove)
            {
                CheckRotation();
                RotateMonster();
                MoveMonster();
            }
        }

        // Update the list of nearby enemies
        UpdateNearbyEnemies();
    }

    void RotateMonster()
    {
        // Calculate direction to player, ignoring the x-axis
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        // Calculate the rotation to look at the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Calculate rotation angle
        targetRotationAngle = Quaternion.Angle(transform.rotation, targetRotation);
    }

    void CheckRotation()
    {
        // Determine if turning left or right
        Vector3 cross = Vector3.Cross(transform.forward, player.position - transform.position);
        isTurningRight = cross.y >= 0;
        isTurningLeft = cross.y < 0;
        if (targetRotationAngle == 0)
        {
            walkingStraight = true;
            isTurningLeft = false;
            isTurningRight = false;
        }
    }

    void MoveMonster()
    {
        // Apply avoidance behavior
        Vector3 avoidanceMove = Vector3.zero;
        foreach (Transform enemy in nearbyEnemies)
        {
            if (enemy != transform)
            {
                if (Vector3.Distance(enemy.position, transform.position) < minDistanceBetweenEnemies)
                {
                    avoidanceMove += transform.position - enemy.position;
                }
            }
        }

        // Apply obstacle avoidance
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, avoidDistance))
        {
            avoidanceMove += hit.normal * avoidForce;
        }

        Vector3 playerx = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 direction = (playerx - transform.position + avoidanceMove).normalized;
        rb.velocity = direction * speed;
    }

    void CheckGrounded()
    {
        // Perform a raycast downwards to check if the enemy is grounded
        RaycastHit hit;
        float distanceToGround = GetComponent<Collider>().bounds.extents.y;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToGround + 0.1f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void UpdateNearbyEnemies()
    {
        nearbyEnemies.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, minDistanceBetweenEnemies);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy") && collider.transform != transform)
            {
                nearbyEnemies.Add(collider.transform);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead)
            return; // If the enemy is dead, ignore collisions

        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    void DeathEffect()
    {
        Instantiate(bloodSplatter, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void Die()
    {
        DeathEffect();
        isDead = true; // Set the enemy as dead
        CanMove = false; // Stop the enemy's movement
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
