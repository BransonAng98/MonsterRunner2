using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class ObjectiveIndicator : MonoBehaviour
{
    public MeshRenderer arrowImage;
    public float rotationSpeed;

    [SerializeField] public Transform objectiveLoc;
    public Transform passengerLoc;
    public Transform playerTransform;
    [SerializeField] public LineRenderer path;
    [SerializeField] private float pathHeightOffset;
    [SerializeField] private float pathUpdateSpeed;

    private NavMeshTriangulation triangulation;
    private Coroutine DrawPathCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        arrowImage.GetComponent<MeshRenderer>();
    }

    //Trigger the arrow on or off
    public void UpdateObjective(int state, Transform objLoc)
    {
        switch (state)
        {
            //Update passenger detail
            case 0:
                path.enabled = true;
                passengerLoc = objLoc;
                if(DrawPathCoroutine != null)
                {
                    StopCoroutine(DrawPathToPassenger());
                }
                DrawPathCoroutine = StartCoroutine(DrawPathToPassenger());
                break;

            //Remove passenger detail
            case 1:
                passengerLoc = null;
                StopCoroutine(DrawPathToPassenger());
                path.enabled = false;
                break;

            //Update location detail
            case 2:
                objectiveLoc = objLoc;
                arrowImage.enabled = true;
                break;

            //Remove location detail
            case 3:
                objectiveLoc = null;
                arrowImage.enabled = false;
                break;
        }
    }

    private IEnumerator DrawPathToPassenger()
    {
        Debug.Log("Spawning line");
        WaitForSeconds wait = new WaitForSeconds(pathUpdateSpeed);
        NavMeshPath navPath = new NavMeshPath();

        while (passengerLoc != null)
        {
            if (NavMesh.CalculatePath(playerTransform.position, passengerLoc.position, NavMesh.AllAreas, navPath))
            {
                path.positionCount = navPath.corners.Length;

                for (int i = 0; i < navPath.corners.Length; i++)
                {
                    path.SetPosition(i, navPath.corners[i] + Vector3.up * pathHeightOffset);
                }
            }
            else
            {
                Debug.LogError($"Unable to calculate a path on the Navmesh between {playerTransform.position} and {passengerLoc.position}");
                yield return new WaitForSeconds(1.0f);
            }

            yield return wait; // Move this inside the loop
        }
    }
}
