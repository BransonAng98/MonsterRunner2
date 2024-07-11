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
    public DemoPlayer player;

    public ScoreManagerScript scoreManager;
    public missionManagerScript missionManager;
    public QuestDialogueManager questDialogue;
    public AbilityTokenManager tokenManager;
    public EnemySpawner enemyspawnerScript;
    public CanvasGroup fadeToBlackImage; 
    public float fadeDuration = 1f; // Duration of the fade effect

    public GameObject exitButton;
    public GameObject retryButton;
    private float fadeTimer = 0f;

    [SerializeField] private bool shouldFadeIn = true;
    [SerializeField] private bool hasSpawnedPassenger;
    [SerializeField] private bool hasSpawnedToken;

    private void Start()
    {
        fadeToBlackImage.alpha = 0f;
        exitButton.SetActive(false);
    }

    void SpawnAbilityTokens()
    {
        if (questDialogue.isDoneExplaining == true)
        {
            tokenManager.SpawnPowerUps();
            hasSpawnedToken = true;
            //enemyspawnerScript.startSpawning = true;
        }
        
    }

    public void StartFade()
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

    public void TypeCompleteText()
    {
        int index = Random.Range(0, questDialogue.rewardText.Length);
        questDialogue.TypeText(false, index);
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
        if (!hasSpawnedToken)
        {
            SpawnAbilityTokens();
        }

    }
}
