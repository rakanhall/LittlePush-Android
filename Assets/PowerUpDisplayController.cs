using UnityEngine;
using UnityEngine.UI;

public class PowerUpDisplayController : MonoBehaviour
{
    public GameObject magnetIcon; // Assign in Unity Editor
    public GameObject coinMultiplierIcon; // Assign in Unity Editor
    public Transform spot1; // Assign in Unity Editor
    public Transform spot2; // Assign in Unity Editor
    public Image magnetTimerBar; // Assign in Unity Editor
    public Image coinMultiplierTimerBar; // Assign in Unity Editor

    private PowerUpController powerUpController;
    private PlayerController playerController;
    private GameObject firstActivePowerUp;

    private void Start()
    {
        powerUpController = FindObjectOfType<PowerUpController>();
        playerController = FindObjectOfType<PlayerController>();

        // Initially hide the icons
        magnetIcon.SetActive(false);
        coinMultiplierIcon.SetActive(false);
        firstActivePowerUp = null;
    }

    private void Update()
    {
        UpdatePowerUpIcons();

        if (playerController.IsDead) // Assuming you have an IsDead property or method in PlayerController
        {
            magnetIcon.SetActive(false);
            coinMultiplierIcon.SetActive(false);
        }
    }

    private void UpdatePowerUpIcons()
    {
        // Update positions and visibility based on current power-up states
        UpdateIconPositionAndVisibility(magnetIcon, powerUpController.IsMagnetActive(), magnetTimerBar, powerUpController.GetMagnetRemainingTime() / powerUpController.magnetDuration);
        UpdateIconPositionAndVisibility(coinMultiplierIcon, powerUpController.isCoinMultiPlayerActive(), coinMultiplierTimerBar, powerUpController.GetCoinMultiplierRemainingTime() / powerUpController.CoinMDuration);
    }

    private void UpdateIconPositionAndVisibility(GameObject icon, bool isActive, Image timerBar, float fillAmount)
    {
        if (isActive)
        {
            if (firstActivePowerUp == null || firstActivePowerUp == icon)
            {
                firstActivePowerUp = icon;
                icon.transform.position = spot1.position;
            }
            else
            {
                icon.transform.position = spot2.position;
            }
            icon.SetActive(true);
            timerBar.fillAmount = fillAmount;
        }
        else
        {
            if (firstActivePowerUp == icon)
            {
                firstActivePowerUp = null;
                // If the other icon is active, move it to spot1
                if (icon == magnetIcon && powerUpController.isCoinMultiPlayerActive())
                    coinMultiplierIcon.transform.position = spot1.position;
                else if (icon == coinMultiplierIcon && powerUpController.IsMagnetActive())
                    magnetIcon.transform.position = spot1.position;
            }
            icon.SetActive(false);
        }
    }
}





