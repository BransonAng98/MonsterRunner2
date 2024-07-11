using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTriggerSecond : MonoBehaviour
{

    public EnemySpawner enemyspawnerScript;
    public Transform secondSpawnPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        enemyspawnerScript.threatlvl =1;
        enemyspawnerScript.tutorialSpawner = secondSpawnPosition;
        enemyspawnerScript.UpdateEnemiesForThreatLevel();
    }
}
