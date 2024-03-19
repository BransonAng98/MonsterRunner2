using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvoCollision : MonoBehaviour
{
    public bool isDestroyed;
    public bool isBox;
    public string layerData;
    public MeshRenderer renderer;
    public Collider collider;
    public float distanceThreshold;

    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        layerData = this.gameObject.layer.ToString();
        renderer = GetComponent<MeshRenderer>();

        if (isBox)
        {
            collider = GetComponent<BoxCollider>();
        }
        else
        {
            collider = GetComponent<MeshCollider>();
        }

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void Collided()
    {
        isDestroyed = true;
        gameObject.layer = LayerMask.NameToLayer("Untouchable");
        GetComponent<Collider>().isTrigger = true;
        renderer.enabled = false;
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
                gameObject.layer = LayerMask.NameToLayer(layerData);
                GetComponent<Collider>().isTrigger = false;
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
