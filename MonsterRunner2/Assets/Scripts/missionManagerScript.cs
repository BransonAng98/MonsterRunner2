using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManagerScript : MonoBehaviour
{
   
    public GameObject player; // Reference to the player GameObject
    public GameObject passengerPrefab;
    public Transform passengerLocation;
   

    public ObjectiveIndicator objectiveIndicator;

    // List to hold all objects under the "building" layer
    public List<GameObject> buildingObjectsList = new List<GameObject>();
    [SerializeField] public int passengerCount;

    [SerializeField] private float survivalTime;

    [SerializeField] private int numberOfPassengersToSpawn = 1; // Number of passengers to spawn

    // assignttoQuest
    public QuestGiver questgiverEntity;
    public QuestDialogueManager questDialogue;
    public DemoPlayer demoPlayer;
    public ScoreManagerScript scoreManager;
    public missionManagerScript missionManager;
    public List<GameObject> buildingObjects;

    public PlayerDataSO playerInfoData;
    public EnemySpawner enemySpawnerScript;

    public List<Transform> SpawnLocations;

    // List to hold references to instantiated passengers
    public List<GameObject> passengers = new List<GameObject>();

    

    void Start()
    {
        CollectBuildingObjects();
        SpawnPassengers();
        objectiveIndicator = player.GetComponentInChildren<ObjectiveIndicator>();
    }

    public void SpawnPassengers()
    {
        if (SpawnLocations.Count == 0)
        {
            Debug.LogWarning("No spawn locations available.");
            return;
        }

        for (int i = 0; i < numberOfPassengersToSpawn; i++)
        {
            Transform randomSpawnLocation = SpawnLocations[Random.Range(0, SpawnLocations.Count)];
            GameObject newPassenger = Instantiate(passengerPrefab, randomSpawnLocation.position, randomSpawnLocation.rotation);
            AssignPassengerProperties(newPassenger);
            passengers.Add(newPassenger);
            passengerLocation = newPassenger.transform;
            objectiveIndicator.UpdateObjective(0, passengerLocation); // State 0 for passenger
        }
    }

    void CollectBuildingObjects()
    {
        int buildingLayer = LayerMask.NameToLayer("Buildings");
        if (buildingLayer == -1)
        {
            Debug.LogError("Layer 'building' not found. Please check the layer name.");
            return;
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == buildingLayer)
            {
                buildingObjectsList.Add(obj);
            }
        }

        Debug.Log($"Collected {buildingObjectsList.Count} objects under the 'building' layer.");
    }

    public void DestroyOtherPassengers(GameObject currentPassenger)
    {
        objectiveIndicator.UpdateObjective(3, passengerLocation);
        for (int i = passengers.Count - 1; i >= 0; i--)
        {
            if (passengers[i] != currentPassenger)
            {
                Destroy(passengers[i]);
                passengers.RemoveAt(i);
            }
        }
    }

    private void AssignPassengerProperties(GameObject passengerEntity)
    {
        PassengerController passengerScript = passengerEntity.GetComponent<PassengerController>();

        if (passengerScript != null)
        {
            passengerScript.scoreManager = scoreManager;
            passengerScript.missionmanager = missionManager;
            passengerScript.questgiverScript = questgiverEntity;
        }
    }
}