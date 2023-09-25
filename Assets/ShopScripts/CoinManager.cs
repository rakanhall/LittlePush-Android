using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int totalCoins;
    public int coinsThisGame;
    public TextMeshProUGUI totalCoinsText;
    public TextMeshProUGUI gameplayCoinsText;
    public TextMeshProUGUI gameplayCoinsTextEndMenu;

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

        LoadCoins();
        UpdateCoinTexts();
    }

    public void CollectCoin()
    {
        coinsThisGame += 1;
        totalCoins += 1;

        if (gameplayCoinsText != null)
            gameplayCoinsText.text = coinsThisGame.ToString();

        if (gameplayCoinsTextEndMenu != null)
            gameplayCoinsTextEndMenu.text = coinsThisGame.ToString();

        SaveCoins();
    }

    public void ResetGameCoins()
    {
        coinsThisGame = 0;
        if (gameplayCoinsText != null)
            gameplayCoinsText.text = coinsThisGame.ToString();

        if (gameplayCoinsTextEndMenu != null)
            gameplayCoinsTextEndMenu.text = coinsThisGame.ToString();
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
    }

    private void LoadCoins()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }

    public void UpdateCoinTexts()
    {
        if (totalCoinsText != null)
            totalCoinsText.text = totalCoins.ToString();

        if (gameplayCoinsText != null)
            gameplayCoinsText.text = coinsThisGame.ToString();

        if (gameplayCoinsTextEndMenu != null)
            gameplayCoinsTextEndMenu.text = coinsThisGame.ToString();
    }
}

