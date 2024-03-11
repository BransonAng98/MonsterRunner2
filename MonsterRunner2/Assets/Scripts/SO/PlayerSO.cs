using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/Player")]
public class PlayerSO : ScriptableObject
{
    public float health;
    public float maxSpeed;
    public float acceleration;
    public float damage;
}
