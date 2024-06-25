using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class missionManagerScript : MonoBehaviour
{
    public GameObject actualGround;
    public GameObject buildingHolder;
    public GameObject player; // Reference to the player GameObject
    public GameObject passengerPrefab;
    public GameObject destination;
   
    public ObjectiveIndicator objectiveIndicator;

    // List to hold all objects under the "building" layer
    public List<GameObject> buildingObjectsList = new List<GameObject>();
    [SerializeField] public int passengerCount;

    [SerializeField] private float survivalTime;

    public float spawnRadius = 50f; // Distance from the player to spawn the prefab
    public float maxSearchRadius = 500f; // Maximum search radius for finding a walkable position
    public int maxAttempts = 1000; // Maximum attempts to find a non-walkable position
    public int numberOfPassengersToSpawn = 10; // Number of passengers to spawn
    public float minSpacing = 10f; // Minimum spacing between passengers

    // assignttoQuest
    public QuestGiver questgiverEntity;
    public QuestDialogueManager questDialogue;
    public DemoPlayer demoPlayer;
    public ScoreManagerScript scoreManager;
    public missionManagerScript missionManager;
    public List<GameObject> buildingObjects;

    public PlayerDataSO playerInfoData;
    public EnemySpawner enemySpawnerScript;

    // List to hold references to instantiated passengers
    public List<GameObject> passengers = new List<GameObject>();

    void Start()
    {
        CollectBuildingObjects();
        SpawnPassengers();
    }

    public void SpawnPassengers()
    {
        for (int i = 0; i < numberOfPassengersToSpawn; i++)
        {
            Vector3 spawnPosition = Vector3.zero;
            bool foundPosition = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                spawnPosition = GetRandomSpawnPosition();

                if (IsWalkable(spawnPosition) && IsFarEnoughFromOtherPassengers(spawnPosition))
                {
                    foundPosition = true;
                    break;
                }
            }

            if (foundPosition)
            {
                GameObject passenger = Instantiate(passengerPrefab, spawnPosition, Quaternion.identity);
                passengers.Add(passenger);
                AssignPassengerProperties(passenger);
            }
            else
            {
                Debug.LogWarning("Could not find a suitable position to spawn the passenger after max attempts.");
            }
        }
    }

    bool IsFarEnoughFromOtherPassengers(Vector3 position)
    {
        foreach (GameObject passenger in passengers)
        {
            if (Vector3.Distance(position, passenger.transform.position) < minSpacing)
            {
                return false; // Position is too close to an existing passenger
            }
        }
        return true; // Position is far enough from all existing passengers
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

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomPosition = Vector3.zero;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
            randomPosition = player.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPosition, out hit, maxSearchRadius, NavMesh.AllAreas))
            {
                return hit.position; // Found a valid walkable position
            }

            attempts++;
        }

        Debug.LogError("Failed to find a walkable spawn position after " + maxAttempts + " attempts.");
        return player.transform.position; // Default to player's position if no valid position found
    }
    bool IsWalkable(Vector3 position)
    {
        NavMeshHit hit;
        bool isWalkable = NavMesh.SamplePosition(position, out hit, Mathf.Infinity, NavMesh.AllAreas);
        return isWalkable;
    }
}