using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GunSystem : MonoBehaviour
{
    public enum GunType
    {
        nothing,
        pistol,
        shotgun,
    }

    [Serializable]
    public struct Gun
    {
        public WeaponSO weaponData;
        public GunType gunType;
        public Sprite gunImage;
    }

    public List<Gun> guns;
    public Image gunIcon;
    public WeaponSO currentWeaponData;
    public WeaponScript weaponScript;

    private void Start()
    {
        weaponScript = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponScript>();
        gunIcon.gameObject.SetActive(false);
    }

    public void UpdateGunInfo(int data)
    {
       switch (data)
        {
            case 0:
                weaponScript.enabled = false;
                weaponScript.weaponData = null;
                currentWeaponData = null;
                gunIcon.sprite = null;
                gunIcon.gameObject.SetActive(false);
                break;

            case 1:
                gunIcon.gameObject.SetActive(true);
                weaponScript.enabled = true;
                weaponScript.weaponData = guns[1].weaponData;
                weaponScript.AssignValues();
                gunIcon.sprite = guns[1].gunImage;
                break;
        }
    }
}
