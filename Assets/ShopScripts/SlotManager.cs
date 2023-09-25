using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;

    [Header("Slot UI")]
    public Image slot1Image;
    public Image slot2Image;

    [Header("Power Icons")]
    public List<string> powerNames = new List<string>();
    public List<Sprite> powerSprites = new List<Sprite>();
    private Dictionary<string, Sprite> powerUpIcons = new Dictionary<string, Sprite>();  // This is the dictionary we'll use

    [HideInInspector]
    public string slot1Power = "";
    [HideInInspector]
    public string slot2Power = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return; // Added return here
        }

        // Populate the powerUpIcons dictionary
        for (int i = 0; i < Mathf.Min(powerNames.Count, powerSprites.Count); i++)
        {
            powerUpIcons.Add(powerNames[i], powerSprites[i]);
        }

        LoadSlotState();
        UpdateSlotImages();

    }

    public bool EquipPower(string powerName, int spaceRequired)
    {
        Debug.Log("Attempting to equip power: " + powerName + " with space required: " + spaceRequired);

        bool equipped = false;

        if (spaceRequired == 2 && string.IsNullOrEmpty(slot1Power) && string.IsNullOrEmpty(slot2Power))
        {
            slot1Power = powerName;
            slot2Power = powerName;
            equipped = true;
        }
        else if (spaceRequired == 1)
        {
            if (string.IsNullOrEmpty(slot1Power))
            {
                slot1Power = powerName;
                equipped = true;
            }
            else if (string.IsNullOrEmpty(slot2Power))
            {
                slot2Power = powerName;
                equipped = true;
            }
        }

        if (equipped)
        {
            UpdateSlotImages();
            SaveSlotState();
        }

        return equipped;
    }

    public void UnequipPower(string powerName)
    {
        if (slot1Power == powerName)
        {
            slot1Power = "";
        }
        if (slot2Power == powerName)
        {
            slot2Power = "";
        }

        UpdateSlotImages();
        SaveSlotState();
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
            slot1Image.enabled = false;  // or use a default image
        }

        if (!string.IsNullOrEmpty(slot2Power))
        {
            slot2Image.sprite = powerUpIcons[slot2Power];
            slot2Image.enabled = true;
        }
        else
        {
            slot2Image.enabled = false;  // or use a default image
        }
    }

    public void ClearSlotData()
    {
        PlayerPrefs.DeleteKey("Slot1Power");
        PlayerPrefs.DeleteKey("Slot2Power");
    }

}


