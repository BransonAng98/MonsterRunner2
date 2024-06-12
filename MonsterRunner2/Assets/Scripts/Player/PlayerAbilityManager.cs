using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Private Variable
    private float cooldown;
    private float activeTime;

    //Serializable Variables
    [SerializeField] List<AbilitySO> ability = new List<AbilitySO>();

    private void Start()
    {
        if(player != null)
        {
            ability.Add(player.ability1);
            ability.Add(player.ability2);
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
                }
                break;

            case AbilityState.Active:
                if(activeTime > 0)
                {
                    //Countdown from the ability's activation time
                    activeTime -= Time.deltaTime;

                    //Activates the corresponding ability SO within the ability list
                    ability[abilityID].Activate();
                }
                else
                {
                    //When the ability has reached the end of its activation period transit the state to cooldown
                    abilityState = AbilityState.Cooldown;
                    ability[abilityID].Deactive();
                    cooldown = ability[abilityID].abilityCD;
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
    }
}
