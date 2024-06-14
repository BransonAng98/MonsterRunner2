using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class PlayerCarDisplay : MonoBehaviour
{
    [Serializable]
    public struct CarType
    {
        public string vehicleName;
        public int vehicleID;
        public PlayerSO vehicleData;
        public Mesh vehicleBody;
        public Material bodyMaterial;
    }

    public List<CarType> cars;
    public int selectedCarID;
    public int activateID;
    public List<GameObject> displayCars = new List<GameObject>();
    public TextMeshProUGUI upgradeCarDisplayName;
    public PlayerInfoData playerData;

    // Start is called before the first frame update
    void Start()
    {
        activateID = playerData.selectedVehicleID;
        selectedCarID = activateID;
        UpdateCarSkin(activateID);
    }

    public void UpdateCarSkin(int id)
    {
        foreach(GameObject dCars in displayCars)
        {
            MeshFilter mesh = dCars.GetComponent<MeshFilter>();
            mesh.mesh = cars[id].vehicleBody;

            MeshRenderer mat = dCars.GetComponent<MeshRenderer>();
            mat.material = cars[id].bodyMaterial;
        }

        upgradeCarDisplayName.text = cars[id].vehicleName;
    }
}
