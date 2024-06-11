using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class OilSlick : AbilitySO
{
    public float trailTimer;
    public float trailinterval;
    public TrailRenderer trailRender;
    public MeshCollider mesh;
    public GameObject gameObj;

    private bool isTriggered;
    public void ExternalVariable(GameObject obj)
    {
        gameObj = obj;
        trailRender = gameObj.GetComponent<TrailRenderer>();
        trailRender.enabled = false;
        mesh = gameObj.GetComponent<MeshCollider>();
    }

    public override void AssignVariables(Transform origin, Transform player)
    {

    }

    void GenerateMeshCollider()
    {
        MeshCollider meshCollider = mesh;

        if(meshCollider != null)
        {
            meshCollider = gameObj.AddComponent<MeshCollider>();
        }

        Mesh newMesh = new Mesh();
        trailRender.BakeMesh(newMesh, true);
        meshCollider.sharedMesh = newMesh;
    }

    public override void Activate()
    {
        trailTimer += Time.deltaTime;
        if(trailTimer >= trailinterval)
        {
            if (!isTriggered)
            {
                trailRender.enabled = true;
                GenerateMeshCollider();
                isTriggered = true;
            }
        }
    }

    public override void Deactive()
    {

    }
}
