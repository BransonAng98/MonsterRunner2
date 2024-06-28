using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveIndicator : MonoBehaviour
{
    public MeshRenderer arrowImage;
    public float rotationSpeed;

    [SerializeField] public Transform objectiveLoc;
    public Transform passengerLoc;
    [SerializeField] Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
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
                passengerLoc = objLoc;
                arrowImage.enabled = true;
                break;

            //Remove passenger detail
            case 1:
                passengerLoc = objLoc;
                break;

            //Update location detail
            case 2:
                objectiveLoc = objLoc;
                arrowImage.enabled = true;
                break;

            //Remove location detail
            case 3:
                objectiveLoc = objLoc;
                arrowImage.enabled = false;
                break;
        }
    }
    void RotateUIElement(Transform goal)
    {
        Vector3 directionToGoal = goal.position - playerTransform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToGoal);

        // Maintain the original x rotation while updating y and z rotations
        targetRotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goal.position - transform.position), rotationSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (objectiveLoc != null)
        {
            RotateUIElement(objectiveLoc);
        }
        
        if(passengerLoc != null)
        {
            RotateUIElement(passengerLoc);
        }
    }
}
