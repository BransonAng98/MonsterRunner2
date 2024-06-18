using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SideObjectiveGoal
{
    public ObjectiveType objectivetype;
    // Start is called before the first frame update
    public float distanceTravelled;
    public void EnemyKilled()
    {
        if (objectivetype == ObjectiveType.Kill)
        {
            //currentAmount++;
        }

    }

    public void ReachDestination()
    {
        if (objectivetype == ObjectiveType.DistanceTravelled)
        {
            
        }

    }

    public enum ObjectiveType
    {
        Kill,
        DistanceTravelled
    }

}
