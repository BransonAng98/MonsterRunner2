using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject[] Trees; // Array of tree prefabs to instantiate
    public GameObject[] Stones; // Array of stone prefabs to instantiate
    public GameObject[] Houses; // Array of house prefabs to instantiate
    public GameObject[] enemySpawner;

    public int minTreePrefabs; // Minimum number of tree prefabs to instantiate
    public int maxTreePrefabs; // Maximum number of tree prefabs to instantiate
    public int minStonePrefabs; // Minimum number of stone prefabs to instantiate
    public int maxStonePrefabs; // Maximum number of stone prefabs to instantiate
    public int minHousePrefabs; // Minimum number of house prefabs to instantiate
    public int maxHousePrefabs; // Maximum number of house prefabs to instantiate
    public int minEnemySpawnerPrefabs;
    public int maxEnemySpawnerPrefabs;
    //private int minScale = 1; // Minimum scale for prefabs
   // private int maxScale = 1; // Maximum scale for prefabs
    private float minXRange = -100; // Minimum spawning width for the x-axis
    private float maxXRange = 100; // Maximum spawning width for the x-axis
    private float minZRange = -100; // Minimum spawning width for the z-axis
    private float maxZRange = 100; // Maximum spawning width for the z-axis
    private float minTreeDistance = 0.1f; // Minimum distance between tree prefabs
    private float minStoneDistance = 1.0f; // Minimum distance between stone prefabs
    private float minHouseDistance = 20.0f; // Minimum distance between house prefabs
    private float minenemySpawnerDistance = 100.0f; // Minimum distance between house prefabs

    [SerializeField] private List<Vector3> instantiatedTreePositions = new List<Vector3>(); // List to store positions of instantiated trees
    [SerializeField] private List<Vector3> instantiatedStonePositions = new List<Vector3>(); // List to store positions of instantiated stones
    [SerializeField] private List<Vector3> instantiatedHousePositions = new List<Vector3>(); // List to store positions of instantiated houses
    [SerializeField] private List<Vector3> instantiatedSpawnerPositions = new List<Vector3>(); // List to store positions of instantiated houses

    public GameObject treeParent; // Parent object for trees
    public GameObject stoneParent; // Parent object for stones
    public GameObject houseParent; // Parent object for houses
    public GameObject enemyspawnerHolder;

    public missionManagerScript missionManager;
    public Transform playerPos;
    public DemoPlayer playerData;
    public GameController gameController;
    public ScoreManagerScript scoreManager;
    //public GridManagerScript gridmanager;
    void Start()
    {
        SpawnEnvironment();
        missionManager.FindBuildingObjects();
        gameController.LocateSpawners();
    }

    void SpawnEnvironment()
    {
        // Spawn trees
        int numTreesToSpawn = Random.Range(minTreePrefabs, maxTreePrefabs + 1);
        for (int i = 0; i < numTreesToSpawn; i++)
        {
            SpawnPrefab(Trees, instantiatedTreePositions, minTreeDistance, treeParent, 0);
        }

        // Spawn stones
        int numStonesToSpawn = Random.Range(minStonePrefabs, maxStonePrefabs + 1);
        for (int i = 0; i < numStonesToSpawn; i++)
        {
            SpawnPrefab(Stones, instantiatedStonePositions, minStoneDistance, stoneParent, 0);
        }

        // Spawn houses
        int numHousesToSpawn = Random.Range(minHousePrefabs, maxHousePrefabs + 1);
        for (int i = 0; i < numHousesToSpawn; i++)
        {
            SpawnPrefab(Houses, instantiatedHousePositions, minHouseDistance, houseParent, 0);
        }

        // Spawn enemy spawners
        int numofSpawnersToSpawn = Random.Range(minEnemySpawnerPrefabs, maxEnemySpawnerPrefabs + 1);
        for (int i = 0; i < numofSpawnersToSpawn; i++)
        {
            SpawnPrefab(enemySpawner, instantiatedSpawnerPositions, minenemySpawnerDistance, enemyspawnerHolder, 1);
            // Note: In this case, no specific parent is assigned to the enemy spawners.
        }

    }


    void SpawnPrefab(GameObject[] prefabs, List<Vector3> instantiatedPositions, float minDistance, GameObject parentObject, int data)
    {
        const int maxAttempts = 100; // Maximum number of attempts to find a spawn location
        int attempts = 0;
        bool positionFound = false;
        Vector3 spawnPosition = new Vector3();

        while (attempts < maxAttempts && !positionFound)
        {
            spawnPosition = GetRandomPrefabPosition();
            if (!IsTooClose(spawnPosition, instantiatedPositions, minDistance) && !IsTooCloseToTrees(spawnPosition, instantiatedTreePositions, 5f))
            {
                positionFound = true;
            }
            attempts++;
        }

        if (!positionFound)
        {
            Debug.LogWarning("Failed to find a suitable spawn position after " + maxAttempts + " attempts.");
            return; // Skip spawning this object
        }

        // Spawn the prefab at the found position
        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];
        GameObject spawnedObject = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        // Additional setup (setting parent, adjusting rotation, etc.) remains unchanged
        spawnedObject.transform.parent = parentObject.transform;
        switch (data)
        {
            // Case 0 and Case 1 logic remains unchanged
        }

        // Randomize rotation around the y-axis
        float randomYRotation = Random.Range(0f, 360f);
        spawnedObject.transform.rotation = Quaternion.Euler(0f, randomYRotation, 0f);

        instantiatedPositions.Add(spawnPosition);
    }

    // Your existing helper methods (GetRandomPrefabPosition, IsTooClose, IsTooCloseToTrees) remain unchanged


    Vector3 GetRandomPrefabPosition()
    {
        Vector3 spawnPosition = transform.position;
        float randomXOffset = Random.Range(minXRange, maxXRange);
        float randomZOffset = Random.Range(minZRange, maxZRange);
        return spawnPosition + new Vector3(randomXOffset, 0f, randomZOffset);
    }

    bool IsTooCloseToTrees(Vector3 position, List<Vector3> treePositions, float minDistance)
    {
        foreach (Vector3 treePosition in treePositions)
        {
            if (Vector3.Distance(position, treePosition) < minDistance)
            {
                return true;
            }
        }
        return false;
    }
    bool IsTooClose(Vector3 position, List<Vector3> instantiatedPositions, float minDistance)
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
