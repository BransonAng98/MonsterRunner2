using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public PlayerDataSO playerData;
    public GameObject startGameScreen;
    public GameObject tutorialScreen;

    private void Start()
    {
        if (!playerData.hasPlayedTutorial)
        {
            tutorialScreen.SetActive(true);
            startGameScreen.SetActive(false);
        }

        else
        {
            tutorialScreen.SetActive(false);
            startGameScreen.SetActive(true);
        }
    }

    public void EndTutorial()
    {
        tutorialScreen.SetActive(false);
        playerData.hasPlayedTutorial = true;
        Time.timeScale = 1f;
    }
}
