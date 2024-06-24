using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goaltype;
    public bool DestinationReached;
    public Transform destination;
    [SerializeField]public float survivalTime;  // Time to survive in seconds
    [SerializeField]private bool playerIsAlive; // To track player's status
    [SerializeField] public int requiredKillAmount;
    [SerializeField] public int currentAmount;

    public void EnemyKilled()
    {
        if (goaltype == GoalType.Kill)
        {
            int[] killCount = { 2, 3, 4 };
            requiredKillAmount = killCount[UnityEngine.Random.Range(0, killCount.Length)];
            Debug.Log("EnemyKilled function called. Required Kill Amount: " + requiredKillAmount);
        }

    }

    public void ReachDestination()
    {
        if (goaltype == GoalType.Reach)
        {
            DestinationReached = true;
        }

    }

    public void SurviveWave()
    {
        if(goaltype == GoalType.Survive)
        {
            
            float[] possibleTimes = { 30f, 60f, 60f };
            survivalTime = possibleTimes[UnityEngine.Random.Range(0, possibleTimes.Length)];
            playerIsAlive = true;  // Assume player is alive initially
          
        }
    }

    public void ChooseRandomGoal()
    {
        GoalType[] goalTypes = (GoalType[])Enum.GetValues(typeof(GoalType));
        // Exclude the 'none' type
        GoalType[] validObjectiveTypes = Array.FindAll(goalTypes, type => type != GoalType.none);;

        // Randomly select one of the valid objective types
        goaltype = validObjectiveTypes[UnityEngine.Random.Range(0, validObjectiveTypes.Length)];
    }
    public enum GoalType
    {
        none,
        Kill,
        Reach,
        Survive
    }
}

  
