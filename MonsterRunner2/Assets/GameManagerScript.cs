using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public BossEnemyScript boss;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Monster").GetComponent<BossEnemyScript>();
        boss.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void TurnMonsterOn()
    {
        boss.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Perform some action, for example, print a message
            TurnMonsterOn();
        }
    }
}
