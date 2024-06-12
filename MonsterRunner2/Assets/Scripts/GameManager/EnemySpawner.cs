using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform playerPos;
    [SerializeField] private int threatlvl;
    [SerializeField] private GameObject[] enemyTypesPrefabs;
    [SerializeField] private DemoPlayer playerData; // Assuming you have a PlayerData script to pass to enemies

    private List<GameObject> spawnedEnemies = new List<GameObject>();
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

    private void SpawnEnemies()
    {
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
        float spawnRadius = 50f; // Adjust as needed
        float minSpacing = 5f; // Adjust as needed

        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += playerPos.position;
        randomDirection.y = playerPos.position.y;

        // Ensure minimum spacing between enemies
        foreach (var pos in spawnedEnemies)
        {
            while (Vector3.Distance(randomDirection, pos.transform.position) < minSpacing)
            {
                randomDirection = Random.insideUnitSphere * spawnRadius;
                randomDirection += playerPos.position;
                randomDirection.y = playerPos.position.y;
            }
        }

        return randomDirection;
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