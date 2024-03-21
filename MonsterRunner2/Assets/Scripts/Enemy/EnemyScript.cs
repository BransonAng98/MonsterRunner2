using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float rotationSpeed;
    public Rigidbody rb;
    public Transform player;
    public DemoPlayer playerData;
    public LayerMask groundLayer;
    public GameController gameController;
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
    [SerializeField] float minDistanceFromOtherEnemy = 2f; // Minimum distance from other enemies

    [SerializeField] private float targetRotationAngle;
    [SerializeField] private bool isTurningRight;
    [SerializeField] private bool isTurningLeft;
    [SerializeField] private bool isDead = false;
    [SerializeField] private float currentAttackTimer;
    [SerializeField] private float distanceToPlayer;

    private Coroutine slowDownCoroutine;
    [SerializeField] private Transform leader; // Reference to the leader enemy

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Ground");

        InitializeEnemyStats();

        // Designate the first spawned enemy as the leader
        if (gameController.IsFirstEnemy(gameObject.transform))
        {
            leader = transform;
        }
    }

    private void InitializeEnemyStats()
    {
        health = enemyData.health;
        speed = enemyData.speed;
        attackCD = enemyData.attackCD;
        attackDmg = enemyData.attackDmg;
        weight = enemyData.weight;
    }

    private void Update()
    {
        if (player == null || isDead || playerData.isDead)
            return;

        CheckGrounded();

        if (isGrounded && CanMove)
        {
            if (IsLeader())
            {
                RotateMonster();
                MoveMonster();
                CheckRotation();
            }
            else
            {
                FollowLeader();
            }
        }
    }

    private bool IsLeader()
    {
        return leader == transform;
    }

    private void FollowLeader()
    {
        // Calculate direction towards the leader
        Vector3 directionToLeader = leader.position - transform.position;
        directionToLeader.y = 0f;

        // Move towards the leader with some offset
        Vector3 targetPosition = leader.position - directionToLeader.normalized * minDistanceFromOtherEnemy;
        rb.velocity = (targetPosition - transform.position).normalized * speed;
    }

    private void RotateMonster()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        targetRotationAngle = Quaternion.Angle(transform.rotation, targetRotation);
    }

    private void CheckRotation()
    {
        Vector3 cross = Vector3.Cross(transform.forward, player.position - transform.position);
        isTurningRight = cross.y >= 0;
        isTurningLeft = cross.y < 0;
        if (targetRotationAngle == 0)
        {
            isTurningLeft = false;
            isTurningRight = false;
        }
    }

    private void MoveMonster()
    {
        Vector3 playerx = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        Vector3 direction = (playerx - transform.position).normalized;
        rb.velocity = direction * enemyData.speed;
    }

    private void CheckGrounded()
    {
        RaycastHit hit;
        float distanceToGround = GetComponent<Collider>().bounds.extents.y;
        isGrounded = Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToGround + 0.1f, groundLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentAttackTimer <= 0f)
            {
                Attack(other.gameObject);
                currentAttackTimer = attackCD;
            }
        }
    }

    private void Attack(GameObject entity)
    {
        DemoPlayer targetPlayer = entity.GetComponentInParent<DemoPlayer>();
        if (targetPlayer != null)
        {
            targetPlayer.TakeDamage(attackDmg);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameController.UpdateEnemyList(this.gameObject);
            Die();
        }
        else
        {
            if (slowDownCoroutine == null)
            {
                slowDownCoroutine = StartCoroutine(SlowDown());
            }
        }
    }

    private void Die()
    {
        DeathEffect();
        isDead = true;
        CanMove = false;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && deadMaterial != null)
        {
            renderer.material = deadMaterial;
        }
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        }

        // If the leader dies, assign a new leader
        if (IsLeader())
        {
            gameController.AssignNewLeader();
        }
    }
    void DeathEffect()
    {
        Instantiate(bloodSplatter, transform.position, Quaternion.identity);
        Destroy(gameObject, 3f);
    }

    private IEnumerator SlowDown()
    {
        speed *= 0.5f;
        yield return new WaitForSeconds(2f);
        speed /= 0.5f;
        slowDownCoroutine = null;
    }
}



