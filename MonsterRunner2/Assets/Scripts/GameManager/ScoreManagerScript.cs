using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManagerScript : MonoBehaviour
{
    public float goldEarned;
    public int missionsCompleted;
    public float timeSurvived;
    public float stopwatchTime;
    public TextMeshProUGUI activeCounter;
    public QuestGiver questgiverScript;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        stopwatchTime += Time.deltaTime;

        float minutes = Mathf.FloorToInt(timeSurvived / 60);
        float seconds = Mathf.FloorToInt(timeSurvived % 60);

        timeSurvived = questgiverScript.survivaltime;
        activeCounter.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

 
}
