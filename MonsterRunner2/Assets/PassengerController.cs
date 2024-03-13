using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour
{
    public QuestGiver questgiver;
    public GameObject idleVFX;
    public GameObject PickupVFX;
    // Start is called before the first frame update
    void Start()
    {
        questgiver = GetComponentInChildren<QuestGiver>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag==("Player"))
        {
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
}
