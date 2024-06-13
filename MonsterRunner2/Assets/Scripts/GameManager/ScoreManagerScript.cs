using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManagerScript : MonoBehaviour
{
    public float goldEarned;
    public int missionsCompleted;
    public float timeSurvived;
    public float stopwatchTime;
    public TextMeshProUGUI activeCounter;
    public TextMeshProUGUI goldamtCounter;
    public QuestGiver questgiverScript;
    public EnemySpawner enemySpawnerScript;

    private int lastMissionCount = 0;

    void Start()
    {
        goldEarned = 0f;
        missionsCompleted = 0;
        timeSurvived = 0f;
        stopwatchTime = 0f;
    }

    void Update()
    {
        stopwatchTime += Time.deltaTime;

        // Update the time survived from the quest giver script
        timeSurvived = questgiverScript.survivaltime;

        // Calculate minutes and seconds
        float minutes = Mathf.FloorToInt(timeSurvived / 60);
        float seconds = Mathf.FloorToInt(timeSurvived % 60);

        // Update the active counter display
        activeCounter.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        goldamtCounter.text = goldEarned.ToString();


        // Check if missions completed has increased
        if (missionsCompleted > lastMissionCount)
        {
            // Increase threat level for every 2 missions completed
            if (missionsCompleted % 2 == 0)
            {
                enemySpawnerScript.threatlvl++;
                Debug.Log("Threat level increased to: " + enemySpawnerScript.threatlvl);
            }

            lastMissionCount = missionsCompleted;
        }
    }
}
