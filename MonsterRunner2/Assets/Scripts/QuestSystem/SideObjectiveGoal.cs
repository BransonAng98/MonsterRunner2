using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SideObjectiveGoal
{ 
    public ObjectiveType objectiveType;
    [SerializeField] public int requiredKillAmount;
    [SerializeField] public int currentAmount;
    [SerializeField] public float goldtobeEarnedAmt;

    public void NoQuest()
    {

    }
    public void EnemyKilled()
    {
        if (objectiveType == ObjectiveType.Kill)
        {
            int[] killCount = { 2, 3, 4 };
            requiredKillAmount = killCount[UnityEngine.Random.Range(0, killCount.Length)];
            Debug.Log("EnemyKilled function called. Required Kill Amount: " + requiredKillAmount);
        }
    }

    public void EarnGold()
    {
        if (objectiveType == ObjectiveType.EarnGold)
        {
            float[] goldtobeEarned = { 400f, 500f, 600f };
            goldtobeEarnedAmt = goldtobeEarned[UnityEngine.Random.Range(0, goldtobeEarned.Length)];
            Debug.Log("ReachDestination function called. Distance Travelled: " + goldtobeEarnedAmt);
        }
    }

   

    public void ChooseRandomObjectiveType()
    {
        // Get all values of ObjectiveType
        ObjectiveType[] objectiveTypes = (ObjectiveType[])Enum.GetValues(typeof(ObjectiveType));

        // Exclude the 'none' type
        ObjectiveType[] validObjectiveTypes = Array.FindAll(objectiveTypes, type => type != ObjectiveType.none);

        // Randomly select one of the valid objective types
        objectiveType = validObjectiveTypes[UnityEngine.Random.Range(0, validObjectiveTypes.Length)];
    }


    public enum ObjectiveType
    {
        none,
        Kill,
        EarnGold
    }
}