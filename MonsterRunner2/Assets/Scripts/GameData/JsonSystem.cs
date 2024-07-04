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

        if (!File.Exists(playerDataFilePath))
        {
            //Creating a new data for playerSO and vehicleSos and saving it to Json
            CreateFreshJsonFile();
            LoadFromJson();
        }
        else
        {
            Debug.Log("player data file loc is" + playerDataFilePath);
            LoadFromJson();
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
                    carData.speedRating = vehicleData.speedRating;
                    carData.accelRating = vehicleData.accelRating;

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
                playerData.contractCompleted = playerInfoData.contractCompleted;
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
            carData.speedRating = vehicleData.speedRating;
            carData.accelRating = vehicleData.accelRating;
            carData.ability1Level = 1;
            carData.ability2Level = 0;
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
        playerData.contractCompleted = 0;
        playerData.distanceTraveled = 0;
        string playerJson = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(playerDataFilePath, playerJson);
    }

    public void LoadFromJson()
    {
        if (File.Exists(vehicleDataFilePath))
        {
            string json = File.ReadAllText(vehicleDataFilePath);
            VehicleData[] dataArray = JsonHelper.FromJson<VehicleData>(json);

            foreach (var data in dataArray)
            {
                PlayerSO vehicleData = allVehicleDataList.Find(v => v.vehicleID == data.vehicleID);

                if (vehicleData != null)
                {
                    vehicleData.vehicleName = data.vehicleName;
                    vehicleData.maxSpeed = data.speed;
                    vehicleData.ability1Level = data.ability1Level;
                    vehicleData.ability2Level = data.ability2Level;
                    vehicleData.speedRating = data.speedRating;
                    vehicleData.accelRating = data.accelRating;
                }
                else
                {
                    // If vehicleData not found, create a new one
                    vehicleData = ScriptableObject.CreateInstance<PlayerSO>();
                    vehicleData.vehicleName = data.vehicleName;
                    vehicleData.vehicleID = data.vehicleID;
                    vehicleData.maxSpeed = data.speed;
                    vehicleData.speedRating = data.speedRating;
                    vehicleData.accelRating = data.accelRating;
                    vehicleData.ability1Level = 1;
                    vehicleData.ability2Level = 0;
                    allVehicleDataList.Add(vehicleData);
                }
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
            playerInfoData.contractCompleted = playerData.contractCompleted;
        }
        else
        {
            Debug.LogError("Unable to find player file path");
            SaveToJson(1);
        }
    }
    public void ResetGame()
    {
        if (File.Exists(vehicleDataFilePath))
        {
            File.Delete(vehicleDataFilePath);
        }
        else
        {
            Debug.LogError("Unable to find vehicle path");
        }


        if (File.Exists(playerDataFilePath))
        {
            File.Delete(playerDataFilePath);
        }
        else
        {
            Debug.LogError("Unable to find player path");
        }
        // Get the current scene name
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneName);
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
