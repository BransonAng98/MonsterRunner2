using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AOESkill : AbilitySO
{
    public Transform playerObj;
    public Transform aoeOrigin;
    public GameObject particleSys;

    public float aoeDuration;
    public float aoeTimer;

    public override void AssignVariables(Transform origin, Transform player)
    {
        aoeOrigin = origin;
        playerObj = player;
        DemoPlayer playerData = playerObj.GetComponent<DemoPlayer>();

        particleSys = playerData.particleSystem;
        SlowScript ccSCript = particleSys.GetComponent<SlowScript>();
        ccSCript.ccDuration = aoeDuration;

        particleSys.SetActive(false);
    }

    public override void Activate()
    {
        if (!particleSys.activeSelf)
        {
            particleSys.SetActive(true);
        }
    }

    public override void Deactive()
    {
        particleSys.SetActive(false);
    }
}
