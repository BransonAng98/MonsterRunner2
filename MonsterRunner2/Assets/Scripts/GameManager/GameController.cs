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

    private List<Transform> enemies = new List<Transform>();
    private Transform currentLeader;


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

    // Add a new enemy to the list
    public void AddEnemy(Transform enemy)
    {
        enemies.Add(enemy);
    }

    // Remove a dead enemy from the list
    public void RemoveEnemy(Transform enemy)
    {
        enemies.Remove(enemy);
    }

    // Check if the enemy is the first spawned one
    public bool IsFirstEnemy(Transform enemy)
    {
        return enemies.Count == 0 || enemy == enemies[0];
    }

    // Assign a new leader from the remaining enemies
    public void AssignNewLeader()
    {
        if (enemies.Count == 0)
        {
            currentLeader = null;
            return;
        }

        // Select a new leader (for simplicity, just choose the first enemy in the list)
        currentLeader = enemies[0];
    }

    // Get the current leader
    public Transform GetLeader()
    {
        return currentLeader;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyAmt();
        GameTime();
    }
}
