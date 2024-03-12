using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public DemoPlayer player;

    public GameObject questWindow;
    public Text titleText;
    public Text descriptionText;
    public Text rewardText;

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        rewardText.text = quest.repairReward;
    }

    public void AcceptQuest()
    {
        // give quest to player
        questWindow.SetActive(false);
        quest.isActive = true;
        player.quest = quest;
    }
}
