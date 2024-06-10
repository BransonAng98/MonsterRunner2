using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTokenManager : MonoBehaviour
{
    public GameObject powerUpType1Prefab; // First power-up prefab
    public GameObject powerUpType2Prefab; // Second power-up prefab
    public List<Transform> spawnLocations; // List of spawn locations
    public List<GameObject> spawnedTokens;
    public bool spawnType2; // Flag to determine if the second power-up should be spawned
    private int nextSpawnLocationIndex; // Index of the next spawn location to be used

    void Start()
    {
        // Initialize the index of the next spawn location to be used
        nextSpawnLocationIndex = 0;

        // Spawn power-ups at alternating locations
        SpawnPowerUps();
    }

    // Method to spawn power-ups at alternating locations
    public void SpawnPowerUps()
    {
        // Iterate through each spawn location
        for (int i = 0; i < spawnLocations.Count; i++)
        {
            // Get the current spawn location
            Transform spawnLocation = spawnLocations[i];

            // Spawn the first power-up at the current spawn location
            GameObject token1 = Instantiate(powerUpType1Prefab, spawnLocation.position, Quaternion.identity);
            spawnedTokens.Add(token1);

            // If spawnType2 is true and there are more spawn locations, spawn the second power-up at the next spawn location
            if (spawnType2 && i < spawnLocations.Count - 1)
            {
                // Increment the index of the next spawn location
                i++;
                spawnLocation = spawnLocations[i];

                // Spawn the second power-up at the next spawn location
                GameObject token2 = Instantiate(powerUpType2Prefab, spawnLocation.position, Quaternion.identity);
                spawnedTokens.Add(token2);
            }
        }
    }

    public void DespawnTokens()
    {
        Debug.Log("Despawn Tokens");
        if(spawnedTokens.Count > 0)
        {
            foreach (GameObject token in spawnedTokens)
            {
                Destroy(token);
                spawnedTokens.Remove(token);
            }
        }
    }
}
