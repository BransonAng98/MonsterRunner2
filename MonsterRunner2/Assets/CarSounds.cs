using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    [SerializeField]private float currentSpeed;

    public AudioSource carAudio;
    public AudioSource hitSound;
    public AudioClip[] carHitSFX;

    private bool iscarHitPlaying = false;
    private float carHitSFXCooldown = 1f;

    public float minPitch;
    public float maxPitch;
    private float pitchfromCar;
    public DemoPlayer playerScript;
    public Rigidbody carRb;
    // Start is called before the first frame update
    void Start()
    {
        //carAudio = GetComponent<AudioSource>();
        //carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
      
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


        if (currentSpeed > maxSpeed)
        {
            carAudio.pitch = maxPitch;
        }
    }

    public void StartSound()
    {
        carAudio.Play();
        carAudio.loop = true;
    }

    public void StopEngine(float duration)
    {
        StartCoroutine(FadeSound(duration));
    }

    private IEnumerator FadeSound( float duration)
    {
        float elaspedTime = 0f;
        float startvolume = carAudio.volume;

        while (elaspedTime < duration)
        {
            carAudio.volume = Mathf.Lerp(startvolume, 0f, elaspedTime / duration);
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        carAudio.volume = 0f;
    }

    public void playCarHit()
    {
        if (!iscarHitPlaying)
        {
            StartCoroutine(PlayCarHitWithCooldown());
        }
    }

    private IEnumerator PlayCarHitWithCooldown()
    {
        iscarHitPlaying = true;

        AudioClip soundtoPlay = carHitSFX[Random.Range(0, carHitSFX.Length)];
        hitSound.PlayOneShot(soundtoPlay);

        yield return new WaitForSeconds(carHitSFXCooldown);

        iscarHitPlaying = false;
    }
}
