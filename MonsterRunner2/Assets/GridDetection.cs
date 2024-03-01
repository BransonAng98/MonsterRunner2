using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridDetection : MonoBehaviour
{
    public  int entitiesDetected;
    private Collider _collider;
    [SerializeField]private GridSpawner _spawner;
    private bool hasSpawnedGrid = false;

    private void Start()
    {
      
        // Get the collider component attached to the GameObject
        _collider = GetComponent<Collider>();

       
    }
    public void SetSpawner(GridSpawner spawner)
    {
        _spawner = spawner;
    }

    public void Update()
    {
        if (entitiesDetected == 0 && !hasSpawnedGrid)
        {
            Debug.Log("NoGridDetected");
            _spawner.SpawnGridPrefab(_spawner.gridPrefab, _spawner.spawnPosition);
            hasSpawnedGrid = true; // Set the flag to true to prevent further calls
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Grid"))
        {
            Debug.Log("GridDetected");
            entitiesDetected = 1;
            // Disable the collider associated with this GridDetection script

        }

        else
        {
            Debug.Log("NoGridDetected");
          
        }
    }
}

