using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyScript : MonoBehaviour
{
    public float speed = 5f; // Adjust this to control the speed of the enemy
    public float rotationSpeed;
    public Rigidbody rb;
    public Transform player;
    private Animator animator;
    public LayerMask groundLayer; // Define the ground layer in the Unity Editor
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assumes player has "Player" tag
        groundLayer = LayerMask.GetMask("Ground");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player != null)
        {
            CheckGrounded();
            if(isGrounded == true)
            {
                RotateMonster();
                animator.SetBool("Walk Forward", true);
                Vector3 playerx = new Vector3(player.transform.position.x, 0, player.transform.position.z);
                Vector3 direction = (playerx - transform.position).normalized;
                rb.velocity = direction * speed;
               
            }
          
        }
    }

    void RotateMonster()
    {
        Vector3 directionToPlayer = player.position - transform.position;

        // Calculate the rotation to look at the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

