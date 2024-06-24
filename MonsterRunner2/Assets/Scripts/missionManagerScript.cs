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
    public QuestGiver questgiverEntity;
    public ObjectiveIndicator objectiveIndicator;

   
   
   

    // List to hold all objects under the "building" layer
    public List<GameObject> buildingObjectsList = new List<GameObject>();
    [SerializeField] public int passengerCount;

    [SerializeField] private float survivalTime;

    public float spawnRadius = 100f; // Distance from the player to spawn the prefab
    public float maxSearchRadius = 500f; // Maximum search radius for finding a walkable position
    public int maxAttempts = 1000; // Maximum attempts to find a non-walkable position
    public int numberOfPassengersToSpawn = 5; // Number of passengers to spawn
    public float minSpacing = 100f; // Minimum spacing between passengers

    //assignttoQuest
    public QuestDialogueManager questDialogue;
    public DemoPlayer demoPlayer;
    public ScoreManagerScript scoreManager;
    public missionManagerScript missionManager;
    public List<GameObject> buildingObjects;

    public PlayerDataSO playerInfoData;

    public EnemySpawner enemySpawnerScript;

    void Start()
    {
        SpawnPassengers();
    }

    public void SpawnPassengers()
    {
        for (int i = 0; i < numberOfPassengersToSpawn; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            if (IsWalkable(spawnPosition))
            {
                AssignPassengerProperties(passengerPrefab);
                Instantiate(passengerPrefab, spawnPosition, Quaternion.identity);
              
            }
            else
            {
                Debug.LogWarning("Could not find a non-walkable position to spawn the passenger.");
            }
        }
    }

    private void AssignPassengerProperties(GameObject passengerEntity)
    {
        QuestGiver questgiverScript = passengerEntity.GetComponent<QuestGiver>();
        PassengerController passengerScript = passengerEntity.GetComponent<PassengerController>();

        if(questgiverScript != null)
        {
            questgiverScript.player = demoPlayer;
            questgiverScript.questDialogue = questDialogue;
            questgiverScript.scoreManager = scoreManager;
            questgiverScript.missionManager = missionManager;
            questgiverScript.playerInfoData = playerInfoData;
            questgiverScript.enemySpawnerScript = enemySpawnerScript;
        }

        if (passengerScript != null)
        {

            passengerScript.scoreManager = scoreManager;
            passengerScript.missionmanager = missionManager;
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = player.transform.position + new Vector3(randomDirection.x, 0, randomDirection.y);
        return spawnPosition;
    }

    bool IsWalkable(Vector3 position)
    {
        NavMeshHit hit;
        bool isWalkable = NavMesh.SamplePosition(position, out hit, Mathf.Infinity, NavMesh.AllAreas);
        return isWalkable;
    }
}