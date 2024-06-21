using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JsonSystem : MonoBehaviour
{
    //public static JsonSystem Instance { get; private set; }

    public List<PlayerSO> allVehicleDataList;
    public PlayerCarDisplay selectedVehicle;
    public PlayerDataSO playerInfoData;
    public int activeID;
    public float playerCurrency;

    private string vehicleDataFilePath;
    private string playerDataFilePath;

    private void Awake()
    {
        vehicleDataFilePath = Path.Combine(Application.persistentDataPath, "VehicleDataFile.json");
        playerDataFilePath = Path.Combine(Application.persistentDataPath, "PlayerDataFile.json");
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);

        if (!File.Exists(playerDataFilePath) && !File.Exists(vehicleDataFilePath))
        {
            //Creating a new data for playerSO and vehicleSos and saving it to Json
            CreateFreshJsonFile();
        }
    }

    public void SaveToJson(int function)
    {
        switch (function)
        {
            //Saving car data
            case 0:

                // Creating and populating the list of VehicleData
                List<VehicleData> vehicleDataList = new List<VehicleData>();

                // Populating the list with multiple VehicleData objects
                foreach (var vehicleData in allVehicleDataList)
                {
                    VehicleData carData = new VehicleData();
                    carData.vehicleName = vehicleData.vehicleName;
                    carData.vehicleID = vehicleData.vehicleID;
                    carData.speed = vehicleData.maxSpeed;
                    carData.ability1Level = vehicleData.ability1Level;
                    carData.ability2Level = vehicleData.ability2Level;

                    vehicleDataList.Add(carData);
                }

                // Convert the list to an array
                VehicleData[] vehicleDataArray = vehicleDataList.ToArray();

                // Serializing the array to JSON
                string carJson = JsonHelper.ToJson(vehicleDataArray, true);
                File.WriteAllText(vehicleDataFilePath, carJson);
                break;

            //Saving selected car data
            case 1:
                PlayerInfoData playerData = new PlayerInfoData();
                playerData.selectedVehicleID = playerInfoData.selectedVehicleID;
                playerData.money = playerInfoData.money;
                playerData.gems = playerInfoData.gems;
                playerData.hasPlayedTutorial = playerInfoData.hasPlayedTutorial;
                string playerJson = JsonUtility.ToJson(playerData, true);
                File.WriteAllText(playerDataFilePath, playerJson);
                break;
        }
    }

    void CreateFreshJsonFile()
    {
        // Creating and populating the list of VehicleData
        List<VehicleData> vehicleDataList = new List<VehicleData>();

        // Populating the list with multiple VehicleData objects
        foreach (var vehicleData in allVehicleDataList)
        {
            VehicleData carData = new VehicleData();
            carData.vehicleName = vehicleData.vehicleName;
            carData.vehicleID = vehicleData.vehicleID;
            carData.speed = vehicleData.maxSpeed;
            carData.ability1Level = vehicleData.ability1Level;
            carData.ability2Level = vehicleData.ability2Level;

            vehicleDataList.Add(carData);
        }

        // Convert the list to an array
        VehicleData[] vehicleDataArray = vehicleDataList.ToArray();

        // Serializing the array to JSON
        string carJson = JsonHelper.ToJson(vehicleDataArray, true);
        File.WriteAllText(vehicleDataFilePath, carJson);

        PlayerInfoData playerData = new PlayerInfoData();
        playerData.selectedVehicleID = 0;
        playerData.money = 0;
        playerData.gems = 0;
        playerData.hasPlayedTutorial = false;
        string playerJson = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(playerDataFilePath, playerJson);
    }

    public void LoadFromJson()
    {
        if (File.Exists(vehicleDataFilePath))
        {
            string json = File.ReadAllText(vehicleDataFilePath);
            VehicleData[] dataArray = JsonHelper.FromJson<VehicleData>(json);
            allVehicleDataList = new List<PlayerSO>();

            foreach (var data in dataArray)
            {
                PlayerSO vehicleData = ScriptableObject.CreateInstance<PlayerSO>();
                vehicleData.vehicleName = data.vehicleName;
                vehicleData.vehicleID = data.vehicleID;
                vehicleData.maxSpeed = data.speed;
                vehicleData.ability1Level = data.ability1Level;
                vehicleData.ability2Level = data.ability2Level;

                allVehicleDataList.Add(vehicleData);
            }
        }
        else
        {
            Debug.LogError("Unable to find vehicle file path");
            SaveToJson(0);
        }

        if (File.Exists(playerDataFilePath))
        {
            string playerJson = File.ReadAllText(playerDataFilePath);
            PlayerInfoData playerData = JsonUtility.FromJson<PlayerInfoData>(playerJson);
            playerInfoData.selectedVehicleID = playerData.selectedVehicleID;
            playerInfoData.money = playerData.money;
            playerInfoData.gems = playerData.gems;
            playerInfoData.hasPlayedTutorial = playerData.hasPlayedTutorial;
        }
        else
        {
            Debug.LogError("Unable to find player file path");
            SaveToJson(1);
        }
    }
}

// Helper class for serializing and deserializing arrays
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
