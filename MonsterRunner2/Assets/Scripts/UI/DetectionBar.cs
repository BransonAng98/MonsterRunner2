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
    public QuestDialogueManager dialogueManager;
    public float sliderValuePercentage;

    public GameObject eyeOpen;
    public GameObject eyeClose;
    public Image pulsatingImage;
    public float pulsateDuration;

    private Color originalColor;
    [SerializeField] private bool isPulsating = false;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = maximumValue;

        if (pulsatingImage != null)
        {
            originalColor = pulsatingImage.color;
            SetPulsatingImageAlpha(0); // Set initial transparency
        }
    }

    IEnumerator PulsateColor()
    {
        while (true)
        {
            yield return StartCoroutine(FadeToAlpha(0));
            yield return StartCoroutine(FadeToAlpha(1));
        }
    }

    IEnumerator FadeToAlpha(float targetAlpha)
    {
        if (isPulsating)
        {
            if (!pulsatingImage.gameObject.activeSelf)
            {
                pulsatingImage.gameObject.SetActive(true);
            }

            float elapsedTime = 0f;
            float startAlpha = pulsatingImage.color.a;
            Color color = originalColor;

            while (elapsedTime < pulsateDuration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / pulsateDuration);
                color.a = newAlpha;
                pulsatingImage.color = color;
                yield return null; // Ensure coroutine yields every frame
            }

            color.a = targetAlpha;
            pulsatingImage.color = color;
        }
    }

    void SetPulsatingImageAlpha(float alpha)
    {
        Color color = pulsatingImage.color;
        color.a = alpha;
        pulsatingImage.color = color;
    }


    public void StartBar()
    {
        eyeClose.SetActive(false);
        eyeOpen.SetActive(true);
        barValue += Time.deltaTime;
        if (slider != null)
        {
            if (slider.value < slider.maxValue)
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
            if (slider.value > 0)
            {
                slider.value = barValue;
            }
            else
            {
                slider.value = 0;
            }
        }
    }

    void TriggerWarning()
    {
        float sliderPercentage = Mathf.Floor((barValue / slider.maxValue) * 100f);
        switch (sliderPercentage)
        {
            case 45:
                dialogueManager.isWarningTyping = true;
                dialogueManager.TypeText(false, 0);
                break;

            case 65:
                dialogueManager.isWarningTyping = true;
                dialogueManager.TypeText(false, 1);
                break;

            case 85:
                dialogueManager.isWarningTyping = true;
                dialogueManager.TypeText(false, 2);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (isIncreasing)
        {
            if (slider.value != slider.maxValue)
            {
                // Increase bar timing
                StartBar();
                TriggerWarning();
            }
            else
            {
                slider.value = slider.maxValue;
                barValue = slider.maxValue;
                // game over
                player.TakeDamage(1000);
            }
        }
        else
        {
            if (slider.value > 0)
            {
                ResetBar();
                Debug.Log("Reduce puslate");
            }
            else
            {
                isPulsating = false;
                SetPulsatingImageAlpha(0);
                slider.value = 0;
                barValue = 0;
            }
        }

        // Check if the slider value is more than 70%
        if (slider.value > slider.maxValue * 0.65f)
        {
            if (!isPulsating)
            {
                isPulsating = true;
                StartCoroutine(PulsateColor());
            }
        }
    }
}
