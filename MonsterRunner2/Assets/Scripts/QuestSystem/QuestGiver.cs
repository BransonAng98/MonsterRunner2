using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public QuestGoal questGoalScript;
    public DemoPlayer player;
    public GameObject destination;
    public GameObject redDot;

    [SerializeField] public int enemykilled;
    [SerializeField] public int currentenemykilled;
    
    public float distancetoDestination;

    public float survivaltime;
    public float questCooldownTime; // Cooldown duration between quests
    public float initialIntroDelay; // Initial delay before showing intro dialogue
    public float initialMissionDelay; // Delay after intro before starting the first mission

    public QuestDialogueManager questDialogue;
    public ScoreManagerScript scoreManager;
    public missionManagerScript missionManager;
    public List<GameObject> buildingObjects;
    public GameMenuManager menuManager;

    public PlayerDataSO playerInfoData;
    [SerializeField]public bool questCompleted; // Add this variable to track quest completion
    public EnemySpawner enemySpawnerScript;

    [SerializeField] public bool countdownStart;
    [SerializeField] private bool isOnCooldown;
    public bool isTutorial = true;
    public bool isInProgress = false;
    public bool gameStarted;

    public DetectionBar dectectionBar;
    public Audiomanager audiomanagerScript; 


    private void Start()
    {
        quest.goal.playerDataSO = playerInfoData;
        quest.goal.goaltype = "None";
        StartCoroutine(StartGameSequence());
        questCompleted = false;
        countdownStart = false;
        //quest.goal.ChooseRandomGoal();
        gameStarted = true;
        quest.enemyspawnerScript = enemySpawnerScript;
        buildingObjects = missionManager.buildingObjectsList;

        GetDestination();
    }

    private IEnumerator StartGameSequence()
    {
        yield return new WaitForSeconds(initialIntroDelay);
        PrintIntroDialogue();
    }

    private void Update()
    {
        UpdateRedDot();
        if (destination != null)
        {
            distancetoDestination = Vector3.Distance(player.transform.position, destination.transform.position);
            // Now distancetoDestination holds the distance between player and destination
        }

        if (countdownStart == true)
        {
            ReduceSurvivalTime();
        }
        
        switch (quest.goal.goaltype)
        {
            case "Kill":
                dectectionBar.isIncreasing = false;
                UpdateKillGoal();
                break;
            case "Survive":
                dectectionBar.isIncreasing = false;
                UpdateSurviveGoal();
                break;
            case "Reach":
                dectectionBar.isIncreasing = false;
                UpdateReachGoal();
                break;
            case "None":
            default:
                dectectionBar.isIncreasing = true;
                // Handle any other objective types if needed
                break;
        }
    }

    private void UpdateKillGoal()
    {
        if (currentenemykilled >= enemykilled & !questCompleted)
        {
            CompleteQuest();
            Debug.Log("Objective Completed!");
        }
    }

    private void UpdateSurviveGoal()
    {
        if (survivaltime <= 0 && !player.isDead & !questCompleted)
        {
            CompleteQuest();
        }
    }

    private void UpdateReachGoal()
    {
        float distanceToDestination = Vector3.Distance(transform.position, destination.transform.position);
        if (distanceToDestination < 20f & !questCompleted) // Adjust the threshold as needed
        {
            CompleteQuest();
        }
    }



    private void ReduceSurvivalTime()
    {
        if (survivaltime > 0)
        {
            survivaltime -= Time.deltaTime;
           
        }
    }

    public void CompleteQuest()
    {
        audiomanagerScript.StopPoliceSiren(3f);
        Debug.Log("QuestCompleted");
        player.contractCompleteVFX.SetActive(true);
        questCompleted = true;
        GetDestination();
        scoreManager.goldEarned += quest.goldReward;
        scoreManager.missionsCompleted++;
        playerInfoData.moneyAccumulatedInGame += scoreManager.goldEarned;
        playerInfoData.contractCompleted++;
        if(menuManager.sceneID != 1)
        {
            missionManager.SpawnPassengers();
        }
        enemySpawnerScript.DestroyAllEnemies();
        enemySpawnerScript.startSpawning = false;
        RunGoalType("None");

        PrintRewardDialogue();
    }

    public void SpawnEnemies()
    {
        audiomanagerScript.PlayPoliceSiren();
        enemySpawnerScript.startSpawning = true;
    }

    public void GetDestination()
    {
        if (buildingObjects.Count > 0)
        {
            GameObject randomBuilding = buildingObjects[Random.Range(0, buildingObjects.Count)];
            destination = randomBuilding;
            Debug.Log("GetDestination");
        }
    }

    void PrintIntroDialogue()
    {
        questDialogue.TypeIntro();
    }

    void PrintQuestDialogue()
    {
        if (menuManager.sceneID == 2)
        {
            int index = Random.Range(0, questDialogue.questText.Length); //Show quest text when new quest is given
            questDialogue.TypeText(true, index);
        }
        
        if (menuManager.sceneID == 1)
        {
            questDialogue.StartSecondTutorial();
        }
    }

    void PrintRewardDialogue()
    {
        int index = Random.Range(0, questDialogue.rewardText.Length);
        questDialogue.TypeText(false, index); // Show reward text when quest is completed
        isInProgress = false;
        
    }

    public void UpdateRedDot()
    {
        if (isInProgress)
        {
            redDot.SetActive(true);
        }
        else
        {
            redDot.SetActive(false);
        }
    }

    public void RunGoalType(string goaltype)
    {
        PrintQuestDialogue();
        quest.goldRewardAmt();
        switch (goaltype)
        {
            case "Kill":
                quest.goal.EnemyKilled();
                enemykilled = quest.goal.requiredKillAmount;
                isInProgress = true;
                break;

            case "Survive":
                quest.goal.SurviveWave();
                survivaltime = quest.goal.survivalTime;
                isInProgress = true;
                break;

            case "Reach":
                quest.goal.ReachDestination();
                isInProgress = true;
                break;
            case "None":
                ResetQuestValues();
                break;
            default:
                break;
        }
    }

    private void ResetQuestValues()
    {
        survivaltime = 0;
        currentenemykilled = 0;
        enemykilled = 0;
        distancetoDestination = 0;
       
        isInProgress = false;
        countdownStart = false;
        questCompleted = false;
        quest.goal.goaltype = "None";
        Debug.Log("Reset Quest Values");
    }
}



