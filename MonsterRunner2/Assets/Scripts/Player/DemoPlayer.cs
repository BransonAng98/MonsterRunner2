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
        smoke,
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

    public float maxAcceleration;

    float currentTorque;

    public float turnSensitivity;
    public float maxSteeringAngle;
    public float maxSpeed;

    public Vector3 centerOfMass;

    public List<Wheel> wheels;

    public List<Trail> trails;

    public SteeringWheel steeringWheel;

    float steerInput;

    public bool inputSteer;
    public float knockBack;
    public float minimumKnockBack;

    Rigidbody rb;

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

                //if (!inputSteer)
                //{
                //    // Check if current speed is less than max speed
                //    if (rb.velocity.magnitude < maxSpeed)
                //    {
                //        // Apply forward torque with increased acceleration until max speed is reached
                //        float forwardTorque = maxAcceleration; // Increased base torque
                //        wheel.wheelColliderl.motorTorque = forwardTorque;

                //        // Update current torque for next frame
                //        currentTorque = forwardTorque;
                //    }
                //    else
                //    {
                //        // Once max speed is reached, stop applying torque
                //        wheel.wheelColliderl.motorTorque = 0f;
                //    }
                //}
                //else
                //{
                //    // Check if current speed is less than max speed
                //    if (rb.velocity.magnitude < maxSpeed * 0.65f)
                //    {
                //        // Apply forward torque with increased acceleration until max speed is reached
                //        float forwardTorque = maxAcceleration * 0.5f; // Increased base torque
                //        wheel.wheelColliderl.motorTorque = forwardTorque;

                //        // Update current torque for next frame
                //        currentTorque = forwardTorque;
                //    }
                //    else
                //    {
                //        // Once max speed is reached, stop applying torque
                //        wheel.wheelColliderl.motorTorque = 0f;
                //    }
                //}
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
                    wheel.wheelColliderl.steerAngle = steerAngle;
                }
            }
        }
        else
        {
            foreach(var wheel in wheels)
            {
                wheel.wheelColliderl.steerAngle = 0f;
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

    public void releaseWheel()
    {
        inputSteer = false;
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
