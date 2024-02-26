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

    public float turnSensitivity;
    public float maxSteeringAngle;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    public Joystick joystick;

    float moveInput;
    float steerInput;

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
            wheel.wheelColliderl.motorTorque = 10f * 50 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var steerAngle = steerInput * turnSensitivity * maxSteeringAngle;

                // Check if the joystick input exceeds a threshold for initiating drift
                float driftThreshold = 0.95f; // Adjust as needed
                if (Mathf.Abs(steerInput) > driftThreshold)
                {
                    // Apply increased steering angle for drifting
                    float driftMultiplier = 3f; // Adjust multiplier for stronger drift
                    float driftSteerAngle = steerInput * turnSensitivity * maxSteeringAngle * driftMultiplier;
                    wheel.wheelColliderl.steerAngle = Mathf.Lerp(wheel.wheelColliderl.steerAngle, driftSteerAngle, 0.1f); // Adjust interpolation factor for smoother transition

                    // Increase wheel slip while drifting
                    WheelFrictionCurve sidewaysFriction = wheel.wheelColliderl.sidewaysFriction;
                    sidewaysFriction.stiffness = 3f; // Adjust stiffness for more slip
                    wheel.wheelColliderl.sidewaysFriction = sidewaysFriction;
                }
                else
                {
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
