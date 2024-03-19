using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missionManagerScript : MonoBehaviour
{
    public GameObject groundObject;

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
    private float minimumObstacleDistance = 20f;
    public GameObject buildingHolder;
    void Start()
    {
        groundObject = GameObject.Find("Ground");
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
        Vector3 randomOffset = Random.insideUnitSphere * (spawnRadius - 10f); // Subtract 10 units from the spawn radius

        // Ensure that the random offset is within the bounds of the ground object
        Vector3 spawnPosition = playerPosition + randomOffset;
        if (groundObject != null)
        {
            Renderer groundRenderer = groundObject.GetComponent<Renderer>();
            if (groundRenderer != null)
            {
                Bounds groundBounds = groundRenderer.bounds;
                // Adjust the bounds by 10 units
                float minX = groundBounds.min.x + 10f;
                float maxX = groundBounds.max.x - 10f;
                float minZ = groundBounds.min.z + 10f;
                float maxZ = groundBounds.max.z - 10f;

                // Clamp the spawn position within the adjusted bounds
                spawnPosition.x = Mathf.Clamp(spawnPosition.x, minX, maxX);
                spawnPosition.z = Mathf.Clamp(spawnPosition.z, minZ, maxZ);
            }
            else
            {
                Debug.LogWarning("Ground object does not have a Renderer component!");
            }
        }
        else
        {
            Debug.LogWarning("Ground object reference is null!");
        }

        // Ensure that the y-component is always 0
        spawnPosition.y = 0f;

        // Check for obstacles within a certain radius
        Collider[] colliders = Physics.OverlapSphere(spawnPosition, minimumObstacleDistance);
        foreach (Collider collider in colliders)
        {
            // If the collider is tagged as an obstacle
            if (collider.CompareTag("Obstacle"))
            {
                // Adjust spawn position to be at least minimumObstacleDistance away from the obstacle
                Vector3 directionToObstacle = spawnPosition - collider.transform.position;
                float distanceToObstacle = directionToObstacle.magnitude;
                if (distanceToObstacle < minimumObstacleDistance)
                {
                    spawnPosition += directionToObstacle.normalized * (minimumObstacleDistance - distanceToObstacle);
                }
            }
        }

        return spawnPosition;
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
