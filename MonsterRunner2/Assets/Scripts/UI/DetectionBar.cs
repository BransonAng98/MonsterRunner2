using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectionBar : MonoBehaviour
{
    public Slider slider;
    public float barValue;
    public float decreaseMultiplier;
    public bool isIncreasing;
    public DemoPlayer player;

    public GameObject eyeOpen;
    public GameObject eyeClose;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
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
