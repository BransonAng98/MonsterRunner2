using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableWeapon", menuName = "ScriptableObjects/Weapons")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType { Pistol, Rifle, MachineGun }

    public WeaponType type;
    public float fireRate;
    public float maxOffsetDistance;
    public float weaponRange;
    public float reloadTime;
    public int magzineSize;

}