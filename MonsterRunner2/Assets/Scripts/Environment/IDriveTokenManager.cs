using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDriveTokenManager : MonoBehaviour
{
    public List<Transform> spawnLocations = new List<Transform>();
    public List<GameObject> deactiveList = new List<GameObject>();
    public List<GameObject> activeList = new List<GameObject>();
    public GameObject letterTokenPrefab;
    public Vector2 uiLetterTarget;
    public Transform uiContainer;
    public List<GameObject> spawnedLetterTokens;

    public float invunDuration;
    public DemoPlayer playerData;

    [SerializeField] private bool i1;
    [SerializeField] private bool d;
    [SerializeField] private bool r;
    [SerializeField] private bool i2;
    [SerializeField] private bool v;
    [SerializeField] private bool e;
    // Start is called before the first frame update
    void Start()
    {
        SpawnLetterTokens();
    }

    public void SpawnLetterTokens()
    {
        // Shuffle the letter spawn locations
        List<Transform> shuffledLocations = new List<Transform>(spawnLocations);
        for (int i = 0; i < shuffledLocations.Count; i++)
        {
            Transform temp = shuffledLocations[i];
            int randomIndex = Random.Range(i, shuffledLocations.Count);
            shuffledLocations[i] = shuffledLocations[randomIndex];
            shuffledLocations[randomIndex] = temp;
        }

        // Spawn 5 tokens at the first 5 locations in the shuffled list
        for (int i = 0; i < 6; i++)
        {
            GameObject newToken = Instantiate(letterTokenPrefab, shuffledLocations[i].position, shuffledLocations[i].rotation);
            newToken.GetComponent<IDriveToken>().tokenID = i; // Assign ID from 0 to 4
            newToken.GetComponent<IDriveToken>().iDriveTokenManager = this;
            newToken.GetComponent<IDriveToken>().AssignVariable();
            spawnedLetterTokens.Add(newToken);
        }

        foreach (GameObject entity in activeList)
        {
            entity.SetActive(false);
        }

        foreach (GameObject entity in deactiveList)
        {
            entity.SetActive(true);
        }
    }

    //public void ActivateUIFeedback(string letter)
    //{ 
    //    // Instantiate the token prefab
    //    GameObject token = Instantiate(uiLetterTokenPrefab, uiContainer);
    //    token.GetComponent<IDriveUIToken>().AssignTargetPos(uiLetterTarget, letter);
    //    // Set the initial position of the token
    //    RectTransform tokenRect = token.GetComponent<RectTransform>();
    //    tokenRect.anchoredPosition = uiLetterTarget;
    //}

    public void DespawnLetterTokens(int id)
    {
        if (spawnedLetterTokens.Count > 0)
        {
            GameObject token = spawnedLetterTokens[id];
            token.SetActive(false);
        }
    }

    public void ActivateUIElement(int id)
    {
        deactiveList[id].SetActive(false);
        activeList[id].SetActive(true);
        switch (id)
        {
            case 0:
                i1 = true;
                break;
            case 1:
                d = true;
                break;
            case 2:
                r = true;
                break;
            case 3:
                i2 = true;
                break;
            case 4:
                v = true;
                break;
            case 5:
                e = true;
                break;
        }

        if(i1 && d && r && i2 && v && e)
        {
            StartCoroutine(TriggerInvunerability());
        }
    }

    private IEnumerator TriggerInvunerability()
    {
        // Set invulnerability on and reset the invulnerability duration
        playerData.invunSphere.SetActive(true);
        float remainingDuration = invunDuration;

        // Loop until the duration runs out
        while (remainingDuration > 0f)
        {
            remainingDuration -= Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Time out reached
        Debug.Log("Time out");
        playerData.invunSphere.SetActive(false);

        // Reset all booleans
        i1 = false; d = false; r = false; i2 = false; v = false; e = false;

        // Destroy all spawned letter tokens
        if (spawnedLetterTokens.Count > 0)
        {
            for (int i = spawnedLetterTokens.Count - 1; i >= 0; i--)
            {
                GameObject token = spawnedLetterTokens[i];
                spawnedLetterTokens.RemoveAt(i);
                Destroy(token);
            }
        }

        // Respawn letter tokens
        SpawnLetterTokens();
    }
}
