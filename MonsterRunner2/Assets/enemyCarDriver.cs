using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCarDriver : MonoBehaviour
{
    #region Fields
    [SerializeField] private float speed;
    public float speedMax = 18f;
    public float speedMin = 9f;
    private float acceleration = 30f;
    private float brakeSpeed = 100f;
    private float reverseSpeed = 30f;
    private float idleSlowdown = 10f;

    private float turnSpeed;
    private float turnSpeedMax = 300f;
    private float turnSpeedAcceleration = 300f;
    private float turnIdleSlowdown = 500f;

    public DemoPlayer playerscript;
    [SerializeField] private bool isDead;
    [SerializeField] public bool isCCed;

    [SerializeField] private float forwardAmount;
    [SerializeField] private float turnAmount;

    [SerializeField] private float ccDuration;

    private Rigidbody carRigidbody;

    public float flingForce = 1f; // Adjust this value as needed
    public float drag = 1f; // Adjust drag as needed
    public float angularDrag = 1f; // Adjust angular drag as needed

    public GameObject DeathExplosionVFX;
    #endregion

    private void Awake()
    {
        isDead = false;
        //DeathExplosionVFX.SetActive(false);
        carRigidbody = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        if (playerscript.isDead)
        {
            speed = Mathf.Lerp(speed, 0, Time.deltaTime * 2f); // Adjust the lerp speed as needed
            turnSpeed = Mathf.Lerp(turnSpeed, 0, Time.deltaTime * 2f); // Adjust the lerp speed as needed

            carRigidbody.velocity = transform.forward * speed;
            carRigidbody.angularVelocity = new Vector3(0, turnSpeed * Mathf.Deg2Rad, 0);

            return;
        }

        if (!isDead) //if im not dead or im not being crowd controlled
        {
            if (forwardAmount > 0)
            {
                // Calculate acceleration based on current speed ratio
                float speedRatio = speed / speedMax;
                float accelerationFactor = 1 - speedRatio; // Inverse acceleration: slower as it approaches max speed
                float currentAcceleration = acceleration * accelerationFactor;

                // Accelerating
                speed += forwardAmount * currentAcceleration * Time.deltaTime;
            }
            else if (forwardAmount == 0)
            {
                // Slow down when not accelerating
                if (speed > 0)
                {
                    speed -= idleSlowdown * Time.deltaTime;
                }
                else if (speed < 0)
                {
                    speed += idleSlowdown * Time.deltaTime;
                }
            }

            // Gradually reduce speed when turning
            float turnSpeedReductionRate = 5f; // Adjust as needed for the desired speed reduction rate
            if (turnAmount != 0)
            {
                // Calculate turn speed reduction based on turn amount and reduction rate
                float turnSpeedReduction = Mathf.Abs(turnAmount) * turnSpeedReductionRate * Time.deltaTime;
                speed -= turnSpeedReduction;
            }

            // Clamp speed within limits
            speed = Mathf.Clamp(speed, speedMin, speedMax);

            carRigidbody.velocity = transform.forward * speed;

            if (speed < 0)
            {
                // Going backwards, invert wheels
                turnAmount = turnAmount * -1f;
            }

            if (turnAmount > 0 || turnAmount < 0)
            {
                // Turning
                if ((turnSpeed > 0 && turnAmount < 0) || (turnSpeed < 0 && turnAmount > 0))
                {
                    // Changing turn direction
                    float minTurnAmount = 20f;
                    turnSpeed = turnAmount * minTurnAmount;
                }
                turnSpeed += turnAmount * turnSpeedAcceleration * Time.deltaTime;
            }
            else
            {
                // Not turning
                if (turnSpeed > 0)
                {
                    turnSpeed -= turnIdleSlowdown * Time.deltaTime;
                }
                if (turnSpeed < 0)
                {
                    turnSpeed += turnIdleSlowdown * Time.deltaTime;
                }
                if (turnSpeed > -1f && turnSpeed < +1f)
                {
                    // Stop rotating
                    turnSpeed = 0f;
                }
            }

            float speedNormalized = speed / speedMax;
            float invertSpeedNormalized = Mathf.Clamp(1 - speedNormalized, .75f, 1f);

            turnSpeed = Mathf.Clamp(turnSpeed, -turnSpeedMax, turnSpeedMax);

            carRigidbody.angularVelocity = new Vector3(0, turnSpeed * (invertSpeedNormalized * 1f) * Mathf.Deg2Rad, 0);

            if (transform.eulerAngles.x > 2 || transform.eulerAngles.x < -2 || transform.eulerAngles.z > 2 || transform.eulerAngles.z < -2)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }

        if (isCCed)
        {
            ccDuration -= Time.deltaTime;
            if(ccDuration <= 0)
            {
                isCCed = false;
            }
        }
    }

    public void StartCCTimer(float duration)
    {
        isCCed = true;
        ccDuration = duration;
    }

    public void RevertCCState()
    {
        isCCed = false;
        ccDuration = 0f;
    }

    public void SetInputs(float forwardAmount, float turnAmount)
    {
        this.forwardAmount = forwardAmount;
        this.turnAmount = turnAmount;
    }

    public void ClearTurnSpeed()
    {
        turnSpeed = 0f;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeedMax(float speedMax)
    {
        this.speedMax = speedMax;
    }

    public void SetTurnSpeedMax(float turnSpeedMax)
    {
        this.turnSpeedMax = turnSpeedMax;
    }

    public void SetTurnSpeedAcceleration(float turnSpeedAcceleration)
    {
        this.turnSpeedAcceleration = turnSpeedAcceleration;
    }

    public void StopCompletely()
    {
        speed = 0f;
        turnSpeed = 0f;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            isDead = true;
            
            CarDeath();
        }
    }

    public void CarDeath()
    {
        if (carRigidbody != null)
        {
            speed = 0;
            TurnOnExplosion();
           
            carRigidbody.constraints = RigidbodyConstraints.None;

            // Apply an impulse force to fling the car
            Vector3 flingDirection = transform.up + transform.forward; // Adjust direction as needed
            carRigidbody.AddForce(flingDirection * flingForce, ForceMode.Impulse);

            // Apply torque force for rotation
            Vector3 torque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            carRigidbody.AddTorque(torque * flingForce, ForceMode.Impulse);
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            DestroyCar();
        }
    }

    public void DestroyCar()
    {
        Destroy(gameObject, 2f);
    }

    public void TurnOnExplosion()
    {
        
        Instantiate(DeathExplosionVFX, transform.position, Quaternion.identity);
        Debug.Log("Police Explode");
    }
}

