using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    //AUDIO SOURCE
    public AudioSource bgmSource;
    public AudioSource feedbackAudioSource;
    public AudioSource policeDeath;
    public AudioSource policeSiren;

    //AUDIO CLIP
    public AudioClip[] bgmClip;
    public AudioClip[] feedbackSFX;
    public AudioClip[] policeDeathSFX;
    public AudioClip policeSirenSFX;


    //BOOLS
    private bool ispoliceDeathPlaying = false;
    private float policedeathCooldown = 0.1f;   // Start is called before the first frame update
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
}
