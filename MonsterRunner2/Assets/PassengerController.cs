using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    public QuestGiver questgiver;
    public GameObject idleVFX;
    public GameObject PickupVFX;
    public missionManagerScript missionmanager;
    public bool Pickedup;
    public GameObject passengerDestination;
    private HouseScript houseScript;
    private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        missionmanager = GameObject.Find("MissionManager").GetComponent<missionManagerScript>();
        questgiver = GetComponentInChildren<QuestGiver>();
       
    }

    void TriggerHouse(bool trigger)
    {
        passengerDestination = questgiver.destination;
        HouseScript selectedHouse = passengerDestination.GetComponent<HouseScript>();
        if (trigger)
        {
            selectedHouse.TurnOnVFX();
        }
        else
        {
            selectedHouse.TurnOffVFX();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Pickedup)
        {
            Debug.Log("Take Me Home");
            
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

            // Check if passenger has reached destination
            float distanceToDestination = Vector3.Distance(transform.position, passengerDestination.transform.position);
            if (distanceToDestination < 0.5f) // Adjust the threshold as needed
            {
                Debug.Log("ReachedHome");
                TriggerHouse(false);
                DestroyPassenger();
            }
        }
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag==("Player"))
        {
            TriggerHouse(true);
            Pickedup = true;
            idleVFX.SetActive(false);
            Instantiate(PickupVFX, transform.position, Quaternion.identity);
            questgiver.AcceptQuest();
            Collider[] passengerCollider = GetComponentsInChildren<Collider>();
            if (passengerCollider != null)
            {

                foreach (Collider collider in passengerCollider)
                {
                    collider.enabled = false;
                }
            }
            Debug.Log("PassengerPickedUp");
            transform.SetParent(collision.transform);
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            gameObject.SetActive(false);
        }

      
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    public void DestroyPassenger()
    {
        missionmanager.passengerCount--;
        Destroy(gameObject);
    }
}
