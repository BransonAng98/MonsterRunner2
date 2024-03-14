using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float rotationSpeed;
    public Rigidbody rb;
    public Transform player;
    public LayerMask groundLayer;
  
    public GameObject bloodSplatter;
    public Material deadMaterial;

    public bool isGrounded;
    public bool CanMove;

    public EnemySO enemyData;

    [SerializeField] float health;
    [SerializeField] float speed;
    [SerializeField] float attackCD;
    [SerializeField] float attackDmg;
    [SerializeField] float weight;

    [SerializeField] private float targetRotationAngle;
    [SerializeField] private bool isTurningRight; // Indicates if the enemy is turning right 
    [SerializeField] private bool isTurningLeft; // Indicates if the enemy is turning left
    //[SerializeField] private bool walkingStraight; // Indicates if the enemy is turning left
    [SerializeField] private bool isDead = false; // Flag to track if the enemy is dead
    [SerializeField] private float distanceToPlayer;
    //[SerializeField] private float cooldownDuration = 3f; // Duration of cooldown after colliding with playerr

    private Coroutine slowDownCoroutine;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Assumes player has "Player" tag
        groundLayer = LayerMask.GetMask("Ground");
        CanMove = true;

        health = enemyData.health;
        speed = enemyData.speed;
        attackCD = enemyData.attackCD;
        attackDmg = enemyData.attackDmg;
        weight = enemyData.weight;
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
            //walkingStraight = true;
            isTurningLeft = false;
            isTurningRight = false;
        }
    }

    void MoveMonster()
    {

        Vector3 playerx = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 direction = (playerx - transform.position).normalized;
        rb.velocity = direction * enemyData.speed;
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

        // Disable collider and change layer to a designated one for dead enemies
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy"); // Change layer to DeadEnemy

        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
        else
        {
            if (slowDownCoroutine == null)
            {
                slowDownCoroutine = StartCoroutine(SlowDown());
                Debug.Log(slowDownCoroutine);
            }
        }
    }

    IEnumerator SlowDown()
    {
        // Reduce speed by half
        speed *= 0.5f;

        // Wait for a duration
        yield return new WaitForSeconds(2f);

        // Restore original speed
        speed /= 0.5f;

        // Reset the coroutine reference
        slowDownCoroutine = null;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}
}



