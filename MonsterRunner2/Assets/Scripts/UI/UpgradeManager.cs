using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public float currency = 0;
    public TextMeshProUGUI currencyText;

    public Button gainCurrencyButton;
    public Button upgradeActiveSkillButton;
    public Button unlockSkillButton;
    public Button upgradeLockedSkillButton;


    public TextMeshProUGUI upgradeActiveSkillButtonText;
    public TextMeshProUGUI unlockSkillButtonText;
    public TextMeshProUGUI upgradeLockedSkillButtonText;

    public GameObject lockedSkillOverlay; // UI object to show overlay for locked skill
    public GameObject unlockSkillBtn;
    public GameObject upgradeLockedSkillBtn;
    public GameObject activeSkillBtn;

    public TextMeshProUGUI activeSkillLevelText;
    public TextMeshProUGUI lockedSkillLevelText;

    public Image ability1Icon;
    public Image ability2Icon;
    public Image ability2LockedIcon;

    public Button leftArrow;
    public Button rightArrow;
    public Button setActiveButton;
    public Button activeIndicator;

    private Skill activeSkill;
    private Skill lockedSkill;

    public PlayerCarDisplay carDisplay;
    public PlayerDataSO playerData;
    public JsonSystem json;

    void Start()
    {
        currency = playerData.money;
        activeSkill = new Skill(true, 100);  // Initial cost is 5
        lockedSkill = new Skill(false, 100); // Initial cost is 5

        UpdateUI();

        gainCurrencyButton.onClick.AddListener(GainCurrency);
        upgradeActiveSkillButton.onClick.AddListener(() => UpgradeSkill(activeSkill, 0));
        unlockSkillButton.onClick.AddListener(UnlockSkill);
        upgradeLockedSkillButton.onClick.AddListener(() => UpgradeSkill(lockedSkill, 1));

        setActiveButton.onClick.AddListener(SetActive);
        //leftArrow.onClick.AddListener(() => SwitchCar(0));
        //rightArrow.onClick.AddListener(() => SwitchCar(1));
        upgradeLockedSkillBtn.SetActive(false);

        //ActiveIconDisplay();
    }

    private void OnEnable()
    {
        ActiveIconDisplay();
    }

    public void ResetVehicleSelection()
    {
        carDisplay.selectedCarID = 0;
    }

    void GainCurrency()
    {
        currency += 100; // For testing, each click adds x currency
        UpdateUI();
    }

    void UpgradeSkill(Skill skill, int abilityID)
    {
        if (skill.unlocked && skill.level < skill.maxLevel && currency >= skill.cost)
        {
            currency -= skill.cost;
            skill.level++;
            skill.cost += 100; // Increase cost by x for each upgrade

            switch (abilityID)
            {
                //Upgrade first ability
                case 0:
                    carDisplay.cars[carDisplay.selectedCarID].vehicleData.ability1Level++;
                    break;

                case 1:
                    carDisplay.cars[carDisplay.selectedCarID].vehicleData.ability2Level++;
                    break;
            }

            UpdateUI();
        }
    }

    public void SetActive()
    {
        carDisplay.activateID = carDisplay.selectedCarID;
        carDisplay.UpdateCarSkin(carDisplay.activateID);
        playerData.selectedVehicleID = carDisplay.selectedCarID;
        json.SaveToJson(1);
        ActiveIconDisplay();
    }

    public void ActiveIconDisplay()
    {
        carDisplay.UpdateCarSkin(carDisplay.selectedCarID);
        ability1Icon.sprite = carDisplay.ability1Sprite;
        ability2Icon.sprite = carDisplay.ability2Sprite;
        ability2LockedIcon.sprite = carDisplay.ability2Sprite;

        if(carDisplay.cars[carDisplay.selectedCarID].vehicleData.ability2Level > 0)
        {
            ability2Icon.gameObject.SetActive(true);
        }
        else
        {
            ability2Icon.gameObject.SetActive(false);
        }

        if (carDisplay.selectedCarID == playerData.selectedVehicleID)
        {
            activeIndicator.interactable = false;
        }

        else
        {
            activeIndicator.interactable = true;
        }
    }

    public void SwitchCar(int dir)
    {
        switch (dir)
        {
            //Selecting left car
            case 0:
                if(carDisplay.selectedCarID > 0)
                {
                    carDisplay.selectedCarID--;
                }
                break;
            
            //Selecting right car
            case 1:
                if(carDisplay.selectedCarID < 1)
                {
                    carDisplay.selectedCarID++;
                }
                break;
        }

        ActiveIconDisplay();
        UpdateUI();
    }

    void UnlockSkill()
    {
        if (!lockedSkill.unlocked && currency >= lockedSkill.cost)
        {
            carDisplay.cars[carDisplay.selectedCarID].vehicleData.ability2Level++;
            currency -= lockedSkill.cost;
            lockedSkill.unlocked = true;
            lockedSkillOverlay.SetActive(true);
            upgradeLockedSkillBtn.SetActive(true);
            unlockSkillBtn.SetActive(false);
            UpgradeSkill(lockedSkill, 1);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        currencyText.text = "Currency: " + currency;
        playerData.money = currency;
        upgradeActiveSkillButtonText.text = $"Upgrade Skill (Cost: {activeSkill.cost})";
        unlockSkillButtonText.text = $"Unlock Skill (Cost: {lockedSkill.cost})";
        // Check if the skill is unlocked to display and enable the upgrade button
        if (lockedSkill.unlocked)
        {
            if (lockedSkill.level < lockedSkill.maxLevel)
            {
                upgradeLockedSkillButtonText.text = $"Upgrade Skill (Cost: {lockedSkill.cost})";
                upgradeLockedSkillButton.interactable = currency >= lockedSkill.cost;
            }
            else
            {
                upgradeLockedSkillButtonText.text = "MAX";
                upgradeLockedSkillButton.interactable = false;
            }
        }
        else
        {
            upgradeLockedSkillButtonText.text = $"Skill Locked (Cost: {lockedSkill.cost})";
            upgradeLockedSkillButton.interactable = currency >= lockedSkill.cost; // Make sure button is interactable if enough currency to unlock
        }

        if (carDisplay.cars[carDisplay.selectedCarID].vehicleData.ability1Level < activeSkill.maxLevel)
        {
            activeSkillLevelText.text = "Skill Level: " + carDisplay.cars[carDisplay.selectedCarID].vehicleData.ability1Level;
            upgradeActiveSkillButton.interactable = currency >= activeSkill.cost;
        }
        else
        {
            activeSkillLevelText.text = "MAX";
            activeSkillBtn.SetActive(false);
        }

        if (carDisplay.cars[carDisplay.selectedCarID].vehicleData.ability2Level < lockedSkill.maxLevel)
        {
            lockedSkillLevelText.text = "Skill Level: " + carDisplay.cars[carDisplay.selectedCarID].vehicleData.ability2Level;
        }
        else
        {
            lockedSkillLevelText.text = "MAX";
            upgradeLockedSkillBtn.SetActive(false);
 
        }

        unlockSkillButton.interactable = !lockedSkill.unlocked && currency >= lockedSkill.cost;
    }
}

    [System.Serializable]
public class Skill
{
    public int level;
    public int maxLevel;
    public int cost;
    public bool unlocked;

    public Skill(bool isUnlocked, int initialCost)
    {
        level = 0;
        maxLevel = 5;
        cost = initialCost;
        unlocked = isUnlocked;
    }
}