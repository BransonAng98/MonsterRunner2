using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public DemoPlayer player;
    public GameObject destination;

    public float survivaltime;
    public float questCooldownTime; // Cooldown duration between quests
    public float initialIntroDelay; // Initial delay before showing intro dialogue
    public float initialMissionDelay; // Delay after intro before starting the first mission

    public QuestDialogueManager questDialogue;
    public ScoreManagerScript scoreManager;
    public missionManagerScript missionManager;
    public List<GameObject> buildingObjects;

    public PlayerDataSO playerInfoData;

    public EnemySpawner enemySpawnerScript;

    private bool isOnCooldown = false;
    private bool isFirstQuest = true;
    public bool gameStarted;

    private void Start()
    {
        gameStarted = false;
        buildingObjects = missionManager.buildingObjectsList;
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        yield return new WaitForSeconds(initialIntroDelay);

        // Display intro dialogue after initial delay if it's the first quest
        if (isFirstQuest)
        {
            PrintIntroDialogue();
            isFirstQuest = false; // Set isFirstQuest to false after displaying intro
            yield return new WaitForSeconds(initialMissionDelay);
        }

        gameStarted = true;

        StartNewQuest();
    }

    private void Update()
    {
        // Only reduce survival time if it's not on cooldown
        if (!isOnCooldown)
        {
            ReduceSurvivalTime();
        }
    }

    private void ReduceSurvivalTime()
    {
        if (survivaltime > 0)
        {
            survivaltime -= Time.deltaTime;
            if (survivaltime <= 0)
            {
                survivaltime = 0;
                if (!player.isDead)
                {
                    quest.Complete();
                    CompleteQuest();

                    StartCoroutine(CooldownBeforeNextMission());
                }
            }
        }
    }

    private IEnumerator CooldownBeforeNextMission()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(questCooldownTime);
        StartNewQuest();
        isOnCooldown = false;
    }

    private void StartNewQuest()
    {
        quest.goal.SurviveWave();
        quest.goldRewardAmt();
        survivaltime = quest.goal.survivalTime;
        enemySpawnerScript.UpdateEnemiesForThreatLevel();
        PrintQuestDialogue();
    }

    public void CompleteQuest()
    {
        scoreManager.goldEarned += quest.goldReward;
        scoreManager.missionsCompleted++;
        playerInfoData.money += scoreManager.goldEarned;

        PrintRewardDialogue();
    }

    public void GetDestination()
    {
        if (buildingObjects.Count > 0)
        {
            GameObject randomBuilding = buildingObjects[Random.Range(0, buildingObjects.Count)];
            destination = randomBuilding;
        }
    }

    void PrintIntroDialogue()
    {
        int introIndex = Random.Range(0, questDialogue.introText.Length);
        questDialogue.TypeIntro(introIndex);
    }

    void PrintQuestDialogue()
    {
        int index = Random.Range(0, questDialogue.questText.Length); //Show quest text when new quest is given
        questDialogue.TypeText(true, index);
    }

    void PrintRewardDialogue()
    {
        int index = Random.Range(0, questDialogue.rewardText.Length);
        questDialogue.TypeText(false, index); // Show reward text when quest is completed
    }
}

