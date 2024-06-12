using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonSystem : MonoBehaviour
{
    //public static JsonSystem Instance { get; private set; }

    public PlayerSO selectedVehicleData;

    private void Awake()
    {
        //if(Instance != null && Instance != this)
        //{
        //    Destroy(this);
        //}
        //else
        //{
        //    Instance = this;
        //}

        //DontDestroyOnLoad(this.gameObject);
    }

    public void SaveToJson()
    {
        //Assigning currentdata to be saved
        VehicleData data = new VehicleData();
        data.vehicleName = selectedVehicleData.vehicleName;
        data.vehicleID = selectedVehicleData.vehicleID;
        data.speed = selectedVehicleData.maxSpeed;
        data.ability1Level = selectedVehicleData.ability1Level;
        data.ability2Level = selectedVehicleData.ability2Level;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.dataPath + "/VehicleDataFile.json", json);
    }

    public void LoadFromJson()
    {
        string json = File.ReadAllText(Application.dataPath + "/VehicleDataFile.json");
        VehicleData data = JsonUtility.FromJson<VehicleData>(json);
    }
}
