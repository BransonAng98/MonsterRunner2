using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PassengerSpawner : MonoBehaviour
{
    public GameObject player; // Reference to the player
    public GameObject prefabToSpawn; // The prefab to spawn
    public float spawnDistance = 10f; // Distance from the player to spawn the prefab
    public float maxSearchRadius = 5f; // Maximum search radius for finding a walkable position

    private void Start()
    {
        SpawnPassenger();
    }
    void Update()
    {
        
    }

    Vector3 GetSpawnPosition(Vector3 playerPosition, float distance)
    {
        // Generate a random direction vector
        Vector2 randomDirection = Random.insideUnitCircle.normalized * distance;
        Vector3 spawnPosition = playerPosition + new Vector3(randomDirection.x, 0, randomDirection.y);
        return spawnPosition;
    }

    bool IsWalkable(Vector3 position, float maxDistance)
    {
        NavMeshHit hit;
        bool isWalkable = NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas);
        return isWalkable;
    }

    public void SpawnPassenger()
    {
        Vector3 spawnPosition = GetSpawnPosition(player.transform.position, spawnDistance);
        if (IsWalkable(spawnPosition, maxSearchRadius))
        {
            Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
