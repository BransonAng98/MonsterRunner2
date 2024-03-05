using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawnerScript : MonoBehaviour
{
    public GameObject[] prefabs; // Array of prefabs to instantiate
    [SerializeField] private int minPrefabs = 1; // Minimum number of prefabs to instantiate
    [SerializeField] private int  maxPrefabs = 5; // Maximum number of prefabs to instantiate
    [SerializeField] private float minXRange = -100f; // Minimum spawning width for the x-axis
    [SerializeField] private float maxXRange = 100f; // Maximum spawning width for the x-axis
    [SerializeField] private float minZRange = -100f; // Minimum spawning width for the z-axis
    [SerializeField] private float maxZRange = 100f; // Maximum spawning width for the z-axis
    [SerializeField] private float minDistance = 5f; // Minimum distance between instantiated objects

    private List<Vector3> instantiatedPositions = new List<Vector3>(); // List to store positions of instantiated objects

    void Start()
    {
        SpawnProps();
    }


    void SpawnProps()
    {
        // Generate a random number of prefabs to instantiate
        int numPrefabs = Random.Range(minPrefabs, maxPrefabs);

        // Instantiate random prefabs with random scales
        for (int i = 0; i < numPrefabs; i++)
        {
            // Choose a random prefab from the array
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

            // Get a random position
            Vector3 randomPosition = GetRandomPosition();

            // Check if the position is too close to existing instantiated objects
            while (!IsPositionValid(randomPosition))
            {
                // If too close, get a new random position
                randomPosition = GetRandomPosition();
            }

            // Instantiate the prefab at the validated position
            GameObject instantiatedPrefab = Instantiate(prefab, randomPosition, Quaternion.identity);

            // Generate a random scale between 4 and 10 for the prefab
            float randomScale = Random.Range(4f, 10f);
            instantiatedPrefab.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            instantiatedPrefab.transform.SetParent(transform);

            // Add the position of the instantiated object to the list
            instantiatedPositions.Add(randomPosition);
        }
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
        return spawnPosition + new Vector3(randomXOffset, 3f, randomZOffset);
    }

    // Helper method to check if a position is too close to existing instantiated objects
    bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 instantiatedPosition in instantiatedPositions)
        {
            if (Vector3.Distance(position, instantiatedPosition) < minDistance)
            {
                // Position is too close to an existing instantiated object
                return false;
            }
        }
        // Position is valid
        return true;
    }
}
