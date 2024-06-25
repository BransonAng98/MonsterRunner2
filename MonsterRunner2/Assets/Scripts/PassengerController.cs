using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    public DemoPlayer playerscript;
    public GameObject idleVFX;
    public GameObject PickupVFX;
    public GameObject passengerDestination;

    public QuestGiver questgiverScript;
    public missionManagerScript missionmanager;
    public ScoreManagerScript scoreManager;

    public bool pickedUp;
    private float moveSpeed = 30f;

    [SerializeField] private int passengerType;

    // Start is called before the first frame update
    void Start()
    {
        AssignPassengerType();
      
        passengerDestination = questgiverScript.destination;
    }

    // Update is called once per frame
    void Update()
    {
            // Check if passenger has reached destination
            float distanceToDestination = Vector3.Distance(transform.position, passengerDestination.transform.position);
            if (distanceToDestination <= 20f) // Adjust the threshold as needed
            {
              
                // Unparent from other.transform
                transform.SetParent(null);

                // Set all children to active
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
                Debug.Log("ReachedHome");
                // Calculate direction towards the destination
                Vector3 direction = (passengerDestination.transform.position - transform.position).normalized;

                // Move towards the destination
                transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

                // Rotate towards the direction of movement
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }
                //TriggerHouse(false);
                
                DestroyPassenger();
            }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetQuestTypeBasedOnPassengerType();
            missionmanager.DestroyOtherPassengers(gameObject);
            questgiverScript.SpawnEnemies();
            //TriggerHouse(true);
            
            idleVFX.SetActive(false);
            Instantiate(PickupVFX, transform.position, Quaternion.identity);
            
            Collider[] passengerCollider = GetComponentsInChildren<Collider>();
            if (passengerCollider != null)
            {
                foreach (Collider collider in passengerCollider)
                {
                    collider.enabled = false;
                }
            }
            Debug.Log("PassengerPickedUp");
            transform.SetParent(other.transform);
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            foreach (Transform child in transform)
            {
                pickedUp = true;
                child.gameObject.SetActive(false);
            }
        }
    }

    private void AssignPassengerType()
    {
        passengerType = Random.Range(1, 4); // Assign a random passenger type between 1 and 3
    }

    private void SetQuestTypeBasedOnPassengerType()
    {
        switch (passengerType)
        {
            case 1:
                questgiverScript.quest.goal.goaltype = "Kill";
                questgiverScript.RunGoalType("Kill");
                Debug.Log("BeingRun1");
                break;
            case 2:
                questgiverScript.quest.goal.goaltype = "Reach";
                questgiverScript.RunGoalType("Reach");
                Debug.Log("BeingRun2");
                break;
            case 3:
                questgiverScript.quest.goal.goaltype = "Survive";
                questgiverScript.RunGoalType("Survive");
                questgiverScript.countdownStart = true;
                Debug.Log("BeingRun3");
                break;
            default:
                questgiverScript.quest.goal.goaltype = "None";
                break;
        }
    }

    public void DestroyPassenger()
    {
        missionmanager.passengerCount--;
        Destroy(gameObject);
    }
}