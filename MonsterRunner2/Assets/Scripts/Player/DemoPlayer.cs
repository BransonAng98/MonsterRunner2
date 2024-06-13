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

    public Vector3 centerOfMass;
    public PlayerAbilityManager abilityManager;
    public PlayerDataManager playerDataManager;

    public VehicleData vehicleData;
    public List<Wheel> wheels;
    public List<Trail> trails;
    public List<Smoke> smokes;
    public List<HealthState> healthSmoke;

    public Joystick joystick;
    public Vector3 lastKnownVector;
    public Vector2 joystickInput;

    public ParticleSystem healingVFX;

    public Quest quest;
    public QuestDialogueManager questdialogueScript;

    public GameObject destination;
    public GameObject passenger;
    public GameObject impactVFX;

    public float distanceThreshold = 30f;

    public GameMenuManager menuManager;
    private Rigidbody rb;

    public bool destinationReached;

    public MeshFilter meshFilter;
    public MeshRenderer vehicleMaterial;
    public MeshCollider meshCollider;

    public AbilitySO ability1;
    public AbilitySO ability2;
    public ObjectiveIndicator questIndicator;

    public Transform abillityOrigin;
    public GameObject particleSystem;

    private void Awake()
    {
        health = playerData.health;
        maxHealth = playerData.health;
        maxSpeed = playerData.maxSpeed;
        crashDamage = 150f - playerData.crashResistance;
        ability1.AssignVariables(abillityOrigin, this.transform);
        ability2.AssignVariables(this.transform, this.transform);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        GetComponent<WeaponScript>().enabled = false;
        healingVFX.Stop();
        lastKnownVector = transform.forward * maxSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!isTriggered)
            {
                isTriggered = true;
                TakeDamage(crashDamage);
            }
        }

        if (other.CompareTag("Passenger"))
        {
            destinationReached = false;
            passenger = other.gameObject;
            QuestGiver questGiver = other.transform.GetComponent<QuestGiver>();
            destination = questGiver.destination;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            TakeDamage(1000);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.layer != 9)
            {
                TakeDamage(1000);
                Vector3 knockbackDirection = transform.position - collision.contacts[0].point;
                Vector3 spawnPos = collision.contacts[0].point;
                Instantiate(impactVFX, spawnPos, Quaternion.identity);
                knockbackDirection.Normalize();
                float knockbackForce = collision.impulse.magnitude * knockBack;
                knockbackForce = Mathf.Max(knockbackForce, minimumKnockBack);
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }

        if (collision.gameObject.CompareTag("AbilityToken"))
        {
            abilityManager.abilityID = collision.gameObject.GetComponent<AbilityToken>().abilityID;
            abilityManager.isTriggered = true;
        }
    }

    void GetInput()
    {
        joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
    }

    void NewMove()
    {
        Vector3 rotatedInputDirection = Quaternion.Euler(0, 45, 0) * new Vector3(joystickInput.x, 0, joystickInput.y);
        if (rotatedInputDirection.magnitude >= maxSteeringAngle)
        {
            Vector3 targetDirection = rotatedInputDirection;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSensitivity);
            Vector3 targetVelocity = transform.forward * maxSpeed;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, velocityLerpFactor);
            lastKnownVector = rb.velocity;
        }
        else
        {
            Vector3 targetVelocity = lastKnownVector;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = targetVelocity;
            rb.rotation = Quaternion.LookRotation(lastKnownVector);
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

        if (!destinationReached && destination != null)
        {
            distance = Vector3.Distance(transform.position, destination.transform.position);
            if (distance <= distanceThreshold)
            {
                DestinationReached();
                destinationReached = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            NewMove();
        }
    }

    public void DestinationReached()
    {
        if (passenger != null)
        {
            int index = UnityEngine.Random.Range(0, 1);
            questdialogueScript.TypeText(false, index);
            quest.Complete();
            passenger.SetActive(true);
            passenger.transform.parent = null;
            passenger = null;
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
}
