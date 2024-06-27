using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SideObjectiveQuestGiver : MonoBehaviour
{
    public SideObjective SideObjective;
    public int enemykilled;
    [SerializeField] public int currentenemykilled;
    public float goldtobeEarned;
    [SerializeField]public float currentGoldAmt;
    public ScoreManagerScript scoreManager;
    public PlayerDataSO playerInfoData;
    public EnemySpawner enemySpawnerScript;

    private bool questCompleted = false;

    [SerializeField]private bool hasRunKillObjective = false;
    [SerializeField]private bool hasRunGoldObjective = false;

    void Start()
    {
        SideObjective.enemySpawnerScript = enemySpawnerScript;
        GetNewSideObjective();
        RunObjectiveFunction(SideObjective.goal.objectiveType);
        
    }

    void Update()
    {
        currentGoldAmt = scoreManager.goldEarned;

        switch (SideObjective.goal.objectiveType)
        {
            case SideObjectiveGoal.ObjectiveType.Kill:

                if (currentenemykilled >= enemykilled)
                {
                    CompleteQuest();
                    Debug.Log("Objective Completed!");
                }

                hasRunKillObjective = true;
                scoreManager.sideobjectiveText.text = $"{currentenemykilled}/{enemykilled} killed";
                if (!hasRunKillObjective)
                {
                    RunObjectiveFunction(SideObjectiveGoal.ObjectiveType.Kill);
                    scoreManager.sideobjectiveText.text = $"{currentenemykilled}/{enemykilled} killed";

                }
                break;
            case SideObjectiveGoal.ObjectiveType.EarnGold:
                if (currentGoldAmt >= goldtobeEarned)
                {
                    CompleteQuest();
                    Debug.Log("Objective Completed!");
                }

                hasRunGoldObjective = true;
                scoreManager.sideobjectiveText.text = $"{currentGoldAmt}/{goldtobeEarned} earned";
                if (!hasRunGoldObjective)
                {
                    RunObjectiveFunction(SideObjectiveGoal.ObjectiveType.EarnGold);
                    

                    
                }
                break;
            case SideObjectiveGoal.ObjectiveType.none:
                RunObjectiveFunction(SideObjectiveGoal.ObjectiveType.none);
                break;
        }
    }

    public void CompleteQuest()
    {
        // Add the reward to the player's gold
        scoreManager.goldEarned += SideObjective.goldReward;
        playerInfoData.money += SideObjective.goldReward;
        SideObjective.CompleteSideObjective();
       
        SideObjective.goal.objectiveType = SideObjectiveGoal.ObjectiveType.none;
        Debug.Log("No Side Objective For Now");
        // Mark the quest as completed
        questCompleted = true;
    }

    public void GetNewSideObjective()
    {
        SideObjective.goal.ChooseRandomObjectiveType();
        SideObjective.goldRewardAmt();
        hasRunKillObjective = false;
        hasRunGoldObjective = false;
        Debug.Log("Choose a new side objective");
    }

    void ResetSideObjectiveValues()
    {
       
        currentenemykilled = 0;
        currentGoldAmt = 0;
        questCompleted = false;
        Debug.Log("ResetSideObjectiveValues");
    }

    void RunObjectiveFunction(SideObjectiveGoal.ObjectiveType objectiveType)
    {
        switch (objectiveType)
        {
            case SideObjectiveGoal.ObjectiveType.Kill:
                SideObjective.goal.EnemyKilled();
                enemykilled = SideObjective.goal.requiredKillAmount;
                break;
            case SideObjectiveGoal.ObjectiveType.EarnGold:
                SideObjective.goal.EarnGold();
                goldtobeEarned = SideObjective.goal.goldtobeEarnedAmt;
                break;
            case SideObjectiveGoal.ObjectiveType.none:
                ResetSideObjectiveValues();
                GetNewSideObjective();
                break;
            default:
                break;
        }
    }
}