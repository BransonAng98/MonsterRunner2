using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    private ObjectFader objectfaderScript;
    public GameObject player;

    void Start()
    {
        // Initialization if needed
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);

            RaycastHit[] hits = Physics.RaycastAll(ray);

            bool playerHit = false;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider == null)
                    continue;

                if (hit.collider.gameObject == player)
                {
                    playerHit = true;

                    if (objectfaderScript != null)
                    {
                        objectfaderScript.DoFade = false;
                    }
                }
                else
                {
                    objectfaderScript = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if (objectfaderScript != null)
                    {
                        objectfaderScript.DoFade = true;
                    }
                }
            }

            // If the player was not hit, ensure the objectfaderScript is reset
            if (!playerHit && objectfaderScript != null)
            {
                objectfaderScript.DoFade = false;
                objectfaderScript = null;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);

            // Set the color of the gizmo
            Gizmos.color = Color.red;

            // Draw the ray
            Gizmos.DrawRay(ray.origin, ray.direction * Vector3.Distance(transform.position, player.transform.position));
        }
    }
}