using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SelfBuff : AbilitySO
{
    public enum BuffType
    {
        Barrier,
        Damage,
        Speed
    }

    public BuffType buffType;

    public override void LevelUpSkill(int abilityLvl)
    {

    }

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
