using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject[] Trees; // Array of tree prefabs to instantiate
    public GameObject[] Stones; // Array of stone prefabs to instantiate
    private int minTreePrefabs = 350; // Minimum number of prefabs to instantiate
    private int maxTreePrefabs = 350; // Maximum number of prefabs to instantiate
    private int minStonePrefabs = 20; // Minimum number of prefabs to instantiate
    private int maxStonePrefabs = 30; // Maximum number of prefabs to instantiate
    private int minScale = 1; // Minimum scale for prefabs
    private int maxScale = 1; // Maximum scale for prefabs
    private float minXRange = -300f; // Minimum spawning width for the x-axis
    private float maxXRange = 300f; // Maximum spawning width for the x-axis
    private float minZRange = -220f; // Minimum spawning width for the z-axis
    private float maxZRange = 220f; // Maximum spawning width for the z-axis
    private float minDistance = 1f; // Minimum distance between instantiated objects

    [SerializeField] private List<Vector3> instantiatedTreePositions = new List<Vector3>(); // List to store positions of instantiated trees
    [SerializeField] private List<Vector3> instantiatedStonePositions = new List<Vector3>(); // List to store positions of instantiated stones

    void Start()
    {
        // Spawn a random number of trees within the specified range
        int numTreesToSpawn = Random.Range(minTreePrefabs, maxTreePrefabs + 1);
        for (int i = 0; i < numTreesToSpawn; i++)
        {
            SpawnPrefab(Trees, instantiatedTreePositions);
        }

        // Spawn a random number of stones within the specified range
        int numStonesToSpawn = Random.Range(minStonePrefabs, maxStonePrefabs + 1);
        for (int i = 0; i < numStonesToSpawn; i++)
        {
            SpawnPrefab(Stones, instantiatedStonePositions);
        }
    }

    // Helper method to spawn a single prefab at a random position
    void SpawnPrefab(GameObject[] prefabs, List<Vector3> instantiatedPositions)
    {
        // Get a random position within the specified range
        Vector3 spawnPosition = GetRandomPrefabPosition();

        // Check if the spawn position is too close to previously instantiated objects
        if (IsTooClose(spawnPosition, instantiatedPositions))
        {
            // If too close, try again with a new position
            SpawnPrefab(prefabs, instantiatedPositions);
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
    Vector3 GetRandomPrefabPosition()
    {
        // Get the position of the spawning GameObject
        Vector3 spawnPosition = transform.position;

        // Generate random offsets within the specified range for the x and z axes
        float randomXOffset = Random.Range(minXRange, maxXRange);
        float randomZOffset = Random.Range(minZRange, maxZRange);

        // Calculate the random position by adding the offsets to the spawn position
        return spawnPosition + new Vector3(randomXOffset, 0f, randomZOffset);
    }

    // Helper method to check if a position is too close to previously instantiated objects
    bool IsTooClose(Vector3 position, List<Vector3> instantiatedPositions)
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
