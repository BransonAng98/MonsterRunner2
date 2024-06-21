using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SideObjectiveQuestGiver : MonoBehaviour
{
    public SideObjective SideObjective;
    [SerializeField] private int enemykilled;
    [SerializeField] public int currentenemykilled;
    [SerializeField] private float goldtobeEarned;
    [SerializeField] private float currentGoldAmt;
    public ScoreManagerScript scoreManager;
    public PlayerDataSO playerInfoData;

    private bool questCompleted = false;
    public EnemySpawner enemySpawnerScript;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawnerScript = SideObjective.enemySpawnerScript;
        SideObjective.goal.ChooseRandomObjectiveType();
        RunObjectiveFunction(SideObjective.goal.objectiveType);
        SideObjective.goldRewardAmt();
    }

    // Update is called once per frame
    void Update()
    {
        currentGoldAmt = scoreManager.goldEarned;

        if (!questCompleted)
        {
            if (SideObjective.goal.objectiveType == SideObjectiveGoal.ObjectiveType.Kill)
            {
                scoreManager.sideobjectiveText.text = $"{currentenemykilled}/{enemykilled} killed";

                if (currentenemykilled == enemykilled)
                {
                    CompleteQuest();
                    Debug.Log("Objective Completed!");
                }
            }
            else if (SideObjective.goal.objectiveType == SideObjectiveGoal.ObjectiveType.EarnGold)
            {
                scoreManager.sideobjectiveText.text = $"{currentGoldAmt}/{goldtobeEarned} earned";

                if (currentGoldAmt >= goldtobeEarned)
                {
                    CompleteQuest();
                    Debug.Log("Objective Completed!");
                }
            }
        }
    }

    public void CompleteQuest()
    {
        // Add the reward to the player's gold
        scoreManager.goldEarned += SideObjective.goldReward;
        playerInfoData.money += SideObjective.goldReward;
        SideObjective.CompleteSideObjective();

        // Mark the quest as completed
        questCompleted = true;
    }

    void RunObjectiveFunction(SideObjectiveGoal.ObjectiveType objectiveType)
    {
        switch (objectiveType)
        {
            case SideObjectiveGoal.ObjectiveType.none:
                // Do nothing for 'none' type
                break;
            case SideObjectiveGoal.ObjectiveType.Kill:
                // Run the enemy kill function
                SideObjective.goal.EnemyKilled();
                enemykilled = SideObjective.goal.requiredKillAmount;
                break;
            case SideObjectiveGoal.ObjectiveType.EarnGold:
                // Run the earn gold function
                SideObjective.goal.ReachDestination();
                goldtobeEarned = SideObjective.goal.goldtobeEarnedAmt;
                break;
            default:
                // Handle any other objective types if needed
                break;
        }
    }
}