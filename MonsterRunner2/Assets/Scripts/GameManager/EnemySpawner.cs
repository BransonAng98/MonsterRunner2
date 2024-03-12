using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPf;

    public void SpawnEnemy(int amt)
    {
        for(int i = 0; i < amt; i++)
        {
            Instantiate(enemyPf, transform.position, Quaternion.identity);
        }   
    }
}
