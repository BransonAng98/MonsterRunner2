using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public int currentScene;
    public GameObject resultScreen;
    public GameObject mainMenu;
    public GameObject gameplayUI;
    public GameObject pauseMenu;
    public GameObject loadingScreen;
    public GameObject popUpScreen;
    public GameObject tutorialScreen1;
    public GameObject tutorialScreen2;
    public Slider loadingSlider;

    [SerializeField] GameObject currentMenu;
    [SerializeField] int sceneID;

    public PlayerDataSO playerData;
    public PlayerCarDisplay carDisplay;
    public JsonSystem json;
    public QuestGiver questGiver;

    // Start is called before the first frame update
    private void Awake()
    {
        AssignSceneID();
        if (resultScreen != null)
        {
            resultScreen.SetActive(false);
            pauseMenu.SetActive(false);
        }
        else
        {
            return;
        }
    }

    private void Start()
    {
        StartOfGameplayScene();
        DeactiveScene();
    }

    void AssignSceneID()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            case "MainMenu":
                sceneID = 0;
                break;
            case "TutorialLevel":
                sceneID = 1;
                break;
            case "TheCity":
                sceneID = 2;
                break;
        }
    }

    void DeactiveScene()
    {
        switch (sceneID)
        {
            case 1:
                tutorialScreen2.SetActive(false);
                break;

            case 2:
                tutorialScreen1.SetActive(false);
                tutorialScreen2.SetActive(false);
                break;
        }
    }

    void StartOfGameplayScene()
    {
        switch (sceneID)
        {
            case 1:
                Time.timeScale = 0f;
                tutorialScreen1.SetActive(true);
                tutorialScreen2.SetActive(false);
                break;

            case 2:
                break;
        }
    }

    public void LoadLevel()
    {
        if (playerData.hasPlayedTutorial)
        {
            StartCoroutine(LoadLevelAsync("TheCity"));
        }
        else
        {
            StartCoroutine(LoadLevelAsync("TutorialLevel"));
        }
    }

    IEnumerator LoadLevelAsync(string sceneName)
    {
        // Show loading screen
        loadingScreen.SetActive(true);

        // Fake loading progress
        float fakeProgress = 0f;
        while (fakeProgress < 1f)
        {
            fakeProgress += Time.deltaTime * 0.8f;
            loadingSlider.value = fakeProgress;
            yield return new WaitForSeconds(0.02f);
        }

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene load is complete
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                // Allow scene activation once the fake loading is done
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }



    public void OpenMenu(GameObject nextMenu)
    {
        currentMenu = nextMenu;
        nextMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ReturnToMain()
    {
        if(sceneID == 0)
        {
            currentMenu.SetActive(false);
            carDisplay.UpdateCarSkin(playerData.selectedVehicleID);
            mainMenu.SetActive(true);
        }

        else
        {
            playerData.money += playerData.moneyAccumulatedInGame;
            playerData.moneyAccumulatedInGame = 0;
        }

        json.SaveToJson(0);
        json.SaveToJson(1);
    }

    public void StartGame()
    {
        switch (sceneID)
        {
            case 1:
                tutorialScreen1.SetActive(false);
                Time.timeScale = 1f;
                break;

            case 2:
                Debug.Log("Start Game");
                Time.timeScale = 1f;
                break;
        }
    }

    public void OpenTutorialPage2()
    {
        tutorialScreen1.SetActive(false);
        tutorialScreen2.SetActive(true);
    }

    public void StartTutorialGameplay()
    {
        if (tutorialScreen1.activeSelf == true)
        {
            tutorialScreen1.SetActive(false);
        }

        if(tutorialScreen2.activeSelf == true)
        {
            tutorialScreen2.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void OpenPopUpScreen()
    {
        popUpScreen.SetActive(true);
        questGiver.isInProgress = false;
        Time.timeScale = 0f;
    }

    public void ClosePopUpScreen()
    {
        popUpScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        Debug.Log("Game Paused");
        gameplayUI.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Debug.Log("Game Unpaused");
        Time.timeScale = 1f;
        gameplayUI.SetActive(true);
        pauseMenu.SetActive(false); 
    }

    public void RestartGame()
    {
        playerData.money += playerData.moneyAccumulatedInGame;
        playerData.moneyAccumulatedInGame = 0;
        json.SaveToJson(0);
        json.SaveToJson(1);
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        playerData.money += playerData.moneyAccumulatedInGame;
        playerData.moneyAccumulatedInGame = 0;
        json.SaveToJson(1);
        SceneManager.LoadScene("MainMenu");
    }
}
