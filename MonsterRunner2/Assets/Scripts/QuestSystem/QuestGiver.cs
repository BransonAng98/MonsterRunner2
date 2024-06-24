using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public QuestGoal questGoalScript;
    public DemoPlayer player;
    public GameObject destination;

    [SerializeField] private int enemykilled;
    [SerializeField] public int currentenemykilled;

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

    private bool questcompleted = false;

    private void Start()
    {
        quest.goal.ChooseRandomGoal();
        gameStarted = true;
        quest.enemyspawnerScript = enemySpawnerScript;
        buildingObjects = missionManager.buildingObjectsList;
        //StartCoroutine(StartGameSequence());
        RunGoalType(quest.goal.goaltype);
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

        
    }

    private void Update()
    {
        switch (quest.goal.goaltype)
        {
            case QuestGoal.GoalType.Kill:
                UpdateKillGoal();
                break;
            case QuestGoal.GoalType.Survive:
                UpdateSurviveGoal();
                break;
            case QuestGoal.GoalType.Reach:
                UpdateReachGoal();
                break;
            case QuestGoal.GoalType.none:
            default:
                // Handle any other objective types if needed
                break;
        }
    }

    private void UpdateKillGoal()
    {
        if (currentenemykilled == enemykilled)
        {
            CompleteQuest();
            Debug.Log("Objective Completed!");
        }
    }

    private void UpdateSurviveGoal()
    {
        if (!isOnCooldown)
        {
            ReduceSurvivalTime();
            if (survivaltime <= 0 && !player.isDead)
            {
                CompleteQuest();
            }
        }
    }

    private void UpdateReachGoal()
    {
        float distanceToDestination = Vector3.Distance(transform.position, destination.transform.position);
        if (distanceToDestination < 0.5f) // Adjust the threshold as needed
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

    private IEnumerator CooldownBeforeNextMission()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(questCooldownTime);
       
        isOnCooldown = false;
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

    void RunGoalType(QuestGoal.GoalType goaltype)
    {
        switch (goaltype)
        {
            case QuestGoal.GoalType.none:
              
                break;
            case QuestGoal.GoalType.Kill:
            
                quest.goal.EnemyKilled();
                enemykilled = quest.goal.requiredKillAmount;
                break;
            case QuestGoal.GoalType.Survive:
      
                quest.goal.SurviveWave();
                survivaltime = quest.goal.survivalTime;
                break;
            case QuestGoal.GoalType.Reach:
                GetDestination();
                quest.goal.ReachDestination();
              
                break;
            default:
                // Handle any other objective types if needed
                break;
        }
    }
}



