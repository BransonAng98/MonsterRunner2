using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionBar : MonoBehaviour
{
    public Slider slider;
    public float barValue;
    public int maximumValue;
    public float decreaseMultiplier;
    public bool isIncreasing;
    public DemoPlayer player;
    public QuestDialogueManager dialogueManger;

    public GameObject eyeOpen;
    public GameObject eyeClose;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maximumValue;
    }

    void TriggerWarning(float sliderValue)
    {
        switch (sliderValue)
        {
            case 60:
                dialogueManger.isWarningTyping = true;
                dialogueManger.TypeText(false, 0);
                break;

            case 75:
                dialogueManger.isWarningTyping = true;
                dialogueManger.TypeText(false, 1);
                break;

            case 85:
                dialogueManger.isWarningTyping = true;
                dialogueManger.TypeText(false, 2);
                break;
        }
    }

    public void StartBar()
    {
        eyeClose.SetActive(false);
        eyeOpen.SetActive(true);
        barValue += Time.deltaTime;
        if(slider != null)
        {
            if(slider.value < slider.maxValue)
            {
                slider.value = barValue;
            }
            else
            {
                slider.value = slider.maxValue;
                barValue = slider.maxValue;
            }
        }
    }

    public void ResetBar()
    {
        eyeClose.SetActive(true);
        eyeOpen.SetActive(false);
        barValue -= Time.deltaTime * decreaseMultiplier;
        if (slider != null)
        {
            if(slider.value > 0)
            {
                slider.value = barValue;
            }

            else
            {
                slider.value = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isIncreasing)
        {

            if (slider.value != slider.maxValue)
            {
                //Increase bar timing
                StartBar();
                TriggerWarning(slider.value);
            }

            else
            {
                slider.value = slider.maxValue;
                barValue = slider.maxValue;
                //game over
                player.TakeDamage(1000);
            }
        }

        else
        {
            if(slider.value > 0)
            {
                ResetBar();
            }

            else
            {
                slider.value = 0;
                barValue = 0;
            }
        }
    }
}
