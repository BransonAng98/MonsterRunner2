using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCarDriver : MonoBehaviour
{
    #region Fields
    [SerializeField] private float speed;
    private float speedMax = 18f;
    private float speedMin = 9f;
    private float acceleration = 30f;
    private float brakeSpeed = 100f;
    private float reverseSpeed = 30f;
    private float idleSlowdown = 10f;

    private float turnSpeed;
    private float turnSpeedMax = 300f;
    private float turnSpeedAcceleration = 300f;
    private float turnIdleSlowdown = 500f;


    [SerializeField] private float forwardAmount;
    [SerializeField] private float turnAmount;

    private Rigidbody carRigidbody;
    #endregion

    private void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
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

}