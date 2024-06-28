using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplayScript : MonoBehaviour
{
    [SerializeField] private int goldEarned;
    [SerializeField] private int missionsCompleted;
    [SerializeField] private float timeSurvived;

    public TextMeshProUGUI goldEarnedText;
    public TextMeshProUGUI missionsCompletedText;
    public TextMeshProUGUI timeSurvivedText;
    private float lerpDuration = 2.0f;

    public ScoreManagerScript scoreManager;
    public PlayerDataSO playerData;
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

        float targetEnemyScore = playerData.moneyAccumulatedInGame;
        float targetMissionScore = scoreManager.missionsCompleted;
        float targetTimeScore = scoreManager.stopwatchTime;

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
        UpdateScoreUI(playerData.moneyAccumulatedInGame, scoreManager.missionsCompleted, scoreManager.stopwatchTime);
    }

    private void UpdateScoreUI(float killscore, int missionScore, float time)
    {
        goldEarnedText.text = killscore.ToString();
        missionsCompletedText.text = missionScore.ToString();
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timeSurvivedText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
