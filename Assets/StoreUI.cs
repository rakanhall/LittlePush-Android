using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreUI : MonoBehaviour
{
    public Button removeAdsButton;
    public Button addSlotButton;
    public TextMeshProUGUI removeAdsButtonText;
    public TextMeshProUGUI addSlotButtonText;

    private bool areAdsRemoved = false;
    private bool isSlotAdded = false;

    private string removeAdsKey = "RemoveAdsState";
    private string addSlotKey = "AddSlotState";

    private void Start()
    {
        // Load the button states from PlayerPrefs
        areAdsRemoved = PlayerPrefs.GetInt(removeAdsKey, 0) == 1;
        isSlotAdded = PlayerPrefs.GetInt(addSlotKey, 0) == 1;

        UpdateButtonStates();


    }

    private void UpdateButtonStates()
    {
        removeAdsButton.interactable = !areAdsRemoved;
        addSlotButton.interactable = !isSlotAdded;

        removeAdsButtonText.text = areAdsRemoved ? "PURCHASED" : "US$ 0.99";
        addSlotButtonText.text = isSlotAdded ? "PURCHASED" : "US$ 2.99";
    }

    public void ToggleRemoveAdsState()
    {
        areAdsRemoved = !areAdsRemoved;
        PlayerPrefs.SetInt(removeAdsKey, areAdsRemoved ? 1 : 0);
        PlayerPrefs.Save();
        UpdateButtonStates();
    }

    public void ToggleAddSlotState()
    {
        isSlotAdded = !isSlotAdded;
        PlayerPrefs.SetInt(addSlotKey, isSlotAdded ? 1 : 0);
        PlayerPrefs.Save();
        UpdateButtonStates();
    }
}

