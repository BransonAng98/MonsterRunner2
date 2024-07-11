using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshChasePlayer : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    [SerializeField] public Transform target;
    private float updateInterval = 0.2f;
    private float intervalTimer = 0f;

    public EnemyCarAI enemyCarAIScript; // Reference to the EnemyCarAI script
    public float switchDistance;  // Distance at which to switch scripts

    [SerializeField] private float distancetoPlayer;

    // Start is called before the first frame update
    void Start()
    {
      
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyCarAIScript.enabled = false;

       
        
    }

    // FixedUpdate is called at a fixed interval and is used for physics updates
    void FixedUpdate()
    {
        ChasePlayer();
    }
    private void Update()
    {
        distancetoPlayer = Vector3.Distance(transform.position, target.position);
    }
    private void ChasePlayer()
    {
        if (target != null)
        {
            // Update the interval timer
            intervalTimer += Time.deltaTime;

            // Check if it's time to update the path
            if (intervalTimer >= updateInterval)
            {
                // Set the destination to the player's current position
                navMeshAgent.SetDestination(target.position);

                // Reset the interval timer
                intervalTimer = 0f;
            }

            // Check the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= switchDistance)
            {
                // Enable the EnemyCarAI script and disable this script
                enemyCarAIScript.enabled = true;
                this.enabled = false;
                navMeshAgent.enabled = false;
            }
        }
    }
}