using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;

    [Header("Slot UI")]
    public GameObject slot1; // Drag your first slot UI element here
    public GameObject slot2; // Drag your second slot UI element here

    [HideInInspector]
    public string slot1Power = ""; // To store the name of the power in slot 1
    [HideInInspector]
    public string slot2Power = ""; // To store the name of the power in slot 2

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public bool EquipPower(string powerName, int spaceRequired)
    {
        if (spaceRequired == 2 && string.IsNullOrEmpty(slot1Power) && string.IsNullOrEmpty(slot2Power))
        {
            slot1Power = powerName;
            slot2Power = powerName;
            // Here, update your slot UI to show the power's icon in both slots.
            return true;
        }
        else if (spaceRequired == 1)
        {
            if (string.IsNullOrEmpty(slot1Power))
            {
                slot1Power = powerName;
                // Here, update your slot1 UI to show the power's icon.
                return true;
            }
            else if (string.IsNullOrEmpty(slot2Power))
            {
                slot2Power = powerName;
                // Here, update your slot2 UI to show the power's icon.
                return true;
            }
        }
        return false; // Couldn't equip
    }

    public void UnequipPower(string powerName)
    {
        if (slot1Power == powerName)
        {
            slot1Power = "";
            // Update slot1 UI to be empty
        }
        if (slot2Power == powerName)
        {
            slot2Power = "";
            // Update slot2 UI to be empty
        }
    }
}
