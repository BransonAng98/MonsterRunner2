using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDetection : MonoBehaviour
{
    public GridManagerScript GMscript;
    public int entitiesDetected;
    private Collider _collider;
    [SerializeField] private GridSpawner _spawner;
    [SerializeField] private bool hasSpawnedGrid = false;
    
    [SerializeField] private bool hasCollidedwithSomething;
    private float resetDelay = 0.5f; // Adjust as needed
    private WaitForSeconds waitTime;
    [SerializeField]private bool shouldResetEntities = true;

    private void Start()
    {
        GMscript = GameObject.Find("GridManager").GetComponent<GridManagerScript>();
        shouldResetEntities = true;
        hasSpawnedGrid = false;
        entitiesDetected = -1; // Initialize to 0
        _collider = GetComponent<Collider>();
        waitTime = new WaitForSeconds(resetDelay);
        StartCoroutine(ResetEntitiesDetected());
        //DestroySelf();
    }

    public void SetSpawner(GridSpawner spawner)
    {
        _spawner = spawner;
    }

    public void DestroySelf()
    {
        Destroy(gameObject, 3f);
    }
    public void Update()
    {
        if (entitiesDetected == 0 && !hasSpawnedGrid)
        {
          
            _spawner.SpawnGridPrefab(_spawner.gridPrefab, _spawner.spawnPosition); //spawnGrid
            GMscript.AddtoGridCount();
            Debug.Log("SpawnGrid");
            hasSpawnedGrid = true; // Set the flag to true to prevent further calls
            shouldResetEntities = false; // Stop resetting entities since grid has been spawned
        }
        if (entitiesDetected > 0 && !hasSpawnedGrid)
        {
            hasCollidedwithSomething = true;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grid"))
        {
            
            entitiesDetected = -1;
            hasCollidedwithSomething = true;
            shouldResetEntities = false;
        }
        else
        {
            return;
        }
    }

    private IEnumerator ResetEntitiesDetected()
    {
        while (shouldResetEntities)
        {
            yield return waitTime;
            if (!hasCollidedwithSomething)
            {
                entitiesDetected = 0;
            }
            hasCollidedwithSomething = false;
        }
    }
}

