using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject gridPrefab; // Reference to the prefab of the grid
    [SerializeField] private float gridDistance;
  

   
    // Start is called before the first frame update
    void Start()
    {

        // Get all colliders of the gridPrefab
        gridPrefab = Resources.Load("Grid") as GameObject;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SpawnNextGrid(Collider collider)
    {
        gridDistance = 130f;
        Vector3 spawnPosition = transform.position;

        if (this.CompareTag("TopCollider"))
            spawnPosition += Vector3.forward * gridDistance; // Move forward in the z-axis
        else if (this.CompareTag("BottomCollider"))
            spawnPosition -= Vector3.forward * gridDistance; // Move backward in the z-axis
        else if (this.CompareTag("LeftCollider"))
            spawnPosition -= Vector3.right * gridDistance; // Move left in the x-axis
        else if (this.CompareTag("RightCollider"))
            spawnPosition += Vector3.right * gridDistance; // Move right in the x-axis

        // Spawn the next grid
        GameObject nextGrid = Instantiate(gridPrefab, spawnPosition, Quaternion.identity);


        Collider[] childColliders = nextGrid.GetComponentsInChildren<Collider>();

        // Disable the appropriate collider(s) based on the collider that triggered the spawn
        foreach (Collider childCollider in childColliders)
        {
            if (this.CompareTag("TopCollider") && childCollider.CompareTag("BottomCollider"))
                childCollider.enabled = false; // Disable the bottom collider of the new grid
            else if (this.CompareTag("BottomCollider") && childCollider.CompareTag("TopCollider"))
                childCollider.enabled = false; // Disable the top collider of the new grid
            else if (this.CompareTag("LeftCollider") && childCollider.CompareTag("RightCollider"))
                childCollider.enabled = false; // Disable the right collider of the new grid
            else if (this.CompareTag("RightCollider") && childCollider.CompareTag("LeftCollider"))
                childCollider.enabled = false; // Disable the left collider of the new grid
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.transform.parent.CompareTag("Player"))
        {
            Debug.Log("PlayerDetected");
            SpawnNextGrid(this.transform.parent.GetComponent<Collider>());
            GetComponent<Collider>().enabled = false; // Disable the collider associated with this GridSpawner
        }
    }
}
