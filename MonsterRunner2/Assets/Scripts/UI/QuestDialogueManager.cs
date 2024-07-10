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
    public string[] featureText; // Array for first set of feature explanations
    public Image[] featureImages; // Array for first set of feature-related images

    // Second set of feature explanations and images
    public string[] secondFeatureText;

    public float textSpd;
    public bool isAccepting;
    public bool isWarningTyping;
    public bool introSequenceComplete = false;
    public bool isDoneExplaining = false;
    public bool isDoneSecondExplaination = false;

    private Coroutine typingCoroutine; // Reference to the current coroutine
    private Coroutine flickeringCouroutine;

    private bool isFeatureExplaining = false; // Flag for feature explanation
    private int currentFeatureIndex = 0; // Index to track current feature explanation
    private bool isWaitingForClick = false; // Debounce flag
    public Audiomanager audiomanagerScript;
    private void Start()
    {
        switch (menuManager.sceneID)
        {
            case 1:
                player.canMove = false;
                break;
            case 2:
                player.canMove = true;
                break;
        }

        CloseWindow();
    }

    public void TypeText(bool isAccepting, int index)
    {
        audiomanagerScript.PlayCartoonTalking();
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
        if (menuManager.sceneID == 1)
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
        typingCoroutine = null; // Reset coroutine reference

        // Start the feature explanation
        StartCoroutine(TypeFeatureExplanation());
    }

    private IEnumerator TypeFeatureExplanation()
    {
        isFeatureExplaining = true; // Set the flag to true
        currentFeatureIndex = 0; // Reset feature index

        // Display the first set of feature explanations
        DisplayFeature(currentFeatureIndex);

        // Wait for player tap to progress through feature explanations
        while (currentFeatureIndex < featureText.Length - 1)
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !isWaitingForClick);
            currentFeatureIndex++;
            DisplayFeature(currentFeatureIndex);
            isWaitingForClick = true;
            yield return new WaitForSeconds(0.2f); // Delay to avoid double click
            isWaitingForClick = false;
        }

        // Wait for one final click before closing
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0) && !isWaitingForClick);
        isWaitingForClick = true;
        yield return new WaitForSeconds(0.2f); // Delay to avoid double click
        isWaitingForClick = false;

        isFeatureExplaining = false; // Set the flag to false
        isDoneExplaining = true;
        player.canMove = true; // Allow player to move again

        CloseWindow();
    }

    public void StartSecondTutorial()
    {
        StartCoroutine(TypeSecondFeatureExplanation());
    }

    private IEnumerator TypeSecondFeatureExplanation()
    {
        currentFeatureIndex = 0; // Reset feature index for second set

        // Display the second set of feature explanations
        DisplaySecondFeature(currentFeatureIndex);

        // Automatically progress through second feature explanations
        while (currentFeatureIndex < secondFeatureText.Length - 1)
        {
            yield return new WaitForSeconds(2f); // Adjust the duration to match the typing speed and length of the text
            currentFeatureIndex++;
            DisplaySecondFeature(currentFeatureIndex);
        }

        // Wait for one final delay before closing
        yield return new WaitForSeconds(1f); // Adjust the duration to match the typing speed and length of the text

        isFeatureExplaining = false; // Set the flag to false
        isDoneSecondExplaination = true;
        player.canMove = true;

        CloseWindow();
    }

    private void DisplayFeature(int index)
    {
        questWindow.SetActive(true);
        descriptionText.text = string.Empty;

        // Update the feature image
        if (index < featureImages.Length)
        {
            foreach (Image img in featureImages)
            {
                img.gameObject.SetActive(false); // Hide all images
            }
            featureImages[index].gameObject.SetActive(true); // Show the current image
        }

        foreach (char letter in featureText[index].ToCharArray())
        {
            descriptionText.text += letter;
            // Yield return is not required here because text display will happen instantly
        }
    }

    private void DisplaySecondFeature(int index)
    {
        questWindow.SetActive(true);
        descriptionText.text = string.Empty;

        foreach (char letter in secondFeatureText[index].ToCharArray())
        {
            descriptionText.text += letter;
            // Yield return is not required here because text display will happen instantly
        }
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

        foreach (Image img in featureImages)
        {
            img.gameObject.SetActive(false); // Hide all images from the first set
        }

        Debug.Log("Flicker closed");
    }
}

