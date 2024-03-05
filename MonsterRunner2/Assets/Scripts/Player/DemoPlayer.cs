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
    public SteeringWheel steeringWheel;

    float steerInput;

    public bool isDrifting;
    public float driftIntensity = 1f;
    public float smoothDriftSteerVelocity = 0f;
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
        steerInput = steeringWheel.GetClampedValue();
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Rear)
            {
                if (!isDrifting)
                {
                    smoothDriftSteerVelocity = 0f;
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
                    float driftTorque = maxAcceleration * 2f;
                    wheel.wheelColliderl.motorTorque = driftTorque;
                }
            }
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                // Calculate the target steer angle based on joystick input
                float steerAngle = steerInput * turnSensitivity * maxSteeringAngle;

                if (!isDrifting)
                {
                    // Reset sideways friction stiffness to default
                    WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewaysFriction.stiffness = 4f; // Reset stiffness to default
                    wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;

                    // Interpolate back to regular steer angle
                    wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, steerAngle, Time.deltaTime * 1000f);
                }
                else
                {
                    WheelFrictionCurve fFriction = wheel.wheelColliderl.forwardFriction;
                    float forwardFriction = 1f;
                    fFriction.asymptoteValue = forwardFriction;

                    WheelFrictionCurve sFriction = wheel.wheelColliderl.sidewaysFriction;
                    float sidewayFriction = 0.3f;
                    sFriction.asymptoteValue = sidewayFriction;

                    // Interpolate back to drift steer angle

                    float driftSteerAngle = steerAngle * driftIntensity;
                    // Interpolate back to regular steer angle
                    wheel.wheelColliderl.steerAngle = Mathf.SmoothDamp(wheel.wheelColliderl.steerAngle, driftSteerAngle, ref smoothDriftSteerVelocity, Time.deltaTime * 0.5f);
                }
            }

            if(wheel.axel == Axel.Rear)
            {
                if (isDrifting)
                {
                    WheelFrictionCurve forwardFriction = wheel.wheelColliderl.forwardFriction;
                    forwardFriction.stiffness = Mathf.SmoothDamp(forwardFriction.stiffness, 2f, ref smoothDriftSteerVelocity, Time.deltaTime * 2f);
                    wheel.wheelColliderl.forwardFriction = forwardFriction;

                    WheelFrictionCurve sidewayFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewayFriction.stiffness = Mathf.SmoothDamp(sidewayFriction.stiffness, 1.5f, ref smoothDriftSteerVelocity, Time.deltaTime * 2f);
                    wheel.wheelColliderl.sidewaysFriction = sidewayFriction;
                }

                else
                {
                    WheelFrictionCurve forwardStiff = wheel.wheelColliderl.forwardFriction;
                    forwardStiff.stiffness = 8f;
                    wheel.wheelColliderl.forwardFriction = forwardStiff;

                    WheelFrictionCurve sideStiff = wheel.wheelColliderl.sidewaysFriction;
                    sideStiff.stiffness = 2f;
                    wheel.wheelColliderl.sidewaysFriction = sideStiff;
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
                //wheel.wheelModel.transform.position = pos;
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
