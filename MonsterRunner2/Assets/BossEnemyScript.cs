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

    public float attackCooldown = 2f; // Cooldown period between attacks
    private float lastAttackTime; // Time when the last attack occurred

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assumes player has "Player" tag
        groundLayer = LayerMask.GetMask("Ground");
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown; // Set initial value to allow immediate attack
    }

    void Update()
    {
        if (player != null)
        {
            CheckGrounded();
            if (isGrounded)
            {
                checkRotation();
                RotateMonster();
                if (CanMove)
                {
                    MoveMonster();
                }

                distanceToPlayer = Vector3.Distance(transform.position, player.position);
                if (distanceToPlayer < 3)
                {
                    CanMove = false;
                    if (Time.time - lastAttackTime > attackCooldown)
                    {
                        AttackPlayer();
                        lastAttackTime = Time.time; // Update last attack time
                    }
                }
                else
                {
                    CanMove = true;
                }
            }
        }
    }

    void RotateMonster()
    {
        speed = 0f;
        Vector3 directionToPlayer = player.position - transform.position;

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
        Debug.Log("Attack Player");
        speed = 0;
        animator.SetTrigger("Smash Attack");
        animator.SetBool("Walk Forward", false);
    }

    void MoveMonster()
    {
        speed = 5;
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
}

