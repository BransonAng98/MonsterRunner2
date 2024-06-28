using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreManagerScript : MonoBehaviour
{
    public float goldEarned;
    public int missionsCompleted;
    public float timeSurvived;
    public float stopwatchTime;

    public int currentEnemiesKilled;
    public int totalEnemiesKilled;

    public TextMeshProUGUI activeCounter;
    public TextMeshProUGUI goldamtCounter;
    public TextMeshProUGUI sideobjectiveText;
    public TextMeshProUGUI questDetail;
    public TextMeshProUGUI questReward;
    public TextMeshProUGUI sideobjectiveDetail;
    public TextMeshProUGUI sideobjectiveReward;

    public QuestGiver questgiverScript;
    public SideObjectiveQuestGiver SideObjectiveQuestGiverScript;
    public EnemySpawner enemySpawnerScript;

    public GameObject timeIcon;
    public GameObject killIcon;

    private bool killQuest;
    public float distancetoTarget;
    private int lastMissionCount = 0;

    void Start()
    {
        goldEarned = 0f;
        missionsCompleted = 0;
        timeSurvived = 0f;
        stopwatchTime = 0f;
    }

    void Update()
    {
        stopwatchTime += Time.deltaTime;
        goldamtCounter.text = goldEarned + "";
        AssignObjectiveText();
        AssignSideObjectiveText();
        UpdateObjIcon();
        // Check if missions completed has increased
        if (missionsCompleted > lastMissionCount)
        {
            // Increase threat level for every 2 missions completed
            if (missionsCompleted % 2 == 0)
            {
                enemySpawnerScript.threatlvl++;
                Debug.Log("Threat level increased to: " + enemySpawnerScript.threatlvl);
            }

            lastMissionCount = missionsCompleted;
        }


    }

    void UpdateObjIcon()
    {
        if (killQuest)
        {
            killIcon.SetActive(true);
            timeIcon.SetActive(false);
        }
        else
        {
            killIcon.SetActive(false);
            timeIcon.SetActive(true);
        }
    }

    public void AssignObjectiveText()
    {
        switch (questgiverScript.quest.goal.goaltype)
        {
            case "Kill":
                questDetail.text = "Take out " + totalEnemiesKilled + " enemies and keep the cops distracted for us";
                currentEnemiesKilled = questgiverScript.currentenemykilled;
                totalEnemiesKilled = questgiverScript.enemykilled;
                activeCounter.text = "Kill " + (totalEnemiesKilled - currentEnemiesKilled) + " Police";
                questReward.text = "Reward: " + questgiverScript.quest.goldReward + "";
                killQuest = true;
                break;
            case "Reach":
                distancetoTarget = questgiverScript.distancetoDestination;
                int distanceInt = Mathf.FloorToInt(distancetoTarget);
                questDetail.text = "Take us to the safe house. It's  " + distancetoTarget + " m away. Hurry!";
                activeCounter.text = distanceInt + "m";
                questReward.text = "Reward: " + questgiverScript.quest.goldReward + "";
                break;

            case "Survive":
                timeSurvived = questgiverScript.survivaltime;

                // Calculate minutes and seconds
                float minutes = Mathf.FloorToInt(timeSurvived / 60);
                float seconds = Mathf.FloorToInt(timeSurvived % 60);

                // Update the active counter display
                string timeHolder;
                timeHolder = string.Format("{0:00}:{1:00}", minutes, seconds);
                activeCounter.text = "Survive for " + timeHolder;
                questDetail.text = "Keep the cops busy for " + timeHolder + ". While you do that, we will rob the bank";
                questReward.text = "Reward: " + questgiverScript.quest.goldReward + "";
                killQuest = false;
                break;
            case "None":
                activeCounter.text = " Follow the Arrow!";
                questDetail.text = "No current contract. Follow the arrow to collect a passenger.";
                questReward.text = "Reward: 0";
                break;

        }
    }

    public void AssignSideObjectiveText()
    {
        switch (SideObjectiveQuestGiverScript.SideObjective.goal.objectiveType)
        {
            case SideObjectiveGoal.ObjectiveType.Kill:
                sideobjectiveDetail.text = "The boss says he will give you a bonus if you destroy " + SideObjectiveQuestGiverScript.enemykilled + " of their cars.";
                sideobjectiveReward.text = "Reward: " + SideObjectiveQuestGiverScript.SideObjective.goldReward + "";
                break;

            case SideObjectiveGoal.ObjectiveType.EarnGold:
                sideobjectiveDetail.text = "The boss is in a generous mood. If you earn " + SideObjectiveQuestGiverScript.SideObjective.goal.goldtobeEarnedAmt + " before you die, we'll give you a bonus";
                sideobjectiveReward.text = "Reward: " + SideObjectiveQuestGiverScript.SideObjective.goldReward + "";
                break;
            case SideObjectiveGoal.ObjectiveType.none:
                sideobjectiveDetail.text = "No side objectives for now.";
                sideobjectiveReward.text = "Reward:0 ";
                break;
        }
    }

}
