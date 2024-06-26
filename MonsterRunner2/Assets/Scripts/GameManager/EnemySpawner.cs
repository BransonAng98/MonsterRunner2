using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerPos;
    [SerializeField] public int threatlvl;
    [SerializeField] private GameObject[] enemyTypesPrefabs;
    [SerializeField] public DemoPlayer playerData; // Assuming you have a PlayerData script to pass to enemies
    [SerializeField] private float Radius;
    [SerializeField] public SideObjectiveQuestGiver SideObjectiveQuestGiverScript;
    [SerializeField] public QuestGiver QuestGiverScript;

    [SerializeField] public bool startSpawning;
    public GameObject player;
   [SerializeField] private List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField]private Dictionary<int, List<int>> threatLevelEnemies = new Dictionary<int, List<int>>()
    {
        { 1, new List<int> { 4, 0 } }, // 4 of type 1, 0 of type 2
        { 2, new List<int> { 6, 0 } }, // 4 of type 1, 2 of type 2
        { 3, new List<int> { 8, 0 } },
        { 4, new List<int> { 6, 2 } },
        { 5, new List<int> { 4, 4 } },
        // Add more threat levels as needed
    };

    [SerializeField]private int currentThreatLevel = 0;
    [SerializeField]private List<GameObject> roads = new List<GameObject>(); // List to store road objects

    private void Start()
    {
        // Assign the player transform by finding the object with the tag "Player"

        startSpawning = false;
      
        //UpdateEnemiesForThreatLevel();
    }

    private void Update()
    {
        currentThreatLevel = threatlvl;

        bool allNull = true;
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                allNull = false;
                break;
            }
        }

        if (allNull & startSpawning == true)
        {
            UpdateEnemiesForThreatLevel();
        }
     
    }
       
    public void UpdateEnemiesForThreatLevel()
    {
        Debug.Log("Updating Enemies for Threat Level: " + currentThreatLevel);

        if (!threatLevelEnemies.ContainsKey(currentThreatLevel))
        {
            Debug.LogError("Threat level not defined!");
            return;
        }

        List<int> enemyCounts = threatLevelEnemies[currentThreatLevel];
        List<GameObject> newSpawnedEnemies = new List<GameObject>(spawnedEnemies);

        for (int i = 0; i < enemyCounts.Count; i++)
        {
            int requiredCount = enemyCounts[i];
            int currentCount = CountEnemiesOfType(i);

            if (currentCount < requiredCount)
            {
                for (int j = 0; j < (requiredCount - currentCount); j++)
                {
                    Vector3 spawnPosition = GetRandomSpawnPosition();
                    GameObject enemy = Instantiate(enemyTypesPrefabs[i], spawnPosition, Quaternion.identity);
                    enemy.transform.LookAt(playerPos);
                    newSpawnedEnemies.Add(enemy);
                    AssignEnemyProperties(enemy);
                    
                }
            }
            else if (currentCount > requiredCount)
            {
                for (int j = 0; j < (currentCount - requiredCount); j++)
                {
                    GameObject enemyToRemove = FindEnemyOfType(i);
                    if (enemyToRemove != null)
                    {
                        newSpawnedEnemies.Remove(enemyToRemove);
                      
                    }
                }
            }
        }

        spawnedEnemies = newSpawnedEnemies;
    }

    private int CountEnemiesOfType(int typeIndex)
    {
        int count = 0;
        foreach (var enemy in spawnedEnemies)
        {
            enemyCarDriver enemyDriver = enemy.GetComponent<enemyCarDriver>();
            if (enemyDriver != null && enemyDriver.enemyType == typeIndex)
            {
                count++;
            }
        }
        return count;
    }

    private GameObject FindEnemyOfType(int typeIndex)
    {
        foreach (var enemy in spawnedEnemies)
        {
            enemyCarDriver enemyDriver = enemy.GetComponent<enemyCarDriver>();
            if (enemyDriver != null && enemyDriver.enemyType == typeIndex)
            {
                return enemy;
            }
        }
        return null;
    }

    private void SpawnEnemies()
    {
        Debug.Log("Spawn Wave");
        spawnedEnemies.Clear();

        if (!threatLevelEnemies.ContainsKey(threatlvl))
        {
            Debug.LogError("Threat level not defined!");
            return;
        }

        List<int> enemyCounts = threatLevelEnemies[threatlvl];

        for (int i = 0; i < enemyCounts.Count; i++)
        {
            for (int j = 0; j < enemyCounts[i]; j++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                GameObject enemy = Instantiate(enemyTypesPrefabs[i], spawnPosition, Quaternion.identity);
                enemy.transform.LookAt(playerPos);
                spawnedEnemies.Add(enemy);
                AssignEnemyProperties(enemy);
                //StartCoroutine(enemy.GetComponent<EnemyCarAI>().FlashTransparent(4f, 1f)); // Call FlashTransparent
            }
        }
    }

    public void SpawnSingleEnemy(int enemyType)
    {
        if (!threatLevelEnemies.ContainsKey(threatlvl))
        {
            Debug.LogError("Threat level not defined!");
            return;
        }

        List<int> enemyCounts = threatLevelEnemies[threatlvl];

        if (enemyType < 0 || enemyType >= enemyCounts.Count)
        {
            Debug.LogError("Invalid enemy type!");
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyTypesPrefabs[enemyType], spawnPosition, Quaternion.identity);
        enemy.transform.LookAt(playerPos);
        spawnedEnemies.Add(enemy);
        AssignEnemyProperties(enemy);
        //StartCoroutine(enemy.GetComponent<EnemyCarAI>().FlashTransparent(4f, 1f)); // Call FlashTransparent
    }

    public void RemoveEnemyFromList(GameObject enemyToRemove)
    {
        if (spawnedEnemies.Contains(enemyToRemove))
        {
            spawnedEnemies.Remove(enemyToRemove); // Remove the enemy car from the list
          
            enemyCarDriver enemyAI = enemyToRemove.GetComponent<enemyCarDriver>();
            if (enemyAI != null)
            {
                int enemyType = enemyAI.enemyType; // Assuming you have a property to get the enemy type
                SpawnSingleEnemy(enemyType);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float spawnRadius = 140f;
        float minSpacing = 30f;
        float minDistanceFromPlayer = 100f; // Minimum distance from the player

        List<Vector3> validSpawnPositions = new List<Vector3>();
        int attempts = 0;
        int maxAttempts = 100; // Avoid infinite loops

        // Check if any road objects are within the spawn area
        foreach (var road in roads)
        {
            Vector3 roadPos = road.transform.position;
            float distanceToPlayer = Vector3.Distance(roadPos, playerPos.position);

            if (distanceToPlayer >= minDistanceFromPlayer && distanceToPlayer <= spawnRadius)
            {
                // Check if the road position meets spacing criteria
                bool isValidRoadPos = true;
                foreach (var pos in spawnedEnemies)
                {
                    if (Vector3.Distance(roadPos, pos.transform.position) < minSpacing)
                    {
                        isValidRoadPos = false;
                        break;
                    }
                }

                if (isValidRoadPos)
                {
                    validSpawnPositions.Add(roadPos);
                }
            }
        }

        while (validSpawnPositions.Count == 0 && attempts < maxAttempts)
        {
            attempts++;
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection += playerPos.position;
            randomDirection.y = 0f; // Set the Y position to 0

            bool isValid = true;

            if (Vector3.Distance(randomDirection, playerPos.position) < minDistanceFromPlayer)
            {
                isValid = false;
            }

            foreach (var pos in spawnedEnemies)
            {
                if (Vector3.Distance(randomDirection, pos.transform.position) < minSpacing)
                {
                    isValid = false;
                    break; // No need to check further, this position is invalid
                }
            }

            if (isValid)
            {
                validSpawnPositions.Add(randomDirection);
            }
        }

        if (validSpawnPositions.Count == 0)
        {
            Debug.LogError("No valid spawn positions found!");
            return playerPos.position + new Vector3(minDistanceFromPlayer, 0, 0); // Default to a position if none found
        }

        // Pick a random valid spawn position from the list
        int randomIndex = Random.Range(0, validSpawnPositions.Count);
        return validSpawnPositions[randomIndex];
    }

    public void DestroyAllEnemies()
    {
        // Create a temporary list to store enemies to be destroyed
        List<GameObject> enemiesToDestroy = new List<GameObject>(spawnedEnemies);

        foreach (var enemy in enemiesToDestroy)
        {
            if (enemy != null)
            {
                enemyCarDriver enemyscript = enemy.GetComponent<enemyCarDriver>();
                if (enemyscript != null)
                {
                    enemyscript.CarDeath(0);
                }
            }
        }
        foreach(var enemy in spawnedEnemies)
        {
            if(enemy != null)
            {
                Destroy(enemy);
            }
        }
        spawnedEnemies.Clear();
        startSpawning = false;
        // Clear the original list after destroying all enemies
        //
    }

    private void AssignEnemyProperties(GameObject spawnedEnemy)
    {
        EnemyCarAI enemyAI = spawnedEnemy.GetComponent<EnemyCarAI>();
        enemyCarDriver enemyDriverlogic = spawnedEnemy.GetComponent<enemyCarDriver>();
        EnemyGunnerScript enemyGunnerAI = spawnedEnemy.GetComponent<EnemyGunnerScript>();

        if (enemyAI != null)
        {
            enemyAI.targetPositionTranform = playerPos;
            // Assign other necessary properties to enemyAI
        }
        if (enemyDriverlogic != null)
        {
            enemyDriverlogic.playerscript = playerData;
            enemyDriverlogic.enemySpawnerScript = this;
            enemyDriverlogic.sideobjective = SideObjectiveQuestGiverScript;
            enemyDriverlogic.questgiverScript = QuestGiverScript;
            // Assign other necessary properties to enemyDriverlogic
        }
        if (enemyGunnerAI != null)
        {
            enemyGunnerAI.playerdata = playerData;
            // Assign other necessary properties to enemyGunnerAI
        }
    }
}