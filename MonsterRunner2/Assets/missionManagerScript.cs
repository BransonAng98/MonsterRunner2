using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManagerScript : MonoBehaviour
{
    // List to hold all objects under the "building" layer
    public List<GameObject> buildingObjectsList = new List<GameObject>();
    public QuestGiver questgiverEnitity;
    public GameObject destination; 
    

    void Start()
    {
        questgiverEnitity = GameObject.FindGameObjectWithTag("Passenger").GetComponentInChildren<QuestGiver>();
    }

    public void FindBuildingObjects()
    {
        // Find all objects with the layer named "building"
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Buildings"))
            {
                // Add object to the list if it's under the "building" layer
                buildingObjectsList.Add(obj);
            }
        }

        // Print the count of building objects found
        Debug.Log("Total building objects found: " + buildingObjectsList.Count);
    }

    public void GetDestination()
    {
        if(questgiverEnitity != null)
        {
            destination = questgiverEnitity.destination;
        }
    }
}
