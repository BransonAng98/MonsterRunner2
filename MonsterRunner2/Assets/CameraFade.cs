using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    [SerializeField] private List<ObjectFader> fadingObjects = new List<ObjectFader>();
    public GameObject player;
    public int numberOfRays = 10; // Number of rays to cast in the arc
    public float arcAngle = 45f; // Total angle of the arc in degrees
    public float rayDistance = 10f; // Distance of the rays

    void Update()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;

            // Calculate direction from camera to player
            Vector3 dir = playerPosition - transform.position;

            // Calculate rotation to face player
            Quaternion rotation = Quaternion.LookRotation(dir);

            // Calculate angle step for rays
            float angleStep = arcAngle / (numberOfRays - 1);
            float startAngle = -arcAngle / 2;

            HashSet<ObjectFader> hitObjects = new HashSet<ObjectFader>();

            // Cast rays in an arc around player
            for (int i = 0; i < numberOfRays; i++)
            {
                float currentAngle = startAngle + i * angleStep;
                Quaternion rayRotation = rotation * Quaternion.Euler(0, currentAngle, 0);
                Vector3 rayDirection = rayRotation * Vector3.forward;

                // Ray origin is from player position
                Ray ray = new Ray(playerPosition, rayDirection);
                RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider != null && hit.collider.gameObject != player)
                    {
                        ObjectFader objectFaderScript = hit.collider.gameObject.GetComponent<ObjectFader>();
                        if (objectFaderScript != null)
                        {
                            objectFaderScript.DoFade = true;
                            hitObjects.Add(objectFaderScript);
                        }
                    }
                }
            }

            // Disable fading for objects that were faded but are no longer hit
            foreach (ObjectFader fader in fadingObjects)
            {
                if (!hitObjects.Contains(fader))
                {
                    fader.DoFade = false;
                }
            }

            fadingObjects = new List<ObjectFader>(hitObjects);
        }
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            Quaternion rotation = Quaternion.LookRotation(playerPosition - transform.position);
            float angleStep = arcAngle / (numberOfRays - 1);
            float startAngle = -arcAngle / 2;

            Gizmos.color = Color.red;

            for (int i = 0; i < numberOfRays; i++)
            {
                float currentAngle = startAngle + i * angleStep;
                Quaternion rayRotation = rotation * Quaternion.Euler(0, currentAngle, 0);
                Vector3 rayDirection = rayRotation * Vector3.forward;
                Gizmos.DrawRay(playerPosition, rayDirection * rayDistance);
            }
        }
    }
}