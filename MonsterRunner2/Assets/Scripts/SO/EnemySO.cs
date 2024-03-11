using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemySO : ScriptableObject
{
    public float health;
    public float speed;
    public float attackCD;
    public float attackDmg;
    public float weight;
}
