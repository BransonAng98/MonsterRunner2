using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //public BossEnemyScript boss;
    [SerializeField] private int gridCount;
    [SerializeField] private List<GameObject> gridObjects = new List<GameObject>();
   

    void Start()
    {
        //boss = GameObject.FindGameObjectWithTag("Monster").GetComponent<BossEnemyScript>();
        //boss.gameObject.SetActive(false);
        GameObject[] gridArray = GameObject.FindGameObjectsWithTag("Grid");
       

    }

    void Update()
    {
        if (gridCount > 4)
        {

            GameObject objectToRemove = gridObjects[0];
            gridObjects.Remove(objectToRemove);
            Destroy(objectToRemove);
            gridCount--;
            //gridObjects.RemoveAt(0);
          
        }
    }
    void TurnMonsterOn()
    {
        //boss.gameObject.SetActive(true);
    }

    public void AddGridObject(GameObject gridObject)
    {
        gridObjects.Add(gridObject);
        gridCount++;
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
