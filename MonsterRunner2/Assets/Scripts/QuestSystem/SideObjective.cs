using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class SideObjective
{
    public bool isActive;
    public string title;
    public float goldReward;

    public SideObjectiveGoal goal;

    public void CompleteSideObjective()
    {
        isActive = false;
        Debug.Log(title + "was Completed");
    }

    public void goldRewardAmt()
    {
        float[] possiblegoldAMT = { 60f, 70f, 80f, 90f, 100f };
        goldReward = possiblegoldAMT[Random.Range(0, possiblegoldAMT.Length)];
    }
}
