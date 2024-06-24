using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractBoostTokenManager : MonoBehaviour
{
    public GameObject contractBoosterPrefab;
    public List<Transform> spawnLocations = new List<Transform>();
    public List<GameObject> spawnedTokens = new List<GameObject>();

    public int currentContractID;

    private void Start()
    {
        SpawnContractBoosters();
    }

    public void SpawnContractBoosters()
    {
        // Clear any existing tokens before spawning new ones
        DespawnContractBoosters();

        // Shuffle the spawn locations
        List<Transform> shuffledLocations = new List<Transform>(spawnLocations);
        for (int i = 0; i < shuffledLocations.Count; i++)
        {
            Transform temp = shuffledLocations[i];
            int randomIndex = Random.Range(i, shuffledLocations.Count);
            shuffledLocations[i] = shuffledLocations[randomIndex];
            shuffledLocations[randomIndex] = temp;
        }

        // Spawn 4 tokens at the first 4 locations in the shuffled list
        for (int i = 0; i < 4; i++)
        {
            GameObject newToken = Instantiate(contractBoosterPrefab, shuffledLocations[i].position, shuffledLocations[i].rotation);
            spawnedTokens.Add(newToken);
        }
    }

    public void DespawnContractBoosters()
    {
        Debug.Log("Despawn Tokens");
        if (spawnedTokens.Count > 0)
        {
            for (int i = spawnedTokens.Count - 1; i >= 0; i--)
            {
                GameObject token = spawnedTokens[i];
                spawnedTokens.RemoveAt(i);
                Destroy(token);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
