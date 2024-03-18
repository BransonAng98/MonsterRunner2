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

    public GameObject buildingHolder;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        passengerCount = 0;
        buildingHolder = GameObject.Find("Buildings");
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
        // Find the holder object named "Building"
        buildingHolder = GameObject.Find("Buildings");
        if (buildingHolder == null)
        {
            Debug.LogWarning("Building holder object not found!");
            return;
        }

        // Find all objects with the layer named "Buildings" under the "Building" holder object
        Transform[] buildingChildren = buildingHolder.GetComponentsInChildren<Transform>();
        foreach (Transform child in buildingChildren)
        {
            if (child.gameObject.layer == LayerMask.NameToLayer("Buildings"))
            {
                // Add the child object to the list if it's under the "Buildings" layer
                buildingObjectsList.Add(child.gameObject);
            }
        }
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

        RaycastHit hitGround = new RaycastHit(); // Initialize with default value
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

            // Raycast down from the spawn position to check if it's on the "Ground" layer
            if (Physics.Raycast(spawnPosition, Vector3.down, out hitGround, Mathf.Infinity, LayerMask.GetMask("Ground")))
            {
                // Check if there's an obstacle within 5 units of the spawn position
                if (!Physics.SphereCast(spawnPosition, 5f, Vector3.up, out hitObstacle, 0f, LayerMask.GetMask("Obstacles")))
                {
                    // If no obstacles are found within 5 units, set the spawn position and exit the loop
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
