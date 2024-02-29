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
    public float maxSpeed;

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
            float knockbackForce = 45f; // Adjust the force as needed
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
            if (wheel.axel == Axel.Rear)
            {
                if (!isDrifting)
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
                else
                {
                    // Use a fixed torque for drifting to keep it consistent
                    float driftTorque = maxAcceleration * 0.5f;
                    wheel.wheelColliderl.motorTorque = driftTorque;
                }
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
                float driftThreshold = 0.95f; // Adjust as needed
                float driftBuffer = 0.05f; // Add a buffer zone around the drift threshold

                // Determine if drifting should be initiated or ended
                bool shouldDrift = isMovingFast && Mathf.Abs(steerInput) > driftThreshold;
                bool shouldEndDrift = !isMovingFast || Mathf.Abs(steerInput) < driftThreshold - driftBuffer;

                // Smoothly transition between regular steering and drifting
                if (shouldDrift)
                {
                    isDrifting = true;
                    // Calculate drift steer angle
                    float driftMultiplier = 2f; // Adjust multiplier for stronger drift
                    float driftSteerAngle = steerAngle * driftMultiplier;

                    // Interpolate between current steer angle and drift steer angle
                    wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, driftSteerAngle, Time.deltaTime * 15f);

                    // Increase sideways friction to tighten drift radius
                    WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewaysFriction.stiffness = 10f; // Increase stiffness for tighter drifting
                    wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;

                    // Apply braking force to decrease speed during drift
                    rb.AddForce(-rb.velocity * 5f, ForceMode.Force);
                }
                else if (shouldEndDrift)
                {
                    isDrifting = false;
                    // Reset sideways friction stiffness to default
                    WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewaysFriction.stiffness = 4f; // Reset stiffness to default
                    wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;

                    // Interpolate back to regular steer angle
                    wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, steerAngle, Time.deltaTime * 375f);
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
