using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFade : MonoBehaviour
{
    [SerializeField]private List<ObjectFader> fadingObjects = new List<ObjectFader>();
    public GameObject player;

    void Update()
    {
        if (player != null)
        {
            Vector3 dir = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);

            RaycastHit[] hits = Physics.RaycastAll(ray);

            List<ObjectFader> hitObjects = new List<ObjectFader>();

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null && hit.collider.gameObject != player)
                {
                    ObjectFader objectfaderScript = hit.collider.gameObject.GetComponent<ObjectFader>();
                    if (objectfaderScript != null)
                    {
                        objectfaderScript.DoFade = true;
                        hitObjects.Add(objectfaderScript);
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

            fadingObjects = hitObjects;
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