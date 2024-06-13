using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goaltype;
    //public int requiredAmount;
    //public int currentAmount;
    public bool DestinationReached;
    public Transform destination;
    [SerializeField]public float survivalTime;  // Time to survive in seconds
    [SerializeField]private bool playerIsAlive; // To track player's status
    
    //public bool isReached()

    //{
    //    return (currentAmount >= requiredAmount);
    //}

    public void EnemyKilled()
    {
        if(goaltype == GoalType.Kill)
        {
            //currentAmount++;
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
            Debug.Log("Survival Time");
            float[] possibleTimes = { 30f, 60f, 90f };
            survivalTime = possibleTimes[Random.Range(0, possibleTimes.Length)];
            playerIsAlive = true;  // Assume player is alive initially
          
        }
    }
}

public enum GoalType
{
    Kill,
    Deliver,
    Reach,
    Survive
}
