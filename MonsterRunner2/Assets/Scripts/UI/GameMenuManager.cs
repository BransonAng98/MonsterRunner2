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
        SceneManager.LoadScene("TestLevel");
        hasStarted = true;
    }

    public void OpenMenu(GameObject nextMenu)
    {
        currentMenu = nextMenu;
        nextMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ReturnToMain()
    {
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
