using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerProfile : MonoBehaviour
{
    public PlayerDataSO playerData;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI contractText;

    private void OnEnable()
    {
        if (moneyText != null)
        {
            moneyText.text = "Money: " + " " + playerData.money;
        }

        if (contractText != null)
        {
            contractText.text = "Contract Completed: " + " " + playerData.contractCompleted;
        }
    }
}
