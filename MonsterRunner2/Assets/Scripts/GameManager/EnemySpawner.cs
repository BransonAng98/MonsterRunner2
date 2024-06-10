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

    public float spawnRange = 10f; // The range within which enemies can spawn

    public void SpawnEnemy(int amt)
    {
        for (int i = 0; i < amt; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject enemyPf = GetRandomEnemyPrefab(); // Randomly select an enemy type to spawn
            GameObject spawnedEnemy = Instantiate(enemyPf, spawnPosition, Quaternion.identity);
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
                // Assign other necessary properties to enemyGunnerAI
            }

            if (enemyGunnerAI != null)
            {
                enemyGunnerAI.playerdata = playerData;
                // Assign other necessary properties to enemyGunnerAI
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnRange, spawnRange);
        float randomZ = Random.Range(-spawnRange, spawnRange);
        return new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    }

    private GameObject GetRandomEnemyPrefab()
    {
        // Randomly select between the two enemy types
        return Random.Range(0, 2) == 0 ? enemyType1Pf : enemyType2Pf;
    }
}