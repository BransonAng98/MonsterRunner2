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

    public Material deadMaterial;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assumes player has "Player" tag
        groundLayer = LayerMask.GetMask("Ground");
        CanMove = true;
    }

    void Update()
    {
        // If player is null or the enemy is dead, return early
        if (player == null || isDead)
            return;
        else
        {
            CheckGrounded();

            // Only move if the enemy is grounded, can move, and is alive
            if (isGrounded && CanMove)
            {
                RotateMonster();
                MoveMonster();
                CheckRotation();
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
        Destroy(gameObject,3f);
    }

    void Die()
    {
        DeathEffect();
        isDead = true; // Set the enemy as dead
        CanMove = false; // Stop the enemy's movement

        // Change material to deadMaterial
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && deadMaterial != null)
        {
            renderer.material = deadMaterial;
        }
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}
}
