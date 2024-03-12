using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveIndicator : MonoBehaviour
{
    public Image arrowImage;
    public float rotationSpeed;

    [SerializeField] Transform objectiveLoc;
    [SerializeField] Transform playerTransform;
    [SerializeField] bool showArrow;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        arrowImage.GetComponent<Image>();
        arrowImage.enabled = false;
    }

    //Trigger the arrow on or off
    public void UpdateObjective(bool active, Transform objLoc)
    {
        if (active)
        {
            objectiveLoc = objLoc;
            arrowImage.enabled = true;
        }

        else
        {
            arrowImage.enabled = false;
        }
    }

    void RotateAroundPlayer()
    {
        // Get the direction from the player to the objective
        Vector3 directionToObjective = (objectiveLoc.position - playerTransform.position).normalized;

        // Calculate the angle between the forward direction of the arrow and the direction to the objective
        float angle = Mathf.Atan2(directionToObjective.y, directionToObjective.x) * Mathf.Rad2Deg - 90f;

        // Create a Quaternion representing the rotation around the Z-axis
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Smoothly rotate the arrow towards the objective
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (arrowImage.isActiveAndEnabled)
        {
            RotateAroundPlayer();
        }

        else { return; }
    }
}
