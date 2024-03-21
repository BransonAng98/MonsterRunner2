using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreManagerScript : MonoBehaviour
{
    public int enemiesKilled;
    public int missionsCompleted;
    public float timeSurvived;
    public TextMeshProUGUI activeCounter;

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

        float minutes = Mathf.FloorToInt(timeSurvived / 60);
        float seconds = Mathf.FloorToInt(timeSurvived % 60);

        timeSurvived += Time.deltaTime;
        activeCounter.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

 
}
