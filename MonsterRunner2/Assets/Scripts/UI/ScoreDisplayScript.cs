using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplayScript : MonoBehaviour
{
    [SerializeField] private int enemiesKilled;
    [SerializeField] private int missionsCompleted;
    [SerializeField] private float timeSurvived;

    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI missionsCompletedText;
    public TextMeshProUGUI timeSurvivedText;
    private float lerpDuration = 2.0f;

    public ScoreManagerScript scoreManager;
    // Start is called before the first frame update
    void Start()
    {
        
        enemiesKilled= scoreManager.enemiesKilled;
        missionsCompleted = scoreManager.missionsCompleted;
        timeSurvived = scoreManager.timeSurvived;
        StartCoroutine(LerpScores());

    }
    private IEnumerator LerpScores()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;

            int enemyScore = Mathf.RoundToInt(Mathf.Lerp(0, scoreManager.enemiesKilled, t));
            int missionScore = Mathf.RoundToInt(Mathf.Lerp(0, scoreManager.missionsCompleted, t));
          
            float timeScore = Mathf.RoundToInt(Mathf.Lerp(0, scoreManager.timeSurvived, t));
            //formattedTime = clock.GetFormattedTime(timeScore);
           


            UpdateScoreUI(enemyScore, missionScore, timeScore);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        UpdateScoreUI(scoreManager.enemiesKilled, scoreManager.missionsCompleted, scoreManager.timeSurvived);
    }

    private void UpdateScoreUI(int killscore, int missionScore, float time)
    {
        enemiesKilledText.text = "" + enemiesKilled;
        missionsCompletedText.text = "" + missionsCompleted;
        float minutes = Mathf.FloorToInt(timeSurvived / 60);
        float seconds = Mathf.FloorToInt(timeSurvived % 60);

        timeSurvivedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void SetActiveScreen()
    {

    }
}
