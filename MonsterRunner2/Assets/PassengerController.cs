using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    public int gunType;

    public DemoPlayer playerscript;
    public QuestGiver questgiver;
    public GameObject idleVFX;
    public GameObject PickupVFX;
    public GameObject passengerDestination;
    public ObjectiveIndicator arrow;
    public GunSystem gunSystem;
    public missionManagerScript missionmanager;

    
    public bool pickedUp;
    private float moveSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        questgiver = GetComponentInChildren<QuestGiver>();
        arrow.UpdateObjective(0, this.transform);
        passengerDestination = questgiver.destination;
    }

    void TriggerHouse(bool trigger)
    {
        
        HouseScript selectedHouse = passengerDestination.GetComponentInChildren<HouseScript>();
        if (trigger)
        {
            selectedHouse.TurnOnVFX();
            arrow.UpdateObjective(2, selectedHouse.gameObject.transform);
        }
        else
        {
            selectedHouse.CreateReachedVFX();
            selectedHouse.TurnOffVFX();
            arrow.UpdateObjective(3, null);
        }
    }

   
    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
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
                playerscript.RestoreHealth();
                TriggerHouse(false);
                gunSystem.UpdateGunInfo(0);
                DestroyPassenger();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gunSystem.UpdateGunInfo(gunType);
            TriggerHouse(true);
            pickedUp = true;
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
            transform.SetParent(other.transform);
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                }
                arrow.UpdateObjective(1, null);
            gameObject.SetActive(false);
        }
    }


    public void DestroyPassenger()
    {
        missionmanager.passengerCount--;
        Destroy(gameObject);
    }
}
