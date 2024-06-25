using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractBoosterScript : MonoBehaviour
{
    public int contractID;
    public ParticleSystem particles;
    public float respawnTime;
    public bool startRespawn;
    public Collider coinCollider;
    public List<GameObject> bodies = new List<GameObject>();
    private float respawnTimeHolder;

    // Start is called before the first frame update
    void Start()
    {
        respawnTimeHolder = respawnTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Activate particles here
            ActiveEffect(contractID);
            DeactiveObject();
            Instantiate(particles, transform.position, transform.rotation);
            Debug.Log("Contracts are now slightly easier");
        }
    }

    void DeactiveObject()
    {
        foreach(GameObject entities in bodies)
        {
            entities.SetActive(false);
        }
        coinCollider.enabled = false;
    }

    void ReactiveObject()
    {
        foreach (GameObject entities in bodies)
        {
            entities.SetActive(true);
        }
        coinCollider.enabled = true;
    }

    void ActiveEffect(int id)
    {
        switch (id)
        {
            //Affecting survive missions
            case 0:
                Debug.Log("Survive timer is reduced");
                break;
            //Affect bounty missions
            case 1:
                Debug.Log("Number of enemies is reduced");
                break;
            //Affect drop-off missions
            case 2:
                Debug.Log("Arrow is displayed");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startRespawn)
        {
            if (respawnTime > 0f)
            {
                respawnTime -= Time.deltaTime;
            }

            else
            {
                ReactiveObject();
                startRespawn = false;
                respawnTime = respawnTimeHolder;
            }
        }
    }
}
