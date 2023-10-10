using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemUIHandler : MonoBehaviour
{
    // Upgrade Properties
    public string upgradeName;
    public int[] upgradeCosts = new int[3];  // Costs for Level 1, 2, and 3
    public bool isUnlocked = false;
    public int currentLevel = 0;  // 0 means not unlocked, 1 to 3 for each level

    // UI Components
    public Button unlockUpgradeButton;
    public Button upgradeButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradePriceText;



    private void Start()
    {
        // Load values from PlayerPrefs
        isUnlocked = PlayerPrefs.GetInt(upgradeName + "_IsUnlocked", 0) == 1;
        currentLevel = PlayerPrefs.GetInt(upgradeName + "_CurrentLevel", 0);

        UpdateUI();
    }

    public void UnlockUpgrade()
    {
        if (!isUnlocked && CoinManager.instance.totalCoins >= upgradeCosts[0])
        {
            CoinManager.instance.totalCoins -= upgradeCosts[0];
            isUnlocked = true;
            currentLevel = 1;

            CoinManager.instance.SaveCoins();
            CoinManager.instance.UpdateCoinTexts();

            PlayerPrefs.SetInt(upgradeName + "_IsUnlocked", 1);
            PlayerPrefs.SetInt(upgradeName + "_CurrentLevel", currentLevel);

        }

        UpdateUI();
    }

    public void UpgradeItem()
    {
        if (isUnlocked && currentLevel < 3 && CoinManager.instance.totalCoins >= upgradeCosts[currentLevel])
        {
            CoinManager.instance.totalCoins -= upgradeCosts[currentLevel];
            currentLevel++;

            CoinManager.instance.SaveCoins();
            CoinManager.instance.UpdateCoinTexts();

            PlayerPrefs.SetInt(upgradeName + "_CurrentLevel", currentLevel);
        }

        UpdateUI();
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }


    public void UpdateUI()
    {
        unlockUpgradeButton.gameObject.SetActive(!isUnlocked);
        upgradeButton.gameObject.SetActive(isUnlocked && currentLevel < 3);

        if (currentLevel == 0)
        {
            levelText.text = "";
            upgradePriceText.text = upgradeCosts[0].ToString();
        }
        else if (currentLevel < 3)
        {
            levelText.text = "Level " + currentLevel;
            upgradePriceText.text = upgradeCosts[currentLevel].ToString();
        }
        else
        {
            levelText.text = "MAX";
            upgradePriceText.text = "";
        }
    }
}

