using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerAbilityManager : MonoBehaviour
{
    public enum AbilityState
    {
        Ready,
        Active,
        Cooldown,
    }

    // Public Variables
    public AbilityState abilityState = AbilityState.Ready;
    public bool isTriggered;
    public int abilityID;
    public DemoPlayer player;
    public AbilityTokenManager abTokenManager;
    public Slider abilityActiveSlider;
    public GameObject abilityPopUpDisplay;
    public Image abilityIcon;
    public TextMeshProUGUI abilityName;
    public RectTransform offScreenPosition;   // Position off the screen
    public RectTransform targetPosition;      // Designated target position
    public float moveDuration;
    public Sprite swordSprite;
    public Sprite shieldSprite;
    public Image abilityIconIndicator;

    // Private Variables
    private float cooldown;
    private float activeTime;
    private RectTransform sliderRectTransform;
    private Vector3 sliderOffset = new Vector3(0, 100, 0);  // Adjust this value to position the slider above the player
    private bool isMoving = false;  // To check if the pop-up is currently moving

    // Serializable Variables
    [SerializeField] List<AbilitySO> ability = new List<AbilitySO>();

    private void Start()
    {
        if (player != null)
        {
            ability.Add(player.ability1);
            ability.Add(player.ability2);
            abilityPopUpDisplay.SetActive(false);
        }

        // Initialize the slider
        if (abilityActiveSlider != null)
        {
            abilityActiveSlider.maxValue = 0;
            abilityActiveSlider.value = 0;
            sliderRectTransform = abilityActiveSlider.GetComponent<RectTransform>();
            abilityActiveSlider.gameObject.SetActive(false);
        }

        // Initialize the pop-up off screen
        if (offScreenPosition != null && abilityPopUpDisplay != null)
        {
            abilityPopUpDisplay.GetComponent<RectTransform>().anchoredPosition = offScreenPosition.anchoredPosition;
        }
    }

    void UpdatePopUp(int abilityID)
    {
        abilityPopUpDisplay.SetActive(true);
        abilityName.text = ability[abilityID].abilityName;
        switch (abilityID)
        {
            case 0:
                abilityIcon.sprite = player.playerData.ability1Sprite;
                abilityIconIndicator.sprite = swordSprite;
                break;
            case 1:
                abilityIcon.sprite = player.playerData.ability2Sprite;
                abilityIconIndicator.sprite = shieldSprite;
                break;
        }

        // Move the pop-up to the target position
        if (targetPosition != null)
        {
            StartCoroutine(MoveToPosition(abilityPopUpDisplay.GetComponent<RectTransform>(), targetPosition.anchoredPosition, moveDuration));
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (abilityState)
        {
            case AbilityState.Ready:
                if (isTriggered)
                {
                    abTokenManager.DespawnTokens();
                    // Sets the state to activate so the ability is triggered
                    abilityState = AbilityState.Active;
                    activeTime = ability[abilityID].abilityActive;
                    UpdatePopUp(abilityID);
                    if (abilityActiveSlider != null)
                    {
                        abilityActiveSlider.maxValue = activeTime;
                        abilityActiveSlider.value = activeTime;
                    }
                }
                break;

            case AbilityState.Active:
                if (activeTime > 0)
                {
                    // Countdown from the ability's activation time
                    activeTime -= Time.deltaTime;
                    // Activates the corresponding ability SO within the ability list
                    ability[abilityID].Activate();

                    if (abilityActiveSlider != null)
                    {
                        abilityActiveSlider.gameObject.SetActive(true);
                        abilityActiveSlider.value = activeTime;
                    }
                }
                else
                {
                    // When the ability has reached the end of its activation period transit the state to cooldown
                    abilityState = AbilityState.Cooldown;
                    ability[abilityID].Deactive();
                    cooldown = ability[abilityID].abilityCD;
                    player.skillCDParticles[abilityID].SetActive(true);
                    // Reset the slider value
                    if (abilityActiveSlider != null)
                    {
                        abilityActiveSlider.gameObject.SetActive(false);
                        abilityActiveSlider.value = 0;
                    }

                    // Move the pop-up off screen
                    if (offScreenPosition != null)
                    {
                        StartCoroutine(MoveToPosition(abilityPopUpDisplay.GetComponent<RectTransform>(), offScreenPosition.anchoredPosition, moveDuration));
                    }
                }
                break;

            case AbilityState.Cooldown:
                if (cooldown > 0)
                {
                    // Cooldown after the end of the ability's activation
                    cooldown -= Time.deltaTime;
                }
                else
                {
                    // Reset the trigger to be false so the ability won't be activated when transitioning to ready state
                    abTokenManager.SpawnPowerUps();
                    isTriggered = false;
                    abilityState = AbilityState.Ready;
                }
                break;
        }

        // Update slider position to follow the player smoothly
        if (player != null && abilityActiveSlider != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position) + sliderOffset;
            sliderRectTransform.position = Vector3.Lerp(sliderRectTransform.position, screenPos, Time.deltaTime * 10f);
        }
    }

    private IEnumerator MoveToPosition(RectTransform rectTransform, Vector2 target, float duration)
    {
        Vector2 initialPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = target;
    }
}
