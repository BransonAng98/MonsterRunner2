using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    //AUDIO SOURCE
    public AudioSource bgmSource;
    public AudioSource feedbackAudioSource;
    public AudioSource policeDeath;

    //AUDIO CLIP
    public AudioClip[] bgmClip;
    public AudioClip[] feedbackSFX;
    public AudioClip[] policeDeathSFX;


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
