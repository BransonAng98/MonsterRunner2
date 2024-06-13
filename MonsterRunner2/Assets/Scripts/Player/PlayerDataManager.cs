using System;
using System.IO;
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
    public int ab1;
    public int ab2;

    public DemoPlayer playerData;

    //Transfer data to player
    public GameObject entityHolder;
    public PlayerAbilityManager playerAbManager;
    public PlayerDataManager playerDataManager;
    public PlayerInfoData playerInfoData;
    public Joystick joystick;
    public QuestDialogueManager questDManager;
    public GameMenuManager menuManager;

    //Scriptst that require player data
    public CinemachineVirtualCamera mainCam;
 
    public missionManagerScript missionManager;
    public PlayerAbilityManager abilityManager;
    public AbilityTokenManager tokenManager;

    private string vehicleDataFilePath;
    private string playerDataFilePath;

    [SerializeField] public int ability2Level;

    private void Awake()
    {
        vehicleDataFilePath = Application.dataPath + "/VehicleDataFile.json";
        playerDataFilePath = Application.dataPath + "/PlayerDataFile.json";
        LoadFromJson();

        //Checking along the list of registered vehicles in the lists
        foreach (Vehicle vehicle in vehicles)
        {
            if (vehicle.vehicleID == vehicleID)
            {
                GameObject playerCar = Instantiate(vehicle.playerPrefab, new Vector3(0, 2, 0), Quaternion.identity);
                playerCar.transform.SetParent(entityHolder.transform);
                DemoPlayer playerData = playerCar.GetComponent<DemoPlayer>();

                mainCam.Follow = playerCar.transform;
             
                missionManager.demoPlayer = playerData;
                missionManager.objectiveIndicator = playerData.questIndicator;
                missionManager.player = playerCar;
                abilityManager.player = playerData;

                playerData.abilityManager = playerAbManager;
                playerData.playerDataManager = playerDataManager;
                playerData.joystick = joystick;
                playerData.questdialogueScript = questDManager;
                playerData.menuManager = menuManager;

                if(ab2 != 0)
                {
                    tokenManager.spawnType2 = true;
                }

                else
                {
                    tokenManager.spawnType2 = false;
                }
            }
        }
    }

    public void LoadFromJson()
    {
        if (File.Exists(vehicleDataFilePath))
        {
            string json = File.ReadAllText(vehicleDataFilePath);
            VehicleData[] dataArray = JsonHelper.FromJson<VehicleData>(json);

            foreach (var data in dataArray)
            {
                PlayerSO vehicleData = ScriptableObject.CreateInstance<PlayerSO>();
                vehicleData.vehicleName = data.vehicleName;
                vehicleData.vehicleID = data.vehicleID;
                vehicleData.maxSpeed = data.speed;
                ab1 = data.ability1Level;
                ab2 = data.ability2Level;
            }
        }
        else
        {
            Debug.Log("VehicleDataFile.json not found");
            return;
        }

        if (File.Exists(playerDataFilePath))
        {
            string playerJson = File.ReadAllText(playerDataFilePath);
            playerInfoData = JsonUtility.FromJson<PlayerInfoData>(playerJson);
            if (playerInfoData != null)
            {
                vehicleID = playerInfoData.selectedVehicleID;
            }
        }
        else
        {
            Debug.Log("PlayerDataFile.json not found");
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
