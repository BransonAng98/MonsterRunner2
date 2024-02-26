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
    public float brakeAcceleration;

    float currentTorque;

    public float turnSensitivity;
    public float maxSteeringAngle;

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
        foreach(var wheel in wheels)
        {
            if (!isDrifting)
            {
                wheel.wheelColliderl.motorTorque = 10f * 50 * maxAcceleration * Time.deltaTime;
                currentTorque = wheel.wheelColliderl.motorTorque;
            }

            else
            {
                wheel.wheelColliderl.motorTorque = Mathf.Lerp( currentTorque, 0.05f, 0.1f);
                wheel.wheelColliderl.motorTorque = currentTorque * Time.deltaTime;
            }
        }
    }
    void Steer()
    {
        // Declare and initialize _stiffnessVelocity
        float _stiffnessVelocity = 0f;

        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * turnSensitivity * maxSteeringAngle;

                // Check if the joystick input exceeds a threshold for initiating drift
                float driftThreshold = 0.5f; // Adjust as needed
                if (Mathf.Abs(steerInput) > driftThreshold)
                {
                    isDrifting = true;
                    // Apply increased steering angle for drifting
                    float driftMultiplier = 2.5f; // Adjust multiplier for stronger drift
                    float driftSteerAngle = steerInput * turnSensitivity * maxSteeringAngle * driftMultiplier;
                    wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, driftSteerAngle, 4f); // Adjust interpolation factor for smoother transition

                    // Smoothly increase wheel slip while drifting
                    float targetStiffness = 10f; // Adjust target stiffness for more slip
                    WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewaysFriction.stiffness = Mathf.SmoothDamp(sidewaysFriction.stiffness, targetStiffness, ref _stiffnessVelocity, 0.2f); // Adjust smooth time for smoother transition
                    wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;

                    // Apply braking force to decrease speed during drift
                    rb.AddForce(-rb.velocity * 0.5f, ForceMode.Force);
                }
                else
                {
                    isDrifting = false;
                    // Apply regular steering angle
                    float regularSteerAngle = steerInput * turnSensitivity * maxSteeringAngle;
                    wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, regularSteerAngle, 0.6f); // Adjust interpolation factor for smoother transition

                    // Reset wheel slip to normal
                    WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewaysFriction.stiffness = 1f; // Reset stiffness to default
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
