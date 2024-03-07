using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawnerScript : MonoBehaviour
{

    [System.Serializable]
    public struct PrefabSpawnInfo
    {
        public GameObject prefab;
        public int minSpawnCount;
        public int maxSpawnCount;
        public int minScale;
        public int maxScale;
    }

    public PrefabSpawnInfo[] prefabsInfo; // Array of prefab spawn information
    [SerializeField] private float minXRange = -100f; // Minimum spawning width for the x-axis
    [SerializeField] private float maxXRange = 100f; // Maximum spawning width for the x-axis
    [SerializeField] private float minZRange = -100f; // Minimum spawning width for the z-axis
    [SerializeField] private float maxZRange = 100f; // Maximum spawning width for the z-axis
    [SerializeField] private float minDistance = 50f; // Minimum distance between instantiated objects

    [SerializeField] private List<Vector3> instantiatedPositions = new List<Vector3>(); // List to store positions of instantiated objects

    void Start()
    {
        // Spawn prefabs according to their specified min and max spawn counts
        foreach (var prefabInfo in prefabsInfo)
        {
            int numPrefabsToSpawn = Random.Range(prefabInfo.minSpawnCount, prefabInfo.maxSpawnCount + 1);
            for (int i = 0; i < numPrefabsToSpawn; i++)
            {
                SpawnPrefab(prefabInfo);
            }
        }
    }

    // Helper method to spawn a single prefab at a random position
    void SpawnPrefab(PrefabSpawnInfo prefabInfo)
    {
        // Get a random position within the specified range
        Vector3 spawnPosition = GetRandomPosition(prefabInfo);

        // Check if the spawn position is too close to previously instantiated objects
        if (IsTooClose(spawnPosition))
        {
            // If too close, try again with a new position
            SpawnPrefab(prefabInfo);
            return;
        }

        // Instantiate the prefab at the random position
        GameObject spawnedObject = Instantiate(prefabInfo.prefab, spawnPosition, Quaternion.identity);



        // Randomize scale
        float randomScale = Random.Range(prefabInfo.minScale, prefabInfo.maxScale);
        spawnedObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        // Randomize rotation around the y-axis

        // Randomize rotation around the y-axis
        float randomYRotation = Random.Range(0f, 360f);
        spawnedObject.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);

        // Add the spawned position to the list of instantiated positions
        instantiatedPositions.Add(spawnPosition);
        //spawnedObject.transform.SetParent(transform);
    }

    // Helper method to get a random position within the specified range around the spawning GameObject
    Vector3 GetRandomPosition(PrefabSpawnInfo prefabInfo)
    {
        // Get the position of the spawning GameObject
        Vector3 spawnPosition = transform.position;

        // Calculate the Y position using the entire Y scale of the prefab
        float prefabYScale = prefabInfo.prefab.transform.localScale.y;
        float spawnYPosition = transform.position.y + (prefabYScale / 2);

        // Generate random offsets within the specified range for the x and z axes
        float randomXOffset = Random.Range(minXRange, maxXRange);
        float randomZOffset = Random.Range(minZRange, maxZRange);

        // Calculate the random position by adding the offsets to the spawn position
        return new Vector3(spawnPosition.x + randomXOffset, spawnYPosition, spawnPosition.z + randomZOffset);
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
