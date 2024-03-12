using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goaltype;
    public int requiredAmount;
    public int currentAmount;
    public bool DestinationReached;
    public Transform destination;
    public bool isReached()

    {
        return (currentAmount >= requiredAmount);
    }

    public void EnemyKilled()
    {
        if(goaltype == GoalType.Kill)
        {
            currentAmount++;
        }
       
    }

    public void ReachDestination()
    {
        if (goaltype == GoalType.Reach)
        {
            DestinationReached = true;
        }

    }

}

public enum GoalType
{
    Kill,
    Deliver,
    Reach
}
