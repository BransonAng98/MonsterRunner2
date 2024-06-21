using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class SideObjective
{
    public bool isActive;
    public string title;
    public float goldReward;

    public SideObjectiveGoal goal;
    public EnemySpawner enemySpawnerScript;

    public void CompleteSideObjective()
    {
        isActive = false;
        Debug.Log(title + "was Completed");
    }

    public void goldRewardAmt()
    {
        float[] possibleGoldAmounts;

        switch (enemySpawnerScript.threatlvl)
        {
            case 1:
                possibleGoldAmounts = new float[] { 20f, 30f, 40f };
                break;
            case 2:
                possibleGoldAmounts = new float[] { 60f, 70f, 80f };
                break;
            default:
                possibleGoldAmounts = new float[] { 0f }; // Default case if needed
                break;
        }

        goldReward = possibleGoldAmounts[Random.Range(0, possibleGoldAmounts.Length)];
    }

}
