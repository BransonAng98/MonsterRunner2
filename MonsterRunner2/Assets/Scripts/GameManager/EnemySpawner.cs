using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerPos;
    [SerializeField] public int threatlvl;
    [SerializeField] private GameObject[] enemyTypesPrefabs;
    [SerializeField] private DemoPlayer playerData; // Assuming you have a PlayerData script to pass to enemies

    [SerializeField] private float Radius;

    [SerializeField]private List<GameObject> spawnedEnemies = new List<GameObject>();
    private Dictionary<int, List<int>> threatLevelEnemies = new Dictionary<int, List<int>>()
    {
        { 1, new List<int> { 4, 0 } }, // 4 of type 1, 0 of type 2
        { 2, new List<int> { 4, 2 } }  // 4 of type 1, 2 of type 2
        // Add more threat levels as needed
    };

    private void Start()
    {
        // Assign the player transform by finding the object with the tag "Player"
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerPos = player.transform;
            playerData = player.GetComponent<DemoPlayer>();
        }
        else
        {
            Debug.LogError("Player object not found!");
            return;
        }

        SpawnEnemies();
    }

    private void Update()
    {
        //change it when enemycardriver script is done. Instead of null change it to re
        bool allNull = true;
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                allNull = false;
                break;
            }
        }

        if (allNull)
        {
            SpawnEnemies();
        }
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
            }
        }
    }
    private Vector3 GetRandomSpawnPosition()
    {
        float spawnRadius = 140f;
        float minSpacing = 30f;
        float minDistanceFromPlayer = 50f; // Minimum distance from the player

        List<Vector3> validSpawnPositions = new List<Vector3>();

        // Generate random directions until a valid spawn position is found
        while (validSpawnPositions.Count == 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection += playerPos.position;
            randomDirection.y = 0f; // Set the Y position to 0

            bool isValid = true;

            // Check if the distance from the player is greater than the minimum distance
            if (Vector3.Distance(randomDirection, playerPos.position) < minDistanceFromPlayer)
            {
                isValid = false;
            }

            // Check if the distance from other spawned enemies is greater than the minimum spacing
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

        return validSpawnPositions[0]; // Return the first valid spawn position
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
            // Assign other necessary properties to enemyDriverlogic
        }
        if (enemyGunnerAI != null)
        {
            enemyGunnerAI.playerdata = playerData;
            // Assign other necessary properties to enemyGunnerAI
        }
    }


}