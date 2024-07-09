using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    //AUDIO SOURCE
    public AudioSource bgmSource;
    public AudioSource feedbackAudioSource;
    public AudioSource carHit;
    public AudioSource policeDeath;
    public AudioSource policeSiren;
    public AudioSource coinPickup;
    public AudioSource powerupPickup;
    public AudioSource cartoonTalking;

    //AUDIO CLIP
    public AudioClip[] bgmClip;
    public AudioClip[] feedbackSFX;
    public AudioClip[] policeDeathSFX;
    public AudioClip policeSirenSFX;
    public AudioClip[] coinPickupSFX;
    public AudioClip[] carHitSFX;
    public AudioClip[] powerupSFX;
    public AudioClip[] cartoontalkingSFX;


    //BOOLS
    private bool ispoliceDeathPlaying = false;
    private float policedeathCooldown = 0.1f;
    private bool iscoinSFXPlaying = false;
    private float coinSFXCooldown = 0.01f;
    private bool iscarHitPlaying = false;
    private float carHitSFXCooldown = 0.1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayBGM()
    {

        AudioClip soundtoPlay = bgmClip[Random.Range(1, 1)]; //play Chase Music
        bgmSource.PlayOneShot(soundtoPlay);
    }

    public void PlayIdleBGM()
    {

        AudioClip soundtoPlay = bgmClip[Random.Range(2, 2)]; //play Idle Music
        bgmSource.PlayOneShot(soundtoPlay);
    }

    public void PlayTap()
    {
        AudioClip soundtoPlay = feedbackSFX[Random.Range(0, 1)];
        feedbackAudioSource.PlayOneShot(soundtoPlay);
    }

    public void PlayPowerUp()
    {
        AudioClip soundtoPlay = powerupSFX[Random.Range(0, powerupSFX.Length)]; //play Idle Music
        powerupPickup.PlayOneShot(soundtoPlay);
    }
    public void PlayCartoonTalking()
    {
        AudioClip soundtoPlay = cartoontalkingSFX[Random.Range(0, cartoontalkingSFX.Length)]; //play Idle Music
        cartoonTalking.PlayOneShot(soundtoPlay);
    }

    public void PlayPoliceSiren()
    {
        policeSiren.volume = 0.7f;
        policeSiren.clip = policeSirenSFX;
        policeSiren.loop = true;
        policeSiren.Play();
    }

    public void StopPoliceSiren(float duration)
    {
        StartCoroutine(FadeOutSiren(duration));
    }

    private IEnumerator FadeOutSiren(float duration)
    {
        float elaspedTime = 0f;
        float startvolume = policeSiren.volume;

        while(elaspedTime < duration)
        {
            policeSiren.volume = Mathf.Lerp(startvolume, 0f, elaspedTime / duration);
            elaspedTime += Time.deltaTime;
            yield return null;
        }

        policeSiren.volume = 0f;
    }



    public void playPoliceDeathSFX()
    {
        if(!ispoliceDeathPlaying)
        {
            StartCoroutine(PlayPoliceDeathWithCooldown());
        }
    }

    private IEnumerator PlayPoliceDeathWithCooldown()
    {
        ispoliceDeathPlaying = true;

        AudioClip deathsoundtoPlay = policeDeathSFX[Random.Range(0, policeDeathSFX.Length)];
        policeDeath.PlayOneShot(deathsoundtoPlay);

        yield return new WaitForSeconds(policedeathCooldown);

        ispoliceDeathPlaying = false;
    }
    public void playCoinPickup()
    {
        if (!iscoinSFXPlaying)
        {
            StartCoroutine(PlayCoinPickUpWithCooldown());
        }
    }

    private IEnumerator PlayCoinPickUpWithCooldown()
    {
        iscoinSFXPlaying = true;

        AudioClip coinsoundtoPlay = coinPickupSFX[Random.Range(0, coinPickupSFX.Length)];
        coinPickup.PlayOneShot(coinsoundtoPlay);

        yield return new WaitForSeconds(coinSFXCooldown);

        iscoinSFXPlaying = false;
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
        carHit.PlayOneShot(soundtoPlay);

        yield return new WaitForSeconds(coinSFXCooldown);

        iscarHitPlaying = false;
    }
}

