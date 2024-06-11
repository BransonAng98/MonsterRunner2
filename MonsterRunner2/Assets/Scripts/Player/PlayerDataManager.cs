using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerDataManager : MonoBehaviour
{
    [Serializable]
    public struct Vehicle
    {
        public int vehicleID;
        public GameObject playerPrefab;
    }

    public List<Vehicle> vehicles;
    public int vehicleID;
    public int abilityUnlocked;
    public bool secondUnlocked;
    public Mesh body;
    public Material vehicleMat;
    public AbilitySO ab1;
    public AbilitySO ab2;

    public DemoPlayer playerData;

    //Transfer data to player
    public GameObject entityHolder;
    public PlayerAbilityManager playerAbManager;
    public PlayerDataManager playerDataManager;
    public Joystick joystick;
    public QuestDialogueManager questDManager;
    public GameMenuManager menuManager;

    //Scriptst that require player data
    public CinemachineVirtualCamera mainCam;
    public EnvironmentSpawner environmentSpawner;
    public missionManagerScript missionManager;
    public PlayerAbilityManager abilityManager;

    private void Awake()
    {
        //Checking along the list of registered vehicles in the lists
        foreach (Vehicle vehicle in vehicles)
        {
            if (vehicle.vehicleID == vehicleID)
            {
                GameObject playerCar = Instantiate(vehicle.playerPrefab, new Vector3(0, 2, 0), Quaternion.identity);
                playerCar.transform.SetParent(entityHolder.transform);
                DemoPlayer playerData = playerCar.GetComponent<DemoPlayer>();

                mainCam.Follow = playerCar.transform;
                environmentSpawner.playerData = playerData;
                environmentSpawner.playerPos = playerCar.transform;
                missionManager.demoPlayer = playerData;
                missionManager.objectiveIndicator = playerData.questIndicator;
                missionManager.player = playerCar;
                abilityManager.player = playerData;

                playerData.abilityManager = playerAbManager;
                playerData.playerDataManager = playerDataManager;
                playerData.joystick = joystick;
                playerData.questdialogueScript = questDManager;
                playerData.menuManager = menuManager;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate selected Vehicle data into the game scene
        //PlayerPrefs.SetFloat("Vehicle", vehicleID);
        //switch (vehicleID)
        //{
        //    //Checking if ability is 0 or 1. 0 means the ability hasn't been unlocked while 1 means the other
        //    case 0:
        //        PlayerPrefs.SetInt("V1Ability", abilityUnlocked);
        //        break;

        //    case 1:
        //        PlayerPrefs.SetInt("V1Ability", abilityUnlocked);
        //        PlayerPrefs.SetInt("V2Ability", abilityUnlocked);
        //        break;
        //}
    }
}
