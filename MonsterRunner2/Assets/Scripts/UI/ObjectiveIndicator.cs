using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveIndicator : MonoBehaviour
{
    public MeshRenderer arrowImage;
    public float rotationSpeed;

    [SerializeField] Transform objectiveLoc;
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
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(goal.position - transform.position), rotationSpeed * Time.deltaTime);
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
