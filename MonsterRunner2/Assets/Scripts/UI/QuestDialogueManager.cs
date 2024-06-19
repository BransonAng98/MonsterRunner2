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

    public float textSpd;
    public bool isAccepting;

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
