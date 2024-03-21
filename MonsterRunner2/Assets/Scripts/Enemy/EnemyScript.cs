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
    public ScoreManagerScript scoreManager;
    public EnemySO enemyData;

    private float health;
    private float speed;
    private float attackCD;
    private float attackDmg;
    private float weight;

    private bool isGrounded;
    private bool CanMove = true;
    private bool isDead = false;
    private float targetRotationAngle;
    private float currentAttackTimer;

    private Coroutine slowDownCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.GetMask("Ground");

        InitializeEnemyStats();
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
            RotateMonster();
            MoveMonster();
            CheckRotation();
        }
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
        bool isTurningRight = cross.y >= 0;
        bool isTurningLeft = cross.y < 0;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead)
            return; // If the enemy is dead, ignore collisions

        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentAttackTimer > 0f)
            {
                currentAttackTimer -= Time.deltaTime;
                Debug.Log("Counting down");
            }
            else
            {
                Attack(other.gameObject);
                currentAttackTimer = attackCD;
                Debug.Log("Commencing attack");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //currentAttackTimer = 0f;
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

    private void Die()
    {
        DeathEffect();

        isDead = true;
        CanMove = false;
        scoreManager.enemiesKilled++;

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
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("TakingDamager");
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
    void DeathEffect()
    {
        Instantiate(bloodSplatter, transform.position, Quaternion.identity);

        StartCoroutine(DelayShrinkAndDestroy());
    }

    private IEnumerator DelayShrinkAndDestroy()
    {
        float delayTime = 5f;
        yield return new WaitForSeconds(delayTime);

        StartCoroutine(ShrinkAndDestroy());
    }

    private IEnumerator ShrinkAndDestroy()
    {
        float duration = 3f; // Duration of shrinking
        float timer = 0f;

        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.zero;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timer / duration);
            yield return null;
        }

        // Ensure scale is exactly zero
        transform.localScale = targetScale;

        // Destroy the object
        Destroy(gameObject);
    }


    private IEnumerator SlowDown()
    {
        speed *= 0.5f;
        yield return new WaitForSeconds(2f);
        speed /= 0.5f;
        slowDownCoroutine = null;
    }
}



 



