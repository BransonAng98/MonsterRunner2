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

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelColliderl;
        public Axel axel;
    }

    public float maxAcceleration;

    float currentTorque;

    public float turnSensitivity;
    public float maxSteeringAngle;
    public float maxTorque;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    public Joystick joystick;

    float moveInput;
    float steerInput;

    [SerializeField] bool isDrifting;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided");
            Vector3 knockbackDirection = -transform.forward;

            // Apply knockback force
            float knockbackForce = 50f; // Adjust the force as needed
            rb.AddForce(knockbackDirection * knockbackForce * 100f, ForceMode.Impulse);
        }
    }

    void GetInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = joystick.Horizontal;
    }
    void Move()
    {
        foreach (var wheel in wheels)
        {
            if (!isDrifting)
            {
                // Apply forward torque by default with increased acceleration
                float forwardTorque = Mathf.Clamp(50f * maxAcceleration * Time.deltaTime, 0f, maxTorque);
                wheel.wheelColliderl.motorTorque = forwardTorque;

                // Update current torque for next frame
                currentTorque = forwardTorque;
            }
            else
            {
                // Use a fixed torque for drifting to keep it consistent
                float driftTorque = Mathf.Clamp(30f * maxAcceleration * Time.deltaTime, 0f, maxTorque * 0.7f);
                wheel.wheelColliderl.motorTorque = driftTorque;
            }
        }
    }

    void Steer()
    {
        // Check if the vehicle is moving at high speed before allowing drift
        float highSpeedThreshold = 10f; // Adjust this threshold as needed
        bool isMovingFast = rb.velocity.magnitude > highSpeedThreshold;

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                // Calculate the target steer angle based on joystick input
                float steerAngle = steerInput * turnSensitivity * maxSteeringAngle;

                // Check if the joystick input exceeds a threshold for initiating drift
                float driftThreshold = 0.5f; // Adjust as needed
                if (isMovingFast && Mathf.Abs(steerInput) > driftThreshold)
                {
                    isDrifting = true;

                    // Apply increased steering angle for drifting
                    float driftMultiplier = 3f; // Adjust multiplier for stronger drift
                    float driftSteerAngle = steerAngle * driftMultiplier;

                    // Set the steer angle directly without smoothing
                    wheel.wheelColliderl.steerAngle = driftSteerAngle;

                    // Increase sideways friction to tighten drift radius
                    WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewaysFriction.stiffness = 3f; // Increase stiffness for tighter drifting
                    wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;

                    // Apply braking force to decrease speed during drift
                    rb.AddForce(-rb.velocity * 9f, ForceMode.Force);
                }
                else
                {
                    isDrifting = false;

                    // Apply regular steering angle if not drifting
                    wheel.wheelColliderl.steerAngle = steerAngle;

                    // Reset sideways friction stiffness to default
                    WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewaysFriction.stiffness = 1.2f; // Reset stiffness to default
                    wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;
                }
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
                wheel.wheelModel.transform.position = pos;
                wheel.wheelModel.transform.rotation = rot;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        AnimateWheels();
    }

    private void LateUpdate()
    {
        Move();
        Steer();
    }
}
