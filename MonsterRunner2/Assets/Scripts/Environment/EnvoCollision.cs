using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvoCollision : MonoBehaviour
{
    public bool isDestroyed;
    public bool isBox;
    public MeshRenderer renderer;
    public Collider collider;
    public float distanceThreshold;
    public ParticleSystem collisionVFX;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();

    }

    public void Collided()
    {
        collider.enabled = false;
        collisionVFX.Play();
        isDestroyed = true;
        renderer.enabled = false;
        player.GetComponent<DemoPlayer>().isTriggered = false;
    }

    private bool DistanceBetween()
    {
        float distance = Vector3.Distance(this.transform.position, player.position);
        return distance >= distanceThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed)
        {
            if (DistanceBetween())
            {
                //Player is x distance away of this gameobject
                collider.enabled = true;
                renderer.enabled = true;
            }

            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
}
