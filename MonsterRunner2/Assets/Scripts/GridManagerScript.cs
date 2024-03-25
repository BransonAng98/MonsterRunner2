using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManagerScript : MonoBehaviour
{
    //public BossEnemyScript boss;
    [SerializeField] private int gridCount;
    [SerializeField] private List<GameObject> gridObjects = new List<GameObject>();
    [SerializeField] private GameObject furthestGrid;

    public GameObject treeParent; // Parent object for trees
    public GameObject stoneParent; // Parent object for stones
    public GameObject houseParent; // Parent object for houses
    public GameObject enemyspawnerHolder;

    public missionManagerScript missionManager;
    public Transform playerPos;
    public DemoPlayer playerData;
    public GameController gameController;
    public ScoreManagerScript scoreManager;
    public GridManagerScript gridmanager;
    void Start()
    {
        //boss = GameObject.FindGameObjectWithTag("Monster").GetComponent<BossEnemyScript>();
        //boss.gameObject.SetActive(false);
        GameObject[] gridArray = GameObject.FindGameObjectsWithTag("Grid");
       

    }

    void Update()
    {
        if (gridCount > 10)
        {
            // Find the player's position
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) // Check if player GameObject exists
            {
                Vector3 playerPosition = player.transform.position;

                // Remove null references from the list
                gridObjects.RemoveAll(item => item == null);

                // Initialize variables to track the furthest grid object and its distance
                furthestGrid = null;
                float maxDistance = 0f;

                // Iterate through each grid object to find the furthest one from the player
                foreach (GameObject gridObject in gridObjects)
                {
                    // Calculate the distance between the grid object and the player
                    float distance = Vector3.Distance(gridObject.transform.position, playerPosition);

                    // Update the furthest grid object if necessary
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        furthestGrid = gridObject;
                    }
                }

                if (furthestGrid != null)
                {
                    gridObjects.Remove(furthestGrid);
                    (furthestGrid).SetActive(false);
                    gridCount--;
                }
            }
        }
    }

    public void AddtoGridCount()
    {
        gridCount++;
    }
    void TurnMonsterOn()
    {
        //boss.gameObject.SetActive(true);
    }

    public void AddGridObject(GameObject gridObject)
    {
        gridObjects.Add(gridObject);
        //gridCount++;
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
