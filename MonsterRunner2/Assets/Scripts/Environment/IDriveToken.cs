using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IDriveToken : MonoBehaviour
{
    public int tokenID;
    public TextMeshPro text;

    public IDriveTokenManager iDriveTokenManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void AssignVariable()
    {
        switch (tokenID)
        {
            case 0:
                text.text = "I";
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerEffect(tokenID);
        }
    }

    void TriggerEffect(int id)
    {
        iDriveTokenManager.ActivateUIElement(tokenID);
        iDriveTokenManager.DespawnLetterTokens(tokenID);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
