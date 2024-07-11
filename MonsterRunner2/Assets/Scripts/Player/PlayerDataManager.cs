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
    public int ab1;
    public int ab2;

    public DemoPlayer playerData;

    //Player spawn position
    public List<Transform> playerRandomSpawnPosition = new List<Transform>();

    //Transfer data to player
    public GameObject entityHolder;
    public PlayerAbilityManager playerAbManager;
    public PlayerDataManager playerDataManager;
    public PlayerInfoData playerInfoData;
    public Joystick joystick;
    public QuestDialogueManager questDManager;
    public TutorialManager tutorialManager;
    public GameMenuManager menuManager;
    public EnemySpawner enemySpawnerScript;
    public DetectionBar detectionBar;
    public LineRenderer lineRenderer;
    public IDriveTokenManager driveTokenManager;
    public WaterTrigger waterTrigger;

    //Scriptst that require player data
    public CinemachineVirtualCamera mainCam;
 
    public missionManagerScript missionManager;
    public PlayerAbilityManager abilityManager;
    public AbilityTokenManager tokenManager;
    public CameraFade cameraFaderManager;
    public QuestGiver questgiverManager;
    public Audiomanager audiomanagerScript;
    private string vehicleDataFilePath;
    private string playerDataFilePath;

    [SerializeField] public int ability2Level;
    public PlayerDataSO playerDataSO;
    private void Awake()
    {
        vehicleID = playerDataSO.selectedVehicleID;
        Debug.Log(vehicleID);

        // Check along the list of registered vehicles in the list
        foreach (Vehicle vehicle in vehicles)
        {
            if (vehicle.vehicleID == vehicleID)
            {
                // Select a random spawn position
                if (playerRandomSpawnPosition.Count > 0)
                {
                    int randomIndex = UnityEngine.Random.Range(0, playerRandomSpawnPosition.Count);
                    Transform spawnPosition = playerRandomSpawnPosition[randomIndex];

                    GameObject playerCar = Instantiate(vehicle.playerPrefab, spawnPosition.position, spawnPosition.rotation);
                    playerCar.transform.SetParent(entityHolder.transform);
                    playerData = playerCar.GetComponent<DemoPlayer>();
                    ObjectiveIndicator objectiveData = playerCar.GetComponentInChildren<ObjectiveIndicator>();
                    objectiveData.path = lineRenderer;
                    objectiveData.playerTransform = playerData.transform;
                    ab2 = playerData.playerData.ability2Level;
                    driveTokenManager.playerData = playerData;
                    questDManager.player = playerData;
                    mainCam.Follow = playerCar.transform;
                    waterTrigger.demoPlayer = playerData;
                    missionManager.demoPlayer = playerData;
                    missionManager.objectiveIndicator = playerData.questIndicator;
                    missionManager.player = playerCar;
                    tutorialManager.player = playerData;
                    abilityManager.player = playerData;
                    detectionBar.player = playerData;
                    playerData.audiomanagerScript = audiomanagerScript;
                    playerData.abilityManager = playerAbManager;
                    playerData.playerDataManager = playerDataManager;
                    playerData.joystick = joystick;
                    playerData.questdialogueScript = questDManager;
                    playerData.menuManager = menuManager;
                    questgiverManager.player = playerData;
                    enemySpawnerScript.playerPos = playerData.transform;
                    enemySpawnerScript.playerData = playerData;
                    cameraFaderManager.player = playerData.gameObject;
                    if (ab2 > 0)
                    {
                        tokenManager.spawnType2 = true;
                    }
                    else
                    {
                        tokenManager.spawnType2 = false;
                    }
                }
                else
                {
                    Debug.LogError("No spawn positions available in playerRandomSpawnPosition list.");
                }

                break;
            }
        }
    }
}
