using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AOESkill : AbilitySO
{
    public Transform aoeOrigin;
    public ParticleSystem particleSys;

    public float aoeTimer;
    public float aoeInterval;

    [SerializeField] List<enemyCarDriver> enemyList = new List<enemyCarDriver>();

    public override void AssignVariables(Transform origin, Transform player)
    {

    }

    public override void Activate()
    {
       
    }

    public override void Deactive()
    {

    }
}
