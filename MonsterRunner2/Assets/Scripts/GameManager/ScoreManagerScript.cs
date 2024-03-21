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

    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        timeSurvived += Time.deltaTime;


    }

 
}
