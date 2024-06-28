using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public string title;
    public float goldReward; 
    
    public QuestGoal goal;
    public EnemySpawner enemyspawnerScript;

    public void Complete()
    {
        isActive = false;
        Debug.Log(title + "was Completed");
    }

    public void goldRewardAmt()
    {
        float[] possibleGoldAmounts;

        switch (enemyspawnerScript.threatlvl)
        {
            case 1:
                possibleGoldAmounts = new float[] { 100f, 150f, 175f };
                break;
            case 2:
                possibleGoldAmounts = new float[] { 200f, 250f, 275f };
                break;
            default:
                possibleGoldAmounts = new float[] { 300f, 320f, 350f }; // Default case if needed
                break;
        }

        goldReward = possibleGoldAmounts[Random.Range(0, possibleGoldAmounts.Length)];
    }
}

