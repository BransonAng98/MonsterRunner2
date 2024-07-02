using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IDriveToken : MonoBehaviour
{
    public int tokenID;
    public TextMeshPro text;

    public IDriveTokenManager iDriveTokenManager;

    string letter;

    public void AssignVariable()
    {
        switch (tokenID)
        {
            case 0:
                text.text = "I";
                letter = text.text;
                break;
            case 1:
                text.text = "D";
                break;
            case 2:
                text.text = "r";
                break;
            case 3:
                text.text = "i";
                break;
            case 4:
                text.text = "v";
                break;
            case 5:
                text.text = "e";
                break;
        }

        letter = text.text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerEffect();
        }
    }

    void TriggerEffect()
    {
        iDriveTokenManager.ActivateUIElement(tokenID);
        iDriveTokenManager.DespawnLetterTokens(tokenID);
    }
}
