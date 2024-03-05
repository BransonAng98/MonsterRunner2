using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject gridPrefab; // Reference to the prefab of the grid
    [SerializeField] private float gridDistance;
    public GameManagerScript GMscript;
    public GameObject gridChecker;
    public Vector3 spawnPosition;

   
    // Start is called before the first frame update
    void Start()
    {
        GMscript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        // Get all colliders of the gridPrefab
        

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnNextGrid(Collider collider)
    {
        GMscript.AddGridObject(gridPrefab);
        const float gridDistance = 162.5f;
         spawnPosition = transform.position;

        if (CompareTag("TopCollider"))
            spawnPosition += Vector3.forward * gridDistance;
        else if (CompareTag("BottomCollider"))
            spawnPosition -= Vector3.forward * gridDistance;
        else if (CompareTag("LeftCollider"))
            spawnPosition -= Vector3.right * gridDistance;
        else if (CompareTag("RightCollider"))
            spawnPosition += Vector3.right * gridDistance;

        gridChecker = Instantiate(gridChecker, spawnPosition, Quaternion.identity);
        GridDetection gridScript = gridChecker.GetComponent<GridDetection>();
        gridScript.SetSpawner(this);

        //Destroy(gridChecker); // Consider whether to enable this line
    }

    public void SpawnGridPrefab(GameObject prefab, Vector3 position)
    {
        GameObject nextGrid = Instantiate(prefab, position, Quaternion.identity);
        Collider[] childColliders = nextGrid.GetComponentsInChildren<Collider>();

        foreach (Collider childCollider in childColliders)
        {
            childCollider.enabled = true;
            if ((CompareTag("TopCollider") && childCollider.CompareTag("BottomCollider")) ||
                (CompareTag("BottomCollider") && childCollider.CompareTag("TopCollider")) ||
                (CompareTag("LeftCollider") && childCollider.CompareTag("RightCollider")) ||
                (CompareTag("RightCollider") && childCollider.CompareTag("LeftCollider")))
            {
                //childCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {
            SpawnNextGrid(this.transform.parent.GetComponent<Collider>());
            Debug.Log("PlayerDetected");
            //GetComponent<Collider>().enabled = false; // Disable the collider associated with this GridSpawner
        }
    }
}
