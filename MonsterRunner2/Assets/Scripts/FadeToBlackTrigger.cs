using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlackTrigger : MonoBehaviour
{

    public TutorialManager tutorialManagerScript;
    [SerializeField] private bool triggerFade;

    private void Update()
    {
        if (triggerFade)
        {
            tutorialManagerScript.StartFade();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!triggerFade) // Ensure it's only triggered once
            {
                triggerFade = true;
                tutorialManagerScript.TypeCompleteText();
            }
        }
       
    }
}
