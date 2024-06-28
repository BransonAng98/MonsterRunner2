using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityManager : MonoBehaviour
{
    public enum AbilityState
    {
        Ready,
        Active,
        Cooldown,
    }

    //Public Variable
    public AbilityState abilityState = AbilityState.Ready;
    public bool isTriggered;
    public int abilityID;
    public DemoPlayer player;
    public AbilityTokenManager abTokenManager;
    public Slider abilityActiveSlider;
    public GameObject abilityPopUpDisplay;

    //Private Variable
    private float cooldown;
    private float activeTime;
    private RectTransform sliderRectTransform;
    private Vector3 sliderOffset = new Vector3(0, 100, 0);  // Adjust this value to position the slider above the player

    //Serializable Variables
    [SerializeField] List<AbilitySO> ability = new List<AbilitySO>();

    private void Start()
    {
        if(player != null)
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
                    //Sets the state to activate so the abilty is triggered
                    abilityState = AbilityState.Active;
                    activeTime = ability[abilityID].abilityActive;
                    abilityPopUpDisplay.SetActive(true);
                    if (abilityActiveSlider != null)
                    {
                        abilityActiveSlider.maxValue = activeTime;
                        abilityActiveSlider.value = activeTime;
                    }
                }
                break;

            case AbilityState.Active:
                if(activeTime > 0)
                {
                    //Countdown from the ability's activation time
                    activeTime -= Time.deltaTime;
                    //Activates the corresponding ability SO within the ability list
                    ability[abilityID].Activate();

                    if (abilityActiveSlider != null)
                    {
                        abilityActiveSlider.gameObject.SetActive(true);
                        abilityActiveSlider.value = activeTime;
                    }
                }
                else
                {
                    //When the ability has reached the end of its activation period transit the state to cooldown
                    abilityState = AbilityState.Cooldown;
                    ability[abilityID].Deactive();
                    cooldown = ability[abilityID].abilityCD;
                    abilityPopUpDisplay.SetActive(false);
                    // Reset the slider value
                    if (abilityActiveSlider != null)
                    {
                        abilityActiveSlider.gameObject.SetActive(false);
                        abilityActiveSlider.value = 0;
                    }
                }
                break;

            case AbilityState.Cooldown:
                if(cooldown > 0)
                {
                    //Cooldown after the end of the ability's activation
                    cooldown -= Time.deltaTime;
                }
                else
                {
                    //Reset the trigger to be false so the ability won't be activate when transitioning to ready state
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


}
