using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPf;
    public Transform playerPos;
    public DemoPlayer playerData;
    public GameController gameController;
    public ScoreManagerScript scoreManager;

    public void SpawnEnemy(int amt)
    {
        for(int i = 0; i < amt; i++)
        {
            GameObject spawnedEnemy = Instantiate(enemyPf, transform.position, Quaternion.identity);
            EnemyScript enemyScript = spawnedEnemy.GetComponent<EnemyScript>();

            if(enemyScript != null)
            {
                enemyScript.player = playerPos;
                enemyScript.playerData = playerData;
                enemyScript.gameController = gameController;
                enemyScript.scoreManager = scoreManager;
            }
        }   
    }
}
