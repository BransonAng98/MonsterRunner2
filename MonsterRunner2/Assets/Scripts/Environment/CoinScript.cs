using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    public PlayerDataSO playerData;
    public int money;
    public ParticleSystem particles;
    public float respawnTime;
    public bool startRespawn;
    public Collider coinCollider;
    public MeshRenderer coinRenderer;
    public Audiomanager audiomanagerScript;

    private float respawnTimeHolder;
    // Start is called before the first frame update
    void Start()
    {
        GameObject audioManagerObject = GameObject.Find("SoundManager");
        if (audioManagerObject != null)
        {
            audiomanagerScript = audioManagerObject.GetComponent<Audiomanager>();
        }
        respawnTimeHolder = respawnTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audiomanagerScript.playCoinPickup();
            //Activate particles here
            playerData.moneyAccumulatedInGame += money;
            Instantiate(particles, transform.position, transform.rotation);
            DeactiveObject();
            startRespawn = true;
        }
    }

    void DeactiveObject()
    {
        coinRenderer.enabled = false;
        coinCollider.enabled = false;
    }

    void ReactiveObject()
    {
        coinRenderer.enabled = true;
        coinCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (startRespawn)
        {
            if(respawnTime > 0f)
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
