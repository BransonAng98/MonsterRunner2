using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManagerScript : MonoBehaviour
{
    // List to hold all objects under the "building" layer
    public List<GameObject> buildingObjectsList = new List<GameObject>();
    public QuestGiver questgiverEnitity;
    public GameObject destination;
    public GameObject passengerPrefab;
    [SerializeField] public int passengerCount;
    private float minXRange = -300f; // Minimum spawning width for the x-axis
    private float maxXRange = 300f; // Maximum spawning width for the x-axis
    private float minZRange = -220f; // Minimum spawning width for the z-axis
    private float maxZRange = 220f; // Maximum spawning width for the z-axis

    public GameObject player; // Reference to the player GameObject
    public float spawnRadius = 40f; // Radius around the player for spawning passengers
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        passengerCount = 0;
    }

    private void Update()
    {
        if(passengerCount == 0)
        {
            CreatePassenger();
        }
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

    public void GetCurrentPassenger()
    {
        questgiverEnitity = GameObject.FindGameObjectWithTag("Passenger").GetComponentInChildren<QuestGiver>();
    }

    public void GetDestination()
    {
        if(questgiverEnitity != null)
        {
            destination = questgiverEnitity.destination;
        }
    }

    Vector3 GetRandomPrefabPosition()
    {
        Vector3 playerPosition = player.transform.position;

        RaycastHit hitGround = new RaycastHit();
        RaycastHit hitObstacle;
        Vector3 randomOffset = Vector3.zero;
        bool validSpawnPosition = false;

        // Try to find a valid spawn position within the spawn radius
        int attempts = 0;
        while (!validSpawnPosition && attempts < 10) // Limit the number of attempts to prevent infinite loops
        {
            // Generate random offsets within the spawn radius
            float randomXOffset = Random.Range(-spawnRadius, spawnRadius);
            float randomZOffset = Random.Range(-spawnRadius, spawnRadius);

            randomOffset = new Vector3(randomXOffset, 0f, randomZOffset);

            // Calculate spawn position around the player
            Vector3 spawnPosition = playerPosition + randomOffset;

            // Raycast down from the player's position to check if the spawn position is on the ground layer
            if (Physics.Raycast(spawnPosition, Vector3.down, out hitGround, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                // Raycast up from the hit ground position to check for obstacles
                if (!Physics.Raycast(hitGround.point, Vector3.up, out hitObstacle, Mathf.Infinity, LayerMask.GetMask("Obstacles")))
                {
                    // If no obstacles are hit, set the spawn position and exit the loop
                    validSpawnPosition = true;
                }
            }

            attempts++;
        }

        // If a valid spawn position is found, return it; otherwise, return a random position within the radius
        if (validSpawnPosition)
        {
            return hitGround.point;
        }
        else
        {
            Debug.LogWarning("Unable to find a valid spawn position. Using random position within the radius.");
            return playerPosition + randomOffset;
        }
    }
    public void CreatePassenger()
    {
        Vector3 spawnPosition = GetRandomPrefabPosition();
        Instantiate(passengerPrefab, spawnPosition, Quaternion.identity);
        passengerCount++;
        GetCurrentPassenger();
        GetDestination();
    }
}
