using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public int currentScene;
    public GameObject startScreen;
    public GameObject resultScreen;
    public GameObject mainMenu;
    public GameObject gameplayUI;
    public GameObject pauseMenu;
    public GameObject loadingScreen;
    public GameObject popUpScreen;

    public Slider loadingSlider;

    [SerializeField] GameObject currentMenu;
    [SerializeField] bool hasStarted;

    public PlayerDataSO playerData;
    public PlayerCarDisplay carDisplay;
    public JsonSystem json;

    // Start is called before the first frame update
    private void Awake()
    {
        
        if (resultScreen != null)
        {
            resultScreen.SetActive(false);
            pauseMenu.SetActive(false);
        }
        else
        {
            return;
        }

        Time.timeScale = 0f;
        Debug.Log(playerData.hasPlayedTutorial);
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelAsync("TestLevel"));
        hasStarted = true;
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
        playerData.money += playerData.moneyAccumulatedInGame;
        playerData.moneyAccumulatedInGame = 0;
        currentMenu.SetActive(false);
        json.SaveToJson(0);
        json.SaveToJson(1);
        carDisplay.UpdateCarSkin(playerData.selectedVehicleID);
        mainMenu.SetActive(true);
    }

    public void StartGame()
    {
        Debug.Log("Start Game");
        Time.timeScale = 1f;
        startScreen.SetActive(false);
    }


    public void OpenPopUpScreen()
    {
        popUpScreen.SetActive(true);
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
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        json.SaveToJson(1);
        SceneManager.LoadScene("MainMenu");
    }
}
