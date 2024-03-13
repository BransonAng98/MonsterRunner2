using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public DemoPlayer player;
    public GameObject destination;
    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public Text rewardText;
    public missionManagerScript missionManager; public List<GameObject> buildingObjects;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<DemoPlayer>();
        missionManager = GameObject.Find("MissionManager").GetComponent<missionManagerScript>();
        buildingObjects = GameObject.FindObjectOfType<missionManagerScript>().buildingObjectsList;
        GetDestination();
      
    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        rewardText.text = quest.repairReward;
    }

    public void AcceptQuest()
    {
        // give quest to player
        //questWindow.SetActive(false);
        quest.isActive = true;
        player.quest = quest;
    }

    public void GetDestination()
    {
        if (buildingObjects.Count > 0)
        {
            GameObject randomBuilding = buildingObjects[Random.Range(0, buildingObjects.Count)];
         
            // Set the randomly chosen transform as the destination
            destination = randomBuilding;
            missionManager.GetDestination();
        }
    }
}
