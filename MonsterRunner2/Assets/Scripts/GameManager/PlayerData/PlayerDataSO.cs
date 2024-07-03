using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerData")]
public class PlayerDataSO : ScriptableObject
{
    public int selectedVehicleID;
    public float money;
    public float moneyAccumulatedInGame;
    public int gems;
    public bool gameStart;
    public bool hasPlayedTutorial;

    public int contractCompleted;
    public int distanceTraveled;
}
