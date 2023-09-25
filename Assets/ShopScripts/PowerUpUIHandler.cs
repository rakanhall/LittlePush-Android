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

    private void Start()
    {
        // Load values from PlayerPrefs
        IsUnlocked = PlayerPrefs.GetInt(powerUpName + "_IsUnlocked", 0) == 1;
        Quantity = PlayerPrefs.GetInt(powerUpName + "_Quantity", 0);
        IsEquipped = PlayerPrefs.GetInt(powerUpName + "_IsEquipped", 0) == 1;

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
        }

        UpdateUI();
    }

    public void EquipPowerUp()
    {
        if (!IsEquipped)
        {
            IsEquipped = true;
            PlayerPrefs.SetInt(powerUpName + "_IsEquipped", 1); // Save equipped state
            UpdateUI();
        }
    }

    public void UnequipPowerUp()
    {
        if (IsEquipped)
        {
            IsEquipped = false;
            PlayerPrefs.SetInt(powerUpName + "_IsEquipped", 0); // Save unequipped state
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        unlockButton.gameObject.SetActive(!IsUnlocked);
        buyButton.gameObject.SetActive(IsUnlocked);
        equipButton.gameObject.SetActive(IsUnlocked && !IsEquipped);
        unequipButton.gameObject.SetActive(IsUnlocked && IsEquipped);

        unlockPriceText.text = UnlockCost.ToString();
        buyPriceText.text = PurchaseCost.ToString();
        powerUpQuantityText.text = Quantity.ToString();
    }
}



