using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;

    public Image slot1;
    public Image slot2;

    [Header("Slot UI")]
    public Image slot1Image;
    public Image slot2Image;
    public Image slot3Image;
    public Image slot4Image;

    [Header("Gameplay Buttons Text")]
    public TextMeshProUGUI gameplayButton1Text;  // Assign the TextMeshProUGUI component of gameplayButton1 in the inspector
    public TextMeshProUGUI gameplayButton2Text;  // Assign the TextMeshProUGUI component of gameplayButton2 in the inspector

    [Header("Gameplay Buttons")]
    public Button gameplayButton1;
    public Button gameplayButton2;

    [Header("Power Icons")]
    public List<string> powerNames = new List<string>();
    public List<Sprite> powerSprites = new List<Sprite>();
    private Dictionary<string, Sprite> powerUpIcons = new Dictionary<string, Sprite>();

    [Header("PowerUp Controller")]
    public PowerUpController powerUpController;

    [Header("PlayerController")]
    public PlayerController playercontroller;

    [HideInInspector]
    public string slot1Power = "";
    [HideInInspector]
    public string slot2Power = "";

    public Button plusButton; // The button to add another slot. Link this in the Unity inspector.

    private int availableSlots = 1;  // Default to 1 available slot.


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Populate the powerUpIcons dictionary
        for (int i = 0; i < Mathf.Min(powerNames.Count, powerSprites.Count); i++)
        {
            powerUpIcons.Add(powerNames[i], powerSprites[i]);
        }

        slot2.gameObject.SetActive(false); // Disable the second slot initially
        gameplayButton2.gameObject.SetActive(false); // Disable its gameplay button as well
        gameplayButton2Text.gameObject.SetActive(false); // Disable the text too if it's separate

        plusButton.onClick.AddListener(UnlockSecondSlot);

        LoadSlotState();
        LoadSlotState();
        UpdateSlotImages();
        UpdateGameplayButtons();
    }

    public bool EquipPower(string powerName, int spaceRequired)
    {
        Debug.Log("Attempting to equip power: " + powerName + " with space required: " + spaceRequired);

        bool equipped = false;

        if (spaceRequired == 1)
        {
            if (string.IsNullOrEmpty(slot1Power))
            {
                slot1Power = powerName;
                equipped = true;
            }
            else if (availableSlots == 2 && string.IsNullOrEmpty(slot2Power))
            {
                slot2Power = powerName;
                equipped = true;
            }
        }

        if (equipped)
        {
            UpdateSlotImages();
            UpdateGameplayButtons();
            SaveSlotState();
        }

        return equipped;
    }

    public void UnequipPower(string powerName)
    {
        if (slot1Power == powerName)
        {
            slot1Power = "";
            if (slot2Power == powerName)
            {
                slot2Power = "";  // Clear the second slot as well
            }
        }
        else if (slot2Power == powerName)
        {
            slot2Power = "";
        }

        UpdateSlotImages();
        UpdateGameplayButtons();
        SaveSlotState();
    }

    private void UpdateGameplayButtons()
    {
        // Button 1
        int slot1Quantity = PlayerPrefs.GetInt(slot1Power + "_Quantity", 0);
        if (!string.IsNullOrEmpty(slot1Power) && slot1Quantity > 0)
        {
            gameplayButton1.gameObject.SetActive(true);
            gameplayButton1.onClick.RemoveAllListeners();
            gameplayButton1.onClick.AddListener(() => UsePower(slot1Power));
            gameplayButton1Text.text = slot1Quantity.ToString(); // Update the text component
        }
        else
        {
            gameplayButton1.gameObject.SetActive(false);
        }

        // Button 2
        if (availableSlots == 2)
        {
            int slot2Quantity = PlayerPrefs.GetInt(slot2Power + "_Quantity", 0);
            if (!string.IsNullOrEmpty(slot2Power) && slot1Power != slot2Power && slot2Quantity > 0)
            {
                gameplayButton2.gameObject.SetActive(true);
                gameplayButton2.onClick.RemoveAllListeners();
                gameplayButton2.onClick.AddListener(() => UsePower(slot2Power));
                gameplayButton2Text.text = slot2Quantity.ToString(); // Update the text component
            }
            else
            {
                gameplayButton2.gameObject.SetActive(false);
            }
        }

    }

    private void UpdateSlotImages()
    {
        if (!string.IsNullOrEmpty(slot1Power))
        {
            slot1Image.sprite = powerUpIcons[slot1Power];
            slot1Image.enabled = true;
        }
        else
        {
            slot1Image.enabled = false;
        }

        if (!string.IsNullOrEmpty(slot2Power) && slot1Power != slot2Power)
        {
            slot2Image.sprite = powerUpIcons[slot2Power];
            slot2Image.enabled = true;
        }
        else
        {
            slot2Image.enabled = false;
        }


        if (!string.IsNullOrEmpty(slot1Power))
        {
            slot3Image.sprite = powerUpIcons[slot1Power];
            slot3Image.enabled = true;
        }
        else
        {
            slot3Image.enabled = false;
        }

        if (!string.IsNullOrEmpty(slot2Power) && slot1Power != slot2Power)
        {
            slot4Image.sprite = powerUpIcons[slot2Power];
            slot4Image.enabled = true;
        }
        else
        {
            slot4Image.enabled = false;
        }
    }

    private void UsePower(string powerName)
    {
        int currentQuantity = PlayerPrefs.GetInt(powerName + "_Quantity", 0);
        if (currentQuantity > 0)
        {
            currentQuantity--;
            PlayerPrefs.SetInt(powerName + "_Quantity", currentQuantity);

            if (currentQuantity == 0)
            {
                PlayerPrefs.SetInt(powerName + "_ShouldUnequip", 1);  // Set a flag to unequip this power when in shop scene
            }
        }

        if (powerName == slot1Power)
        {
            gameplayButton1.gameObject.SetActive(false);
        }
        else if (powerName == slot2Power)
        {
            gameplayButton2.gameObject.SetActive(false);
        }

        // Add logic here when a power button is pressed.
        if (powerName == "PowerUpShield")
        {
            powerUpController.EnableShield();
        }

        if (powerName == "PowerUpSlow")
        {
            powerUpController.ActivateSlowFall();
        }

        if (powerName == "PowerUpTimerPlus")
        {
            powerUpController.ActivateTimerPlus();
        }

        if (powerName == "PowerUpTriple")
        {
            playercontroller.EnableTripleJump();
        }
        // Handle other powers as you add them in the future
    }

    private void SaveSlotState()
    {
        PlayerPrefs.SetString("Slot1Power", slot1Power);
        PlayerPrefs.SetString("Slot2Power", slot2Power);
    }

    private void LoadSlotState()
    {
        slot1Power = PlayerPrefs.GetString("Slot1Power", "");
        slot2Power = PlayerPrefs.GetString("Slot2Power", "");

        // Load available slots from PlayerPrefs
        availableSlots = PlayerPrefs.GetInt("AvailableSlots", 1);  // Defaults to 1 if not set

        // Check if slot2 has a power or if available slots are set to 2, and adjust the UI
        if (!string.IsNullOrEmpty(slot2Power) || availableSlots == 2)
        {
            slot2.gameObject.SetActive(true);
            plusButton.gameObject.SetActive(false);  // Hide the '+' button
            gameplayButton2Text.gameObject.SetActive(true); // Enable the text too if it's separate
        }
    }



    public void ClearSlotData()
    {
        PlayerPrefs.DeleteKey("Slot1Power");
        PlayerPrefs.DeleteKey("Slot2Power");
    }

    public void UnlockSecondSlot()
    {
        availableSlots = 2;
        PlayerPrefs.SetInt("AvailableSlots", availableSlots);  // Save the updated availableSlots to PlayerPrefs
        PlayerPrefs.Save();  // Ensure that the data is written immediately to disk
        slot2.gameObject.SetActive(true); // Enable the second slot
        plusButton.gameObject.SetActive(false); // Hide the '+' button once the second slot is unlocked
        gameplayButton2Text.gameObject.SetActive(true); // Enable the text too if it's separate
    }


}




