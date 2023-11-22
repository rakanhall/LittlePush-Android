using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeItemUIHandler : MonoBehaviour
{
    // Upgrade Properties
    public string upgradeName;
    public int[] upgradeCosts = new int[6];  // Costs for Level 1, 2, and 3
    public int currentLevel = 1;  // Start from level 1

    // UI Components
    public Button upgradeButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradePriceText;

    public Animator MessageAnimation;
    public GameObject BlackScreen;

    private void Start()
    {
        // Load values from PlayerPrefs
        currentLevel = PlayerPrefs.GetInt(upgradeName + "_CurrentLevel", 1);

        UpdateUI();
    }

    public void UpgradeItem()
    {
        if (currentLevel < 6 && CoinManager.instance.totalCoins >= upgradeCosts[currentLevel - 1])
        {
            CoinManager.instance.totalCoins -= upgradeCosts[currentLevel - 1];
            currentLevel++;

            CoinManager.instance.SaveCoins();
            CoinManager.instance.UpdateCoinTexts();

            PlayerPrefs.SetInt(upgradeName + "_CurrentLevel", currentLevel);
        }
        else
        {
            Debug.Log("Upgrade failed. Either at max level or insufficient coins.");

            MessageAnimation.SetTrigger("Open");
            BlackScreen.SetActive(true);
        }

        UpdateUI();
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void UpdateUI()
    {
        upgradeButton.gameObject.SetActive(currentLevel < 6);

        if (currentLevel < 6)
        {
            levelText.text = "Level " + currentLevel;
            upgradePriceText.text = upgradeCosts[currentLevel - 1].ToString();
        }
        else
        {
            levelText.text = "MAX";
            upgradePriceText.text = "";
        }
    }
}



