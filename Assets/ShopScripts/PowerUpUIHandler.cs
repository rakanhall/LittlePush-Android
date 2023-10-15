using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpUIHandler : MonoBehaviour
{
    // Power-Up Properties
    public string powerUpName;
    public int UnlockCost;
    public int PurchaseCost;
    public bool IsUnlocked = false;
    public bool IsEquipped = false;
    public int Quantity = 0;

    // UI Components
    public Button unlockButton;
    public Button buyButton;
    public Button equipButton;
    public Button unequipButton;
    public TextMeshProUGUI powerUpQuantityText;
    public TextMeshProUGUI unlockPriceText;
    public TextMeshProUGUI buyPriceText;

    public int slotSpaceRequired = 1; // Define how much space this power requires. 1 or 2.

    public Animator MessageAnimation;
    public GameObject BlackScreen;

    private void Start()
    {
        // Load values from PlayerPrefs
        IsUnlocked = PlayerPrefs.GetInt(powerUpName + "_IsUnlocked", 0) == 1;
        Quantity = PlayerPrefs.GetInt(powerUpName + "_Quantity", 0);
        IsEquipped = PlayerPrefs.GetInt(powerUpName + "_IsEquipped", 0) == 1;

        if (PlayerPrefs.GetInt(powerUpName + "_ShouldUnequip", 0) == 1)
        {
            UnequipPowerUp();
            PlayerPrefs.SetInt(powerUpName + "_ShouldUnequip", 0);  // Reset the flag
        }

        UpdateUI();
    }

    public void UnlockPowerUp()
    {
        Debug.Log("Attempting to unlock... Coins before unlock: " + CoinManager.instance.totalCoins);

        if (!IsUnlocked && CoinManager.instance.totalCoins >= UnlockCost)
        {
            CoinManager.instance.totalCoins -= UnlockCost;
            IsUnlocked = true;

            CoinManager.instance.SaveCoins();
            CoinManager.instance.UpdateCoinTexts();

            PlayerPrefs.SetInt(powerUpName + "_IsUnlocked", 1); // Save unlocked state

            Debug.Log("Unlocked! Coins after unlock: " + CoinManager.instance.totalCoins);
        }
        else
        {
            Debug.Log("Unlocking failed. Either already unlocked or insufficient coins.");
            MessageAnimation.SetTrigger("Open");
            BlackScreen.SetActive(true);
        }

        UpdateUI();
    }

    public void BuyPowerUp()
    {
        Debug.Log("Attempting to buy... Coins before purchase: " + CoinManager.instance.totalCoins);

        if (IsUnlocked && CoinManager.instance.totalCoins >= PurchaseCost)
        {
            CoinManager.instance.totalCoins -= PurchaseCost;
            Quantity++;

            CoinManager.instance.SaveCoins();
            CoinManager.instance.UpdateCoinTexts();

            PlayerPrefs.SetInt(powerUpName + "_Quantity", Quantity); // Save quantity

            Debug.Log("Purchased! Coins after purchase: " + CoinManager.instance.totalCoins);
        }
        else
        {
            Debug.Log("Purchase failed. Either not unlocked or insufficient coins.");
            MessageAnimation.SetTrigger("Open");
            BlackScreen.SetActive(true);
        }

        UpdateUI();
    }

    public void EquipPowerUp()
    {
        Debug.Log("Equip button pressed for power: " + powerUpName);

        if (Quantity > 0 && !IsEquipped && SlotManager.instance.EquipPower(powerUpName, slotSpaceRequired))
        {
            Debug.Log("Equipped power: " + powerUpName);
            // If power is successfully equipped, decrement quantity
            IsEquipped = true;  // Set equipped flag
            PlayerPrefs.SetInt(powerUpName + "_IsEquipped", 1);  // Save equipped state
            UpdateUI();
        }
        else
        {
            Debug.Log("Failed to equip power: " + powerUpName);
        }

    }

    public void UnequipPowerUp()
    {
        SlotManager.instance.UnequipPower(powerUpName);
        IsEquipped = false;  // Reset equipped flag
        PlayerPrefs.SetInt(powerUpName + "_IsEquipped", 0);  // Save unequipped state
        UpdateUI();
    }

    public void UsePowerUp()
    {
        if (Quantity > 0)
        {
            Quantity--;
            PlayerPrefs.SetInt(powerUpName + "_Quantity", Quantity); // Save updated quantity
            UpdateUI();
        }
    }


    public void UpdateUI()
    {
        unlockButton.gameObject.SetActive(!IsUnlocked);
        buyButton.gameObject.SetActive(IsUnlocked);

        // Explicitly check if the power-up is unlocked
        if (IsUnlocked)
        {
            equipButton.gameObject.SetActive(!IsEquipped);  // Show equip button if not equipped and power-up is unlocked
            unequipButton.gameObject.SetActive(IsEquipped);  // Show unequip button if equipped and power-up is unlocked
        }
        else
        {
            equipButton.gameObject.SetActive(false);  // Ensure equip button is always off if power-up is not unlocked
            unequipButton.gameObject.SetActive(false); // Ensure unequip button is always off if power-up is not unlocked
        }

        unlockPriceText.text = UnlockCost.ToString();
        buyPriceText.text = PurchaseCost.ToString();
        powerUpQuantityText.text = Quantity.ToString();
    }

}



