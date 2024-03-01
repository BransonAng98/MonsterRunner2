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

    float moveInput;
    float steerInput;

    public bool isDrifting;

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
                    float driftTorque = maxAcceleration * 1.5f;
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

                // Reset sideways friction stiffness to default
                WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                sidewaysFriction.stiffness = 4f; // Reset stiffness to default
                wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;

                // Interpolate back to regular steer angle
                wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, steerAngle, Time.deltaTime * 50f);
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
