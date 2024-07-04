using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/Player")]
public class PlayerSO : ScriptableObject
{
    public int speedRating;
    public int accelRating;

    public float health;
    public float maxSpeed;
    public float acceleration;
    public float decceleration;
    public float damage;
    public float crashResistance;

    public string vehicleName;
    public int vehicleID;
    public Mesh vehicleBody;
    public Material bodyMaterial;
    public AbilitySO ability1;
    public Sprite ability1Sprite;
    public int ability1Level;
    public AbilitySO ability2;
    public int ability2Level;
    public Sprite ability2Sprite;
}
