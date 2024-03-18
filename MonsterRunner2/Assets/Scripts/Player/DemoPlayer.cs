using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayer : MonoBehaviour
{
    public enum Axel
    {
        Front,
        Rear,
    }

    public enum CarState
    {
        Rev,
        Driving,
        Collision,
        Death, 
    }

    public enum TrailType
    {
        skid,
    }   
    public enum SmokeType
    {
       normal,
       drift,
    }

    public enum Health
    {
        normal,
        whiteSmoke,
        blackSmoke,
        fire,
        death,
    }

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

    public PlayerSO playerData;
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] float maxAcceleration;
    [SerializeField] float maxSpeed;
    public LayerMask enemyLayer;

    public float explosionForce = 1000f; // Force of the explosion
    public float explosionRadius = 5f; // Radius of the explosion

    float currentTorque;

    public float turnSensitivity;
    public float maxSteeringAngle;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    public List<Trail> trails;

    public List<Smoke> smokes;

    public List<HealthState> healthSmoke;

    public SteeringWheel steeringWheel;

    float steerInput;

    public bool inputSteer;
    public float knockBack;
    public float minimumKnockBack;

    public Quest quest; // might need to change this to a list if you want to add quest in runtime. 
    public GameObject destination;
    public GameObject passenger;
    public GameObject impactVFX;

    [SerializeField]private float distance;
    public float distanceThreshold = 30f; // Adjust this value as needed

    public PlayerCollider playercollider;
    public QuestDialogueManager questdialogueScript;
    Rigidbody rb;

    public bool destinationReached; // Flag to track if destination has been reached


    private void Awake()
    {
        health = playerData.health;
        maxHealth = playerData.health;
        maxAcceleration = playerData.acceleration;
        maxSpeed = playerData.maxSpeed;
        playercollider = GetComponentInChildren<PlayerCollider>();
        questdialogueScript = GameObject.Find("MissionManager").GetComponent<QuestDialogueManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided");

            // Calculate knockback direction based on collision point
            Vector3 knockbackDirection = transform.position - collision.contacts[0].point;
            Vector3 spawnPos = collision.contacts[0].point; // Corrected variable name
            Instantiate(impactVFX, spawnPos, Quaternion.identity);
            knockbackDirection.Normalize();

            // Calculate knockback force based on collision impact force
            float knockbackForce = collision.impulse.magnitude * knockBack; // Multiply by knockBack variable

            // If knockback force is less than the minimum, use the minimum force instead
            knockbackForce = Mathf.Max(knockbackForce, minimumKnockBack);

            // Apply knockback force
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

        }
    }

    void GetInput()
    {
        steerInput = steeringWheel.GetClampedValue();
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {

            if (wheel.axel == Axel.Rear)
            {
                // Check if current speed is less than max speed
                if (rb.velocity.magnitude < maxSpeed)
                {
                    // Apply forward torque with increased acceleration until max speed is reached
                    float forwardTorque = maxAcceleration; // Increased base torque
                    wheel.wheelColliderl.motorTorque = forwardTorque;

                    // Update current torque for next frame
                    currentTorque = forwardTorque;
                }
                else
                {
                    // Once max speed is reached, stop applying torque
                    wheel.wheelColliderl.motorTorque = 0f;
                }
            }

            if(wheel.axel == Axel.Rear)
            {
                if (inputSteer)
                {
                    foreach(var trail in trails)
                    {
                        if(trail.trail == TrailType.skid)
                        {
                            trail.renderer.emitting = true;
                        }
                    }
                    foreach (var smoke in smokes)
                    {
                        if (smoke.smoke == SmokeType.drift)
                        {
                            smoke.smokeRenderer.enableEmission = true;
                        }
                        else
                        {
                            smoke.smokeRenderer.enableEmission = false;
                        }
                    }
                }

                else
                {
                    foreach (var trail in trails)
                    {
                        if (trail.trail == TrailType.skid)
                        {
                            trail.renderer.emitting = false;
                        }
                    }

                    foreach (var smoke in smokes)
                    {
                        if (smoke.smoke == SmokeType.drift)
                        {
                            smoke.smokeRenderer.enableEmission = false;
                        }
                        else
                        {
                            smoke.smokeRenderer.enableEmission = true;
                        }
                    }
 
                }
            }
        }
    }

    void Steer()
    {
        if (inputSteer)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    // Calculate the target steer angle based on joystick input
                    float steerAngle = steerInput * turnSensitivity * maxSteeringAngle;

                    //// Reset sideways friction stiffness to default
                    //WheelFrictionCurve forwardF = wheel.wheelColliderl.forwardFriction;
                    //forwardF.stiffness = 4f; // Reset stiffness to default
                    //wheel.wheelColliderl.sidewaysFriction = forwardF;

                    //// Reset sideways friction stiffness to default
                    //WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    //sidewaysFriction.stiffness = 3f; // Reset stiffness to default
                    //wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;

                    // Interpolate back to regular steer angle
                    wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, steerAngle, 1f);
                }
            }
        }
        else
        {
            foreach(var wheel in wheels)
            {
                wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, 0f, 0.3f);
            }
        }
    }

    void AnimateWheels()
    {
        foreach(var wheel in wheels)
        {
            if(wheel.axel == Axel.Front)
            {
                Quaternion rot;
                Vector3 pos;
                wheel.wheelColliderl.GetWorldPose(out pos, out rot);
                //wheel.wheelModel.transform.position = pos;
                wheel.wheelModel.transform.rotation = rot;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
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
                if(smoke.playerHealth == Health.whiteSmoke)
                {
                    smoke.carSmoke.enableEmission = true;
                    Debug.Log("Play white smoke");
                }
                else
                {
                    smoke.carSmoke.enableEmission = false;
                }
            }
            if (healthPercentage <= 45f && healthPercentage > 10)
            {
                if(smoke.playerHealth == Health.blackSmoke)
                {
                    smoke.carSmoke.enableEmission = true;
                    Debug.Log("Play black smoke");
                }
                else
                {
                    smoke.carSmoke.enableEmission = false;
                }
            }
            if (healthPercentage <= 10f && healthPercentage > 0)
            {
                if(smoke.playerHealth == Health.fire)
                {
                    smoke.carSmoke.enableEmission = true;
                    Debug.Log("Play fire");
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
        //Unparents all the wheels with the body
        foreach(var wheel in wheels)
        {
            wheel.wheelColliderl.gameObject.transform.parent = null;
        }

        foreach (var smoke in healthSmoke)
        {
            if (smoke.playerHealth == Health.death)
            {
                smoke.carSmoke.gameObject.SetActive(true);
                Debug.Log("Play explosion");
            }
            else
            {
                smoke.carSmoke.gameObject.SetActive(false);
            }
        }

        //Add force when the car explodes
        Explode();

        //Disables the demoplayer code so it stops moving and everything else
        this.GetComponent<DemoPlayer>().enabled = false;
    }

    void Explode()
    {
        // Find all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        // Apply explosion force to each enemy
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Calculate the direction away from the explosion point
                Vector3 direction = (collider.transform.position - transform.position).normalized;

                // Apply the explosion force
                rb.AddForce(direction * explosionForce, ForceMode.Impulse);
            }
        }
    }

    public void releaseWheel()
    {
        inputSteer = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        AnimateWheels();
        CheckHealthState();

        if (!destinationReached && destination != null) // Check if destination has not been reached and destination is not null
        {
            distance = Vector3.Distance(transform.position, destination.transform.position);

            if (distance < distanceThreshold)
            {
                DestinationReached();
                destinationReached = true; // Set the flag to true to indicate that the destination has been reached
            }
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
            Debug.Log("Passenger unparented.");
        }
    }

    private void LateUpdate()
    {
        Move();
        Steer();
    }
}
