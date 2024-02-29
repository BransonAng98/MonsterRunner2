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

    // Update is called once per frame
    void Update()
    {
       
        if (gridCount > 2)
        {

            if(gridObjects[0].name == "TestGrid")
            {
                GameObject objectToRemove = gridObjects[0]; // Remove the first object for simplicity, you can change this logic as per your requirement
                //gridObjects[0].SetActive(false); // Disable the first grid object if its name is "Grid"
                gridObjects.Remove(objectToRemove);
                gridCount--;
            }
            else
            {
                Debug.Log("Delete Grid");
                GameObject objectToRemove = gridObjects[0]; // Remove the first object for simplicity, you can change this logic as per your requirement
                gridObjects.Remove(objectToRemove);
                Destroy(objectToRemove);
                gridCount--;
            }
            
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
