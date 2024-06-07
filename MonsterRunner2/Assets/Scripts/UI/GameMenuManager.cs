using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuManager : MonoBehaviour
{
    public int currentScene;
    public GameObject startScreen;
    public GameObject defeatScreen;
    public GameObject mainMenu;
    [SerializeField] bool hasStarted;
    [SerializeField] GameObject currentMenu;
    // Start is called before the first frame update
    private void Awake()
    {
        if(defeatScreen != null)
        {
            defeatScreen.SetActive(false);
        }
        else
        {
            return;
        }

        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {

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
        mainMenu.SetActive(true);
    }


    public void StartGame()
    {
        Debug.Log("Start Game");
        Time.timeScale = 1f;
        startScreen.SetActive(false);
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
        SceneManager.LoadScene("MainMenu");
    }

}
