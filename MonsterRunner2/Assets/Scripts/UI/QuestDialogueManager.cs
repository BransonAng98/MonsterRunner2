using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestDialogueManager : MonoBehaviour
{
    public GameObject questWindow;
    public TextMeshProUGUI descriptionText;
    public string[] introText;
    public string[] questText;
    public string[] rewardText;
    public string[] warningText;

    public float textSpd;
    public bool isAccepting;
    public bool isWarningTyping;

    private Coroutine typingCoroutine; // Reference to the current coroutine

    private void Awake()
    {
        CloseWindow();
    }

    public void TypeText(bool isAccepting, int index)
    {
        questWindow.SetActive(true);
        descriptionText.text = string.Empty;
        this.isAccepting = isAccepting;

        

        if(!isWarningTyping)
        {
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
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Stop previous coroutine if still running
            }
            typingCoroutine = StartCoroutine(TypeWarning(index));
            isWarningTyping = false;
        }
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
    public void TypeIntro(int index)
    {
        questWindow.SetActive(true);
        descriptionText.text = string.Empty;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop previous coroutine if still running
        }
        typingCoroutine = StartCoroutine(TypeIntroInternal(index));
    }

    private IEnumerator TypeIntroInternal(int index)
    {
        foreach (char letter in introText[index].ToCharArray())
        {
            descriptionText.text += letter;
            yield return new WaitForSeconds(textSpd);
        }

        typingCoroutine = null; // Reset coroutine reference
        // Wait a bit longer before closing to ensure the player can read the text
        yield return new WaitForSeconds(1f);
        CloseWindow();
    }

    void CloseWindow()
    {
        questWindow.SetActive(false);
    }
}
