using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public string title;
  
    public QuestGoal goal;

    public void Complete()
    {
        isActive = false;
        Debug.Log(title + "was Completed");
    }
}
