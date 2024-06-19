using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCarAI : MonoBehaviour
{
    public Transform targetPositionTranform;
    public enemyCarDriver carDriver;
    public GameObject detectionColliderObject; // Reference to the GameObject with the trigger collider

    private Vector3 targetPosition;
    private const float reachedTargetDistance = 1f;
    private const float stoppingDistance = 30f;
    private const float stoppingSpeed = 40f;
    private const float reverseDistance = 25f;
    private const float minSpeedToReverse = 15f;
    private const float avoidanceStrength = 1f; // Multiplier for avoidance steering
    private const float detectionRadius = 10f; // Radius for detecting obstacles

    [SerializeField] private bool isAvoiding;
    [SerializeField]private List<Collider> nearbyObstacles = new List<Collider>();

    private void Update()
    {
        SetTargetPosition(targetPositionTranform.position);

        float forwardAmount = 0f;
        float turnAmount = 0f;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > reachedTargetDistance)
        {
            // Still too far, keep going
            Vector3 dirToMovePosition = (targetPosition - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToMovePosition);

            if (dot > 0)
            {
                // Target in front
                forwardAmount = 1f;

                if (distanceToTarget < stoppingDistance && carDriver.GetSpeed() > stoppingSpeed)
                {
                    forwardAmount = -1f; // Slow down when too fast and close to the target
                }
            }
            else
            {
                // Target behind
                if (distanceToTarget > reverseDistance)
                {
                    // Too far to reverse
                    forwardAmount = 1f;
                }
                else
                {
                    forwardAmount = -1f;
                }
            }

            Vector3 avoidanceVector = CalculateAvoidanceVector();
            Vector3 combinedDirection = (dirToMovePosition + avoidanceVector).normalized;

            float angleToDir = Vector3.SignedAngle(transform.forward, combinedDirection, Vector3.up);
            if (Mathf.Abs(angleToDir) < 10f)
            {
                turnAmount = 0f; // Don't steer if angle is within -10 to 10 degrees
            }
            else
            {
                turnAmount = angleToDir > 0 ? 1f : -1f;
            }

            if (isAvoiding)
            {
                forwardAmount *= 0.5f; // Reduce speed when avoiding
            }
        }
        else
        {
            // Reached target
            forwardAmount = carDriver.GetSpeed() > minSpeedToReverse ? -1f : 0f;
            turnAmount = 0f;
        }

        carDriver.SetInputs(forwardAmount, turnAmount);
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            isAvoiding = true;
            if (!nearbyObstacles.Contains(other))
            {
                nearbyObstacles.Add(other);
            }
            Debug.Log("Avoiding: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            nearbyObstacles.Remove(other);
            if (nearbyObstacles.Count == 0)
            {
                isAvoiding = false;
            }
            Debug.Log("Stopped avoiding: " + other.name);
        }
    }

    private Vector3 CalculateAvoidanceVector()
    {
        Vector3 avoidanceVector = Vector3.zero;
        foreach (Collider obstacle in nearbyObstacles)
        {
            Vector3 directionAwayFromObstacle = (transform.position - obstacle.transform.position).normalized;
            float distanceToObstacle = Vector3.Distance(transform.position, obstacle.transform.position);
            float avoidanceForce = Mathf.Clamp01(detectionRadius - distanceToObstacle) / detectionRadius;
            avoidanceVector += directionAwayFromObstacle * avoidanceForce;
        }
        return avoidanceVector.normalized * avoidanceStrength;
    }
}