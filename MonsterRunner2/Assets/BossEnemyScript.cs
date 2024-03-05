using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyScript : MonoBehaviour
{
    [SerializeField] private float speed; // Adjust this to control the speed of the enemy
    public float rotationSpeed;
    public Rigidbody rb;
    public Transform player;
    private Animator animator;
    public LayerMask groundLayer; // Define the ground layer in the Unity Editor
    public bool isGrounded;
    [SerializeField] private float targetRotationAngle;
    [SerializeField] private bool isTurningRight; // Indicates if the enemy is turning right
    [SerializeField] private bool isTurningLeft; // Indicates if the enemy is turning left
    [SerializeField] private bool walkingStraight; // Indicates if the enemy is turning left
    [SerializeField] private float distanceToPlayer;
    public bool CanMove;

    public float detectionRadius; // Radius within which the enemy detects the player

    public float attackCooldown = 2f; // Cooldown period between attacks
    private float lastAttackTime; // Time when the last attack occurred

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assumes player has "Player" tag
        groundLayer = LayerMask.GetMask("Ground");
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown; // Set initial value to allow immediate attack
        CanMove = false; // Initially, don't move until player is detected
    }

    void Update()
    {
        if (player != null)
        {
            CheckGrounded();
            if (isGrounded)
            {
                if (CanMove)
                {
                    checkRotation();
                    RotateMonster();
                    MoveMonster();
                }

                distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer < detectionRadius)
                {
                    CanMove = true; // Start chasing the player
                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        AttackPlayer();
                        lastAttackTime = Time.time; // Update last attack time
                    }
                }
            }
        }
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

    void checkRotation()
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

    void AttackPlayer()
    {
        animator.SetTrigger("Smash Attack");
        animator.SetBool("Walk Forward", false);
    }

    void MoveMonster()
    {
        animator.SetBool("Walk Forward", true);
        Vector3 playerx = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 direction = (playerx - transform.position).normalized;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanMove = true; // Start chasing the player when player enters detection radius
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            AttackPlayer();
            CanMove = false;
            // Calculate direction from the monster to the collided object
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize(); // Normalize the direction vector

            // Get the Rigidbody component of the collided object
            Rigidbody collidedRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (collidedRigidbody != null)
            {
                StartCoroutine(AddForceWithDelay(collidedRigidbody, direction, 0.5f));
                Debug.Log("Throw");

            }
            // Set CanMove back to true after throwing the obstacle
            StartCoroutine(EnableMovementAfterDelay(1f)); // Enable movement after x seconds
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate direction from the monster to the player
            Vector3 direction = collision.transform.position - transform.position;
            direction.Normalize(); // Normalize the direction vector

            // Get the Rigidbody component of the player
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Apply a force to the player to fling it away
                float forceMagnitude = 20f; // Adjust this value as needed
                float upwardForce = 150f; // Adjust this value to control the height of the arc
                Vector3 forceDirection = direction + Vector3.up * upwardForce; // Add an upward component to the direction
                playerRigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
                Debug.Log("Player Fling");
            }

        }
    }


    IEnumerator AddForceWithDelay(Rigidbody rigidbody, Vector3 direction, float delay)
    {
        yield return new WaitForSeconds(delay); // Change the delay time as needed

        float forceMagnitude = 500f;
        float upwardForce = 400f; // Adjust this value to control the height of the arc
        Vector3 forceDirection = direction + Vector3.up * upwardForce; // Add an upward component to the direction
        rigidbody.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
    }

    private IEnumerator EnableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        CanMove = true;
    }

    // Visualize detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
