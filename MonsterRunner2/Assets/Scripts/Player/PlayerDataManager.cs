using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    [Serializable]
    public struct Vehicle
    {
        public MeshFilter vehicleBody;
        public int vehicleID;
        public AbilitySO ability1;
        public AbilitySO ability2;
    }

    public List<Vehicle> vehicles;
    public int vehicleID;
    public int abilityUnlocked;
    public bool secondUnlocked;
    public MeshFilter body;
    public AbilitySO ab1;
    public AbilitySO ab2;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate selected Vehicle data into the game scene
        PlayerPrefs.SetFloat("Vehicle", vehicleID);
        switch (vehicleID)
        {
            //Checking if ability is 0 or 1. 0 means the ability hasn't been unlocked while 1 means the other
            case 0:
                PlayerPrefs.SetInt("V1Ability", abilityUnlocked);
                break;

            case 1:
                PlayerPrefs.SetInt("V1Ability", abilityUnlocked);
                PlayerPrefs.SetInt("V2Ability", abilityUnlocked);
                break;
        }

        //Checking along the list of registered vehicles in the lists
        foreach (Vehicle vehicle in vehicles)
        {
            if (vehicle.vehicleID == vehicleID)
            {
                //When the appropriate vehicle is located store the model and ability 1 data into this script
                body = vehicle.vehicleBody;
                ab1 = vehicle.ability1;

                //Future check to see if the second ability has been unlocked or not
                if (abilityUnlocked == 1)
                {
                    ab2 = vehicle.ability2;
                }
                else
                {
                    return;
                }
            }
        }
    }
}
