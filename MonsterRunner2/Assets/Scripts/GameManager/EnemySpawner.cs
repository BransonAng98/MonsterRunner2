using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyType1Pf; // Prefab for enemy type 1
    public GameObject enemyType2Pf; // Prefab for enemy type 2
    public Transform playerPos;
    public DemoPlayer playerData;
    public GameController gameController;
    public ScoreManagerScript scoreManager;

    private float minspawnRange = -60f; // The range within which enemies can spawn
    private float maxspawnRange = 60f; // The range within which enemies can spawn
    private float minDistanceFromPlayer = 30f; // Minimum distance from the player

    public void SpawnEnemy(int amt)
    {
        for (int i = 0; i < amt; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition();
            GameObject enemyPf = GetRandomEnemyPrefab(); // Randomly select an enemy type to spawn
            GameObject spawnedEnemy = Instantiate(enemyPf, spawnPosition, Quaternion.identity);
            AssignEnemyProperties(spawnedEnemy);
        }
    }

    private Vector3 GetValidSpawnPosition()
    {
        Vector3 spawnPosition;
        int attempts = 0;
        do
        {
            spawnPosition = GetRandomSpawnPositionAroundPlayer();
            attempts++;
        } while (Vector3.Distance(spawnPosition, playerPos.position) < minDistanceFromPlayer && attempts < 100);

        return spawnPosition;
    }

    private Vector3 GetRandomSpawnPositionAroundPlayer()
    {
        float randomX = Random.Range(minspawnRange, maxspawnRange);
        float randomZ = Random.Range(minspawnRange, maxspawnRange);
        Vector3 offset = new Vector3(randomX, 0f, randomZ);
        return playerPos.position + offset;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        // Randomly select between the two enemy types
        return Random.Range(0, 2) == 0 ? enemyType1Pf : enemyType2Pf;
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