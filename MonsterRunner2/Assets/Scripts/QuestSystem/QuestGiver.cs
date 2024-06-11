using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public DemoPlayer player;
    public GameObject destination;
   
    public float survivaltime; 

    public QuestDialogueManager questDialogue;
    public ScoreManagerScript scoreManager;

    public missionManagerScript missionManager; 
    public List<GameObject> buildingObjects;


    private void Start()
    {
        buildingObjects = missionManager.buildingObjectsList;
        //GetDestination();
        quest.goal.SurviveWave();
        quest.goldRewardAmt();
        GetSurvivalTime();
    }

    private void Update()
    {
        ReduceSurvivalTime();

        if(survivaltime == 0 & player.isDead == false)
        {
            quest.Complete();
            GetNewSurvivalTime();
        }
    }
    public void AcceptQuest()
    {
        // give quest to player
        quest.isActive = true;
        player.quest = quest;
        int index = Random.Range(0, 1);
        questDialogue.TypeText(true , index);
    }

    private void ReduceSurvivalTime()
    {
        if (survivaltime > 0)
        {
            survivaltime -= Time.deltaTime;
            if (survivaltime <= 0)
            {
                survivaltime = 0;
             
            }
        }
    }

    public void GetNewSurvivalTime()
    {
        scoreManager.goldEarned += quest.goldReward;
        scoreManager.missionsCompleted++;
        quest.goldRewardAmt();
        quest.goal.SurviveWave();
        GetSurvivalTime();
    }
    public void GetDestination()
    {
        if (buildingObjects.Count > 0)
        {
            GameObject randomBuilding = buildingObjects[Random.Range(0, buildingObjects.Count)];
         
            // Set the randomly chosen transform as the destination
            destination = randomBuilding;
            //missionManager.GetDestination();
        }
    }

    public void GetSurvivalTime()
    {
       survivaltime = quest.goal.survivalTime;
    }
}


