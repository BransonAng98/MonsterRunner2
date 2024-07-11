using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlackTrigger : MonoBehaviour
{
    public TutorialManager tutorialManagerScript;
    [SerializeField] private bool triggerfade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerfade == true)
        {
            tutorialManagerScript.StartFade();
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerfade = true;
        tutorialManagerScript.TypeCompleteText();
    }
}
