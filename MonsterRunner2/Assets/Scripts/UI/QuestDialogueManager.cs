using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestDialogueManager : MonoBehaviour
{
    public DemoPlayer player;

    public GameObject questWindow;
    public GameMenuManager menuManager;
    public TextMeshProUGUI descriptionText;
    public Image speechFX;
    public float flickerDuration;

    public string[] introText;
    public string[] questText;
    public string[] abilityText;
    public string[] rewardText;
    public string[] warningText;

    public float textSpd;
    public bool isAccepting;
    public bool isWarningTyping;
    public bool introSequenceComplete = false;

    private Coroutine typingCoroutine; // Reference to the current coroutine
    private Coroutine flickeringCouroutine; 
    private void Awake()
    {
    }
    private void Start()
    {
        CloseWindow();
    }

    public void TypeText(bool isAccepting, int index)
    {
        questWindow.SetActive(true);
        descriptionText.text = string.Empty;
        this.isAccepting = isAccepting;
        
        if (!isWarningTyping)
        {
            if (flickeringCouroutine != null)
            {
                StopCoroutine(flickeringCouroutine);
            }
            flickeringCouroutine = StartCoroutine(FlickerFX());

            if (isAccepting)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine); // Stop previous coroutine if still running
                }
                typingCoroutine = StartCoroutine(TypeQuest(index));
            }
            else
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine); // Stop previous coroutine if still running
                }
                typingCoroutine = StartCoroutine(TypeReward(index));
            }
        }
        else
        {
            if (flickeringCouroutine != null)
            {
                StopCoroutine(flickeringCouroutine);
            }
            flickeringCouroutine = StartCoroutine(FlickerFX());

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Stop previous coroutine if still running
            }
            typingCoroutine = StartCoroutine(TypeWarning(index));
            isWarningTyping = false;
        }
    }

    private IEnumerator FlickerFX()
    {
        while (true)
        {
            if (!speechFX.gameObject.activeSelf)
            {
                speechFX.gameObject.SetActive(true);
            }

            SetImageAlpha(speechFX, 0.3f); // Set alpha to 0
            yield return new WaitForSeconds(flickerDuration);
            SetImageAlpha(speechFX, 1f); // Set alpha to 1
            yield return new WaitForSeconds(flickerDuration);
        }
    }

    void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    IEnumerator TypeQuest(int index)
    {
        foreach (char letter in questText[index].ToCharArray())
        {
            descriptionText.text += letter;
            yield return new WaitForSeconds(textSpd);
        }

        typingCoroutine = null; // Reset coroutine reference
        // Wait a bit longer before closing to ensure the player can read the text
        yield return new WaitForSeconds(2f);
        CloseWindow();

        if (menuManager.currentScene == 1)
        {
            yield return StartCoroutine(TypeAbilityTutorial(0));
        }
    }

    IEnumerator TypeReward(int index)
    {
        foreach (char letter in rewardText[index].ToCharArray())
        {
            descriptionText.text += letter;
            yield return new WaitForSeconds(textSpd);
        }

        typingCoroutine = null; // Reset coroutine reference
        // Wait a bit longer before closing to ensure the player can read the text
        yield return new WaitForSeconds(1f);
        CloseWindow();
    }


    IEnumerator TypeAbilityTutorial(int index)
    {
        if (menuManager.currentScene == 1)
        {
            questWindow.SetActive(true);
            descriptionText.text = string.Empty;

            // Ensure the index is within bounds of the array
            if (index >= 0 && index < abilityText.Length)
            {
                foreach (char letter in abilityText[index].ToCharArray())
                {
                    descriptionText.text += letter;
                    yield return new WaitForSeconds(textSpd);
                }
            }
        }

        typingCoroutine = null; // Reset coroutine reference
        // Wait a bit longer before closing to ensure the player can read the text
        yield return new WaitForSeconds(1f);
        CloseWindow();
    }

    IEnumerator TypeWarning(int index)
    {
        questWindow.SetActive(true);
        descriptionText.text = string.Empty;

        foreach (char letter in warningText[index].ToCharArray())
        {
            descriptionText.text += letter;
            yield return new WaitForSeconds(textSpd);
        }

        typingCoroutine = null; // Reset coroutine reference
        // Wait a bit longer before closing to ensure the player can read the text
        yield return new WaitForSeconds(1f);
        CloseWindow();
    }

    public void TypeIntro()
    {
        // Check if the current scene is the tutorial scene
        if (menuManager.sceneID == 1) // Replace "TutorialScene" with your actual tutorial scene name
        {
            questWindow.SetActive(true);
            descriptionText.text = string.Empty;

            if (flickeringCouroutine != null)
            {
                StopCoroutine(flickeringCouroutine);
            }
            flickeringCouroutine = StartCoroutine(FlickerFX());

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Stop previous coroutine if still running
            }
            typingCoroutine = StartCoroutine(TypeIntroSequence());
        }
    }

    private IEnumerator TypeIntroSequence()
    {
        for (int i = 0; i < introText.Length; i++)
        {
            descriptionText.text = string.Empty;
            foreach (char letter in introText[i].ToCharArray())
            {
                descriptionText.text += letter;
                yield return new WaitForSeconds(textSpd);
            }

            // Wait a bit longer before moving to the next line
            yield return new WaitForSeconds(1f);
        }
        introSequenceComplete = true;
        Debug.Log("INTRO DONE");

        typingCoroutine = null; // Reset coroutine reference
        CloseWindow();
        player.canMove = true;
        
    }

    void CloseWindow()
    {
        questWindow.SetActive(false); 
        if (flickeringCouroutine != null)
        {
            StopCoroutine(flickeringCouroutine);
            flickeringCouroutine = null;
        }
        speechFX.gameObject.SetActive(false);
        Debug.Log("Flicker closed");

        switch (menuManager.sceneID)
        {
            case 1:
                player.canMove = false;
                break;
            case 2:
                player.canMove = true;
                break;
        }
    }
}

