using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    [SerializeField]private float currentSpeed;

    public AudioSource carAudio;

    public float minPitch;
    public float maxPitch;
    private float pitchfromCar;
    public DemoPlayer playerScript;
    public Rigidbody carRb;
    public Joystick joystick;
    public Vector2 joystickInput;
    // Start is called before the first frame update
    void Start()
    {
        //carAudio = GetComponent<AudioSource>();
        //carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        joystick = playerScript.joystick;
        joystickInput = new Vector2(joystick.Horizontal, joystick.Vertical).normalized;
        EngineSound();
    }

    void EngineSound()
    {
        currentSpeed = carRb.velocity.magnitude;
        pitchfromCar = carRb.velocity.magnitude / 50f;

        if (currentSpeed < minSpeed)
        {
            carAudio.pitch = minPitch;

        }

        if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            carAudio.pitch = minPitch + pitchfromCar;
        }

        if (joystickInput.magnitude > 0.1f) // Adjust the threshold as needed
        {
            carAudio.pitch = minPitch + pitchfromCar;
        }

        if (currentSpeed > maxSpeed)
        {
            carAudio.pitch = maxPitch;
        }
    }
}
