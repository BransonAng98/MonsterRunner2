using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    public enum Axel { Front, Rear }
    public enum CarState { Rev, Driving, Collision, Death }
    public enum TrailType { skid }
    public enum SmokeType { normal, drift }
    public enum Health { normal, whiteSmoke, blackSmoke, fire, death }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelColliderl;
        public Axel axel;
    }

    [Serializable]
    public struct Trail
    {
        public TrailRenderer renderer;
        public TrailType trail;
    }

    [Serializable]
    public struct Smoke
    {
        public ParticleSystem smokeRenderer;
        public SmokeType smoke;
    }

    [Serializable]
    public struct HealthState
    {
        public ParticleSystem carSmoke;
        public Health playerHealth;
    }

    [Serializable]
    public struct VehicleData
    {
        public Mesh vehicleBody;
        public AbilitySO ability1;
        public AbilitySO ability2;
    }

    public PlayerSO playerData;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float crashDamage;
    [SerializeField] private float distance;

    public LayerMask enemyLayer;

    public float explosionForce = 1000f;
    public float explosionRadius = 5f;
    public float turnSensitivity;
    public float maxSteeringAngle;
    public float knockBack;
    public float minimumKnockBack;
    public float velocityLerpFactor;

    public bool isDead;
    public bool isTriggered;
    public bool canMove;

    public Vector3 centerOfMass;
    public PlayerAbilityManager abilityManager;
    public PlayerDataManager playerDataManager;

    public VehicleData vehicleData;
    public List<Wheel> wheels;
    public List<Trail> trails;
    public List<Smoke> smokes;
    public List<GameObject> secondarySmokeTrail;
    public List<HealthState> healthSmoke;
    public GameObject[] skillCDParticles;
    public GameObject contractCompleteVFX;

    public Joystick joystick;
    public Vector3 lastKnownVector;
    public Vector2 joystickInput;

    public float acceleration;
    [SerializeField] private float collisionSpeedReduction;
    [SerializeField] private float currentSpeed = 0f;
    private float originalSpeed;
    private Vector3 originalVelocity;
    private Vector3 originalAngularVelocity;
    [SerializeField] private bool isColliding;

    public ParticleSystem healingVFX;

    public Quest quest;
    public QuestDialogueManager questdialogueScript;

    public GameObject destination;
    public GameObject passenger;
    public GameObject impactVFX;

    public float distanceThreshold = 30f;

    public GameMenuManager menuManager;
    private Rigidbody rb;

    public MeshFilter meshFilter;
    public MeshRenderer vehicleMaterial;
    public MeshCollider meshCollider;

    public AbilitySO ability1;
    public AbilitySO ability2;
    public ObjectiveIndicator questIndicator;

    public Transform abillityOrigin;
    public GameObject particleSystem;
    public GameObject collisionVFXPrefab;
    private GameObject instantiatedVFX;
    private int obstacleCollisionCount = 0;

    public GameObject invunSphere;
    public GameObject abilityActivatedVFX;

    private void Awake()
    {
        health = playerData.health;
        maxHealth = playerData.health;
        maxSpeed = playerData.maxSpeed;
        acceleration = playerData.acceleration;
        collisionSpeedReduction = playerData.decceleration;
        crashDamage = 150f - playerData.crashResistance;
        ability1.AssignVariables(abillityOrigin, this.transform);
        ability1.LevelUpSkill(playerData.ability1Level);
        ability2.LevelUpSkill(playerData.ability2Level);
        ability2.AssignVariables(this.transform, this.transform);
        abilityActivatedVFX.SetActive(false);
        invunSphere.SetActive(false);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        GetComponent<WeaponScript>().enabled = false;
        healingVFX.Stop();
        lastKnownVector = transform.forward * maxSpeed;
        contractCompleteVFX.SetActive(false);
        foreach (GameObject particles in skillCDParticles)
        {
            particles.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.layer != 9)
            {
                TakeDamage(1000);
                Vector3 ExplodePos = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
                Instantiate(impactVFX, ExplodePos, Quaternion.identity);
            }
        }

        if (collision.gameObject.CompareTag("AbilityToken"))
        {
            abilityManager.abilityID = collision.gameObject.GetComponent<AbilityToken>().abilityID;
            abilityManager.isTriggered = true;
            PowerUp triggerCollectedVFX = collision.gameObject.GetComponent<PowerUp>();
            triggerCollectedVFX.Pickup();
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (obstacleCollisionCount == 0) // First obstacle collision
            {
                Vector3 contactPoint = collision.contacts[0].point; // Get the first contact point
                InstantiateCollisionVFX(contactPoint);
            }
            obstacleCollisionCount++;
            isColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            obstacleCollisionCount--;
            isColliding = false;
            if (obstacleCollisionCount == 0)
            {
                DestroyCollisionVFX();
            }
        }
    }

    void GetInput()
    {
        joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
    }

    void NewMove()
    {
        for (int i = 0; i < trails.Count; i++)
        {
            if (!trails[i].renderer.gameObject.activeSelf)
            {
                trails[i].renderer.gameObject.SetActive(true);
                secondarySmokeTrail[i].SetActive(true);
            }
        }

        if (!isColliding)
        {
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);
        }
        else
        {
            currentSpeed *= collisionSpeedReduction;
        }

        Vector3 rotatedInputDirection = Quaternion.Euler(0, 45, 0) * new Vector3(joystickInput.x, 0, joystickInput.y);
        if (rotatedInputDirection.magnitude >= maxSteeringAngle)
        {
            Vector3 targetDirection = rotatedInputDirection;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSensitivity);
            Vector3 targetVelocity = transform.forward * currentSpeed;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, velocityLerpFactor);
            lastKnownVector = targetDirection;
        }
        else
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastKnownVector, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSensitivity);
            Vector3 targetVelocity = transform.forward * currentSpeed;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, velocityLerpFactor);
            rb.rotation = Quaternion.LookRotation(lastKnownVector);
        }
    }

    void InstantiateCollisionVFX(Vector3 contactPoint)
    {
        if (collisionVFXPrefab != null && instantiatedVFX == null)
        {
            instantiatedVFX = Instantiate(collisionVFXPrefab, contactPoint, Quaternion.identity);
            instantiatedVFX.transform.SetParent(transform);
        }
    }

    void DestroyCollisionVFX()
    {
        if (instantiatedVFX != null)
        {
            Destroy(instantiatedVFX);
            instantiatedVFX = null;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            Death();
        }
    }

    void CheckHealthState()
    {
        float healthPercentage = (health / maxHealth) * 100f;

        foreach (var smoke in healthSmoke)
        {
            if (healthPercentage <= 100f && healthPercentage > 75f)
            {
                if (smoke.playerHealth == Health.death)
                {
                    smoke.carSmoke.gameObject.SetActive(false);
                }
                else
                {
                    smoke.carSmoke.enableEmission = false;
                }
            }
            if (healthPercentage <= 75f && healthPercentage > 45f)
            {
                if (smoke.playerHealth == Health.whiteSmoke)
                {
                    smoke.carSmoke.enableEmission = true;
                }
                else
                {
                    smoke.carSmoke.enableEmission = false;
                }
            }
            if (healthPercentage <= 45f && healthPercentage > 10)
            {
                if (smoke.playerHealth == Health.blackSmoke)
                {
                    smoke.carSmoke.enableEmission = true;
                }
                else
                {
                    smoke.carSmoke.enableEmission = false;
                }
            }
            if (healthPercentage <= 10f && healthPercentage > 0)
            {
                if (smoke.playerHealth == Health.fire)
                {
                    smoke.carSmoke.enableEmission = true;
                }
                else
                {
                    smoke.carSmoke.enableEmission = false;
                }
            }
        }
    }

    public void GetDestination()
    {
        QuestGiver questGiver = gameObject.GetComponentInChildren<QuestGiver>();
    }

    void Death()
    {
        Explode();
        StartCoroutine(OpenResultScreenAfterDelay(3f));
    }

    public void DamagedByBullet()
    {
        TakeDamage(200);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (collider.transform.position - transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode.Impulse);
                canMove = false;
            }
        }
    }

    IEnumerator OpenResultScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        menuManager.resultScreen.SetActive(true);
    }

    void Update()
    {
        GetInput();
        CheckHealthState();
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            if (canMove)
            {
                NewMove();
            }
            else
            {
                for (int i = 0; i < trails.Count; i++)
                {
                    if (trails[i].renderer.gameObject.activeSelf)
                    {
                        trails[i].renderer.gameObject.SetActive(false);
                        secondarySmokeTrail[i].SetActive(false);
                    }
                }
            }
        }
    }

    public void RestoreHealth()
    {
        float healthToAdd = 150;
        healingVFX.Play();
        Invoke("TurnOffVFX", 0.3f);
        if (health + healthToAdd > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += healthToAdd;
        }
    }


    void TurnOffVFX()
    {
        healingVFX.Stop();
    }

    // Stop the car instantly
    public void StopCarInstantly()
    {
        originalSpeed = currentSpeed;
        originalVelocity = rb.velocity;
        originalAngularVelocity = rb.angularVelocity;
        currentSpeed = 0f;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Restore the car's speed
    public void RestoreCarSpeed()
    {
        currentSpeed = originalSpeed;
        rb.velocity = originalVelocity;
        rb.angularVelocity = originalAngularVelocity;
    }
}
