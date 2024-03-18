using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public DemoPlayer player;
    public GameObject destination;

    public QuestDialogueManager questDialogue;

    public missionManagerScript missionManager; public List<GameObject> buildingObjects;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<DemoPlayer>();
        missionManager = GameObject.Find("MissionManager").GetComponent<missionManagerScript>();
        buildingObjects = GameObject.FindObjectOfType<missionManagerScript>().buildingObjectsList;
        questDialogue = GameObject.Find("MissionManager").GetComponent<QuestDialogueManager>();
        
        GetDestination();
    }

    public void AcceptQuest()
    {
        // give quest to player
        quest.isActive = true;
        player.quest = quest;
        int index = Random.Range(0, 1);
        questDialogue.TypeText(true , index);
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
}
