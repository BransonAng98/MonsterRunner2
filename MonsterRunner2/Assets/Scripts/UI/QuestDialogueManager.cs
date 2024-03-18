using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestDialogueManager : MonoBehaviour
{
    public GameObject questWindow;
    public TextMeshProUGUI descriptionText;
    public string[] questText;
    public string[] rewardText;

    public float textSpd;

    // Start is called before the first frame update
    private void Awake()
    {
        CloseWindow();
    }

    public void TypeText(bool isAccepting, int index)
    {
        questWindow.SetActive(true);
        descriptionText.text = string.Empty;

        if (isAccepting)
        {
            StartCoroutine(TypeQuest(index));
        }
        else
        {
            StartCoroutine(TypeReward());
        }
    }

    IEnumerator TypeQuest(int index)
    {
        foreach (char letter in questText[index].ToCharArray())
        {
            descriptionText.text += letter;
            yield return new WaitForSeconds(textSpd); // Adjust speed here
        }

        Invoke("CloseWindow", 3f);
    }

    IEnumerator TypeReward()
    {
        foreach (char letter in questText[index].ToCharArray())
        {
            descriptionText.text += letter;
            yield return new WaitForSeconds(textSpd); // Adjust speed here
        }

        Invoke("CloseWindow", 3f);
    }

    void CloseWindow()
    {
        questWindow.SetActive(false);
    }
}
