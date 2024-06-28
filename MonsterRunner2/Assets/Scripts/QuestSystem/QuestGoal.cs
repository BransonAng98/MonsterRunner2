using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public string goaltype;
    public bool DestinationReached;
    private Transform destination;
    [SerializeField] public float survivalTime;  // Time to survive in seconds
    [SerializeField] private bool playerIsAlive; // To track player's status
    [SerializeField] public int requiredKillAmount;
    [SerializeField] public int currentAmount;
    public PlayerDataSO playerDataSO;
    public void EnemyKilled()
    {
        if (goaltype == "Kill")
        {
            if(playerDataSO.hasPlayedTutorial == true)
            {
                int[] killCount = { 4, 6, 8 };
                requiredKillAmount = killCount[UnityEngine.Random.Range(0, killCount.Length)];
            }
            else
            {
                int[] killCount = {2};
                requiredKillAmount = killCount[UnityEngine.Random.Range(0, killCount.Length)];
            }
           
           
            Debug.Log("EnemyKilled function called. Required Kill Amount: " + requiredKillAmount);
        }

    }

    public void ReachDestination()
    {
        if (goaltype == "Reach")
        {   
            
            DestinationReached = true;
        }

    }

    public void SurviveWave()
    {
        if (goaltype == "Survive")
        {

            if(playerDataSO.hasPlayedTutorial == true)
            {
                float[] possibleTimes = { 30f, 60f, 60f };
                survivalTime = possibleTimes[UnityEngine.Random.Range(0, possibleTimes.Length)];
                playerIsAlive = true;  // Assume player is alive initially
            }

            if (playerDataSO.hasPlayedTutorial == false)
            {
                float[] possibleTimes = {20f};
                survivalTime = possibleTimes[UnityEngine.Random.Range(0, possibleTimes.Length)];
                playerIsAlive = true;  // Assume player is alive initially
            }

        }
    }

    public void ChooseRandomGoal()
    {
        string[] validObjectiveTypes = { "Kill", "Reach", "Survive" };

        // Randomly select one of the valid objective types
        goaltype = validObjectiveTypes[UnityEngine.Random.Range(0, validObjectiveTypes.Length)];
    }
}