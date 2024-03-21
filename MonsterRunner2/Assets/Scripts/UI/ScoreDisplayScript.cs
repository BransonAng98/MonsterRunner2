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
        StartCoroutine(LerpScores());
      

    }
    private IEnumerator LerpScores()
    {
        float initialEnemyScore = 0;
        float initialMissionScore = 0;
        float initialTimeScore = 0;

        float targetEnemyScore = scoreManager.enemiesKilled;
        float targetMissionScore = scoreManager.missionsCompleted;
        float targetTimeScore = scoreManager.timeSurvived;

        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            float t = elapsedTime / lerpDuration;

            int enemyScore = Mathf.RoundToInt(Mathf.Lerp(initialEnemyScore, targetEnemyScore, t));
            int missionScore = Mathf.RoundToInt(Mathf.Lerp(initialMissionScore, targetMissionScore, t));
            float timeScore = Mathf.Lerp(initialTimeScore, targetTimeScore, t);

            UpdateScoreUI(enemyScore, missionScore, timeScore);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final scores match the actual values
        UpdateScoreUI(scoreManager.enemiesKilled, scoreManager.missionsCompleted, scoreManager.timeSurvived);
    }

    private void UpdateScoreUI(int killscore, int missionScore, float time)
    {
        enemiesKilledText.text = killscore.ToString();
        missionsCompletedText.text = missionScore.ToString();
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timeSurvivedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
