using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float currentGameTime;
    public float numberThreshold;

    public List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] bool delayedSpawn;
    [SerializeField] int spawnAmt;
    [SerializeField] int spawnInterval;
    public List<GameObject> spawners = new List<GameObject>();
    private EnemySpawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        //LocateSpawners();
    }

    public void LocateSpawners()
    {
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (var entity in entities)
        {
            if(entity.GetComponent<EnemySpawner>() != null)
            {
                spawners.Add(entity);
            }

            else
            {
                return;
            }
        }
    }

    //Trigger upon death
    public void UpdateEnemyList(GameObject enemy)
    {
        if (enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);

            if (enemyList.Count > numberThreshold)
            {
                delayedSpawn = false;
            }
        }
    }

    void CheckEnemyAmt()
    {
        if (enemyList.Count >= numberThreshold)
        {
            delayedSpawn = true;
        }

        else
        {
            delayedSpawn = false;
        }
    }

    void TriggerSpawn()
    {
        if (!delayedSpawn)
        {
            foreach(var entity in spawners)
            {
                EnemySpawner eS = entity.GetComponent<EnemySpawner>();
                if(eS != null)
                {
                    eS.SpawnEnemy(spawnAmt);

                    GameObject[] entities = GameObject.FindGameObjectsWithTag("Enemy");

                    // Update each entity found
                    foreach (GameObject enemy in entities)
                    {
                        if (!enemyList.Contains(enemy))
                        {
                            enemyList.Add(enemy);
                        }
                    }
                }
            }

            delayedSpawn = false;
        }
    }

    void GameTime()
    {
        currentGameTime += Time.deltaTime;

        if(currentGameTime >= spawnInterval && !delayedSpawn)
        {
           // spawnAmt += 5;
            if (!delayedSpawn)
            {
                TriggerSpawn();
            }

            currentGameTime = 0f;
        }

        else
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyAmt();
        GameTime();
    }
}
