using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public PlayerDataManager playerManager;
    public PlayerDataSO playerData;

    public ScoreManagerScript scoreManager;
    public missionManagerScript missionManager;
    public QuestDialogueManager questDialogue;
    public AbilityTokenManager tokenManager;
    
    public CanvasGroup fadeToBlackImage; 
    public float fadeDuration = 1f; // Duration of the fade effect

    public GameObject exitButton;
    public GameObject retryButton;

    private bool shouldFadeIn = true;
    private float fadeTimer = 0f;

    [SerializeField] private bool hasSpawned;

    private void Start()
    {
        fadeToBlackImage.alpha = 0f;
        exitButton.SetActive(false);
    }

    void SpawnTutorialPassenger()
    {
        if (questDialogue.introSequenceComplete == true)
        {
            missionManager.SpawnPassengers();
            tokenManager.SpawnPowerUps();
            hasSpawned = true;
        }
    }


    void StartFade()
    {
        fadeTimer += Time.deltaTime;
        float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
        fadeToBlackImage.alpha = alpha;
        if (fadeToBlackImage.alpha >= 1f)
        {
            StopPlayerMovement();
            shouldFadeIn = false;
        }
    }

    void StopPlayerMovement()
    {
        exitButton.SetActive(true);
        retryButton.SetActive(false);
        playerManager.playerData.TakeDamage(1000);
        Invoke("DisplayEndScreen", 3.2f);
    }

    void DisplayEndScreen()
    {
        fadeToBlackImage.gameObject.SetActive(false);
        playerData.hasPlayedTutorial = true;
    }

    private void Update()
    {
        if(!hasSpawned)
        {
            SpawnTutorialPassenger();
        }
   

        if (scoreManager.missionsCompleted == 1)
        {
            if (shouldFadeIn)
            {
                Invoke("StartFade", 4f);

                missionManager.enabled = false;

                for (int i = missionManager.passengers.Count - 1; i >= 0; i--)
                {
                    GameObject passenger = missionManager.passengers[i];
                    missionManager.passengers.RemoveAt(i);
                    Destroy(passenger);
                }
            }
        }
        else
        {
            Debug.Log("Waiting for player to complete the mission");
        }

    }
}
