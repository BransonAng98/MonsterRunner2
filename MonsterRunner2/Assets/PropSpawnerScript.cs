using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawnerScript : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs to instantiate
     private int minPrefabs = 3; // Minimum number of prefabs to instantiate
     private int  maxPrefabs = 10; // Maximum number of prefabs to instantiate
    private int minScale = 4; // Minimum number of prefabs to instantiate
    private int maxScale = 12; // Maximum number of prefabs to instantiate
    [SerializeField] private float minXRange = -100f; // Minimum spawning width for the x-axis
    [SerializeField] private float maxXRange = 100f; // Maximum spawning width for the x-axis
    [SerializeField] private float minZRange = -100f; // Minimum spawning width for the z-axis
    [SerializeField] private float maxZRange = 100f; // Maximum spawning width for the z-axis
    [SerializeField] private float minDistance = 5f; // Minimum distance between instantiated objects

    [SerializeField]private List<Vector3> instantiatedPositions = new List<Vector3>(); // List to store positions of instantiated objects

    void Start()
    {
        // Spawn a random number of prefabs within the specified range
        int numPrefabsToSpawn = Random.Range(minPrefabs, maxPrefabs + 1);
        for (int i = 0; i < numPrefabsToSpawn; i++)
        {
            SpawnPrefab();
        }
    }

    // Helper method to spawn a single prefab at a random position
    void SpawnPrefab()
    {
        // Get a random position within the specified range
        Vector3 spawnPosition = GetRandomPosition();

        // Check if the spawn position is too close to previously instantiated objects
        if (IsTooClose(spawnPosition))
        {
            // If too close, try again with a new position
            SpawnPrefab();
            return;
        }

        // Select a random prefab from the array
        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];

        // Instantiate the prefab at the random position
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Randomize scale
        float randomScale = Random.Range(minScale, maxScale);
        spawnedObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        // Randomize rotation around the y-axis
        float randomYRotation = Random.Range(0f, 360f);
        spawnedObject.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);

        // Add the spawned position to the list of instantiated positions
        instantiatedPositions.Add(spawnPosition);
    }

    // Helper method to get a random position within the specified range around the spawning GameObject
    Vector3 GetRandomPosition()
    {
        // Get the position of the spawning GameObject
        Vector3 spawnPosition = transform.position;

        // Generate random offsets within the specified range for the x and z axes
        float randomXOffset = Random.Range(minXRange, maxXRange);
        float randomZOffset = Random.Range(minZRange, maxZRange);

        // Calculate the random position by adding the offsets to the spawn position
        return spawnPosition + new Vector3(randomXOffset, 10f, randomZOffset);
    }

    // Helper method to check if a position is too close to previously instantiated objects
    bool IsTooClose(Vector3 position)
    {
        foreach (Vector3 instantiatedPosition in instantiatedPositions)
        {
            if (Vector3.Distance(position, instantiatedPosition) < minDistance)
            {
                return true;
            }
        }
        return false;
    }


}
