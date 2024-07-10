using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRespawner : MonoBehaviour
{
    //[SerializeField] private int respawnTime = 5;
    public GameObject DeathVFX;
    public MeshRenderer Renderer;
    public Collider entityCollider;
    [SerializeField] private bool isDead;
    private Transform playerPos;
    private float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, playerPos.position);

        if(distanceToPlayer >= 100f && isDead == true)
        {
            Respawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !isDead || collision.gameObject.tag == "Enemy" && !isDead)
        {
            Instantiate(DeathVFX, transform.position, Quaternion.identity);
            Renderer.enabled = false;
            entityCollider.enabled = false;
            isDead = true;
           
        }
    }

    private void Respawn()
    {
        
        Renderer.enabled = true;
        entityCollider.enabled = true;
        isDead = false;
    }
}