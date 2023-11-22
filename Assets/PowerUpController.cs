using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController instance;

    public PlayerController playerController; // Reference to the PlayerController
    public CoinManager coinManager;

    public string magnetCoinsUpgradeName;
    public string coinUpgradeName;

    public AudioSource ShieldBreakSound;
    public ParticleSystem ShieldBreak;
    public string shieldName;
    public float invincibilityDuration = 3f;
    public float blinkSpeed = 0.1f;

    private GameObject shieldSprite;
    private bool hasShield = false;
    private SpriteRenderer spriteRenderer;

    public float slowFallMultiplier = 0.5f; // This multiplies the falling speed by half (or 50% reduction in speed)
    private bool isSlowFallingActive = false;

    public float timeIncreaseAmount = 5f; // Time to add to the timer when the power-up is activated.

    private bool isMagnetActive = false;
    public float magnetForce = 5f; // The force with which coins are attracted. Adjust as needed.
    public float magnetRange = 3f; // The radius within which coins will be attracted. Adjust as needed.
    public float magnetDuration = 5f; // Duration of the magnet effect, in seconds
    private Coroutine deactivateMagnetCoroutine;

    public float CoinMDuration = 5f;
    private Coroutine deactivateCoinMCoroutine;
    private bool isCoinMultiplayerActive = false;

    private float magnetElapsedTime = 0f;
    private float coinMultiplierElapsedTime = 0f;

    public AudioSource powerUpSound;
    public AudioSource UpgradesUpSound;

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

    void Start()
    {
        shieldSprite = transform.Find(shieldName).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();



        if (hasShield)
        {
            shieldSprite.SetActive(true); // Enable the shield sprite
        }
        else
        {
            shieldSprite.SetActive(false); // Disable the shield sprite
        }
    }

    public void EnableShield()
    {
        hasShield = true;
        shieldSprite.SetActive(true); // Enable the shield sprite
        powerUpSound.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Deadly"))
        {
            if (hasShield)
            {
                BreakShield();
            }
            else
            {
                // Signal the PlayerController to handle the death
                GetComponent<PlayerController>().Die();
            }
        }
    }


    public void BreakShield()
    {
        hasShield = false;
        shieldSprite.SetActive(false); // This will disable your shield sprite
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(10f, 10f), ForceMode2D.Impulse);
        StartCoroutine(MakeInvincible(invincibilityDuration));
        ShieldBreak.Play();
        ShieldBreakSound.Play();
    }

    private IEnumerator MakeInvincible(float duration)
    {


        // Find all obstacles and disable their colliders
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Deadly");
        List<Collider2D> obstacleColliders = new List<Collider2D>();
        foreach (GameObject obstacle in obstacles)
        {
            Collider2D collider = obstacle.GetComponent<Collider2D>();
            if (collider != null)
            {
                obstacleColliders.Add(collider);
                collider.enabled = false;
            }
        }

        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            // Make the player transparent
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            yield return new WaitForSeconds(blinkSpeed);

            // Make the player opaque
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            yield return new WaitForSeconds(blinkSpeed);
        }

        // Ensure the player is opaque when invincibility ends
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        // After the invincibility period, re-enable all the colliders
        foreach (Collider2D collider in obstacleColliders)
        {
            if (collider != null) // Check in case the object was destroyed while invincible
            {
                collider.enabled = true;
            }
        }
    }

    public bool HasShield()
    {
        return hasShield;
    }

    public void ActivateSlowFall()
    {
        isSlowFallingActive = true;
        playerController.AdjustFallingSpeed(slowFallMultiplier);
        powerUpSound.Play();
    }

    public void DeactivateSlowFall()
    {
        isSlowFallingActive = false;
        playerController.ResetFallingSpeed();
    }

    public bool IsSlowFallingActive()
    {
        return isSlowFallingActive;
    }

    public void ActivateTimerPlus()
    {
        GameManager1.instance.AddTimeToTimer(timeIncreaseAmount);
        powerUpSound.Play();
    }

    public void ActivateMagnet()
    {
        int magnetLevel = PlayerPrefs.GetInt(magnetCoinsUpgradeName + "_CurrentLevel", 0);

        UpgradesUpSound.Play();
        isMagnetActive = true;

        // Adjust the magnet's effects based on its level
        switch (magnetLevel)
        {
            case 1:
                magnetForce = 5f;
                magnetRange = 1f;
                magnetDuration = 5f;
                break;
            case 2:
                magnetForce = 5f;
                magnetRange = 2f;
                magnetDuration = 7f;
                break;
            case 3:
                magnetForce = 10f;
                magnetRange = 2f;
                magnetDuration = 9f;
                break;
            case 4:
                magnetForce = 10f;
                magnetRange = 3f;
                magnetDuration = 11f;
                break;
            case 5:
                magnetForce = 15f;
                magnetRange = 3f;
                magnetDuration = 13f;
                break;
            case 6:
                magnetForce = 15f;
                magnetRange = 4f;
                magnetDuration = 15f;
                break;
            default:
                break;
        }

        magnetElapsedTime = 0f; // Reset elapsed time

        if (deactivateMagnetCoroutine != null)
        {
            StopCoroutine(deactivateMagnetCoroutine);
        }

        // Start a new coroutine to deactivate the magnet
        deactivateMagnetCoroutine = StartCoroutine(DeactivateMagnetAfterDelay());
    }

    public bool IsMagnetActive()
    {
        return isMagnetActive;
    }

    public float GetMagnetRemainingTime()
    {
        return Mathf.Max(magnetDuration - magnetElapsedTime, 0);
    }

    private IEnumerator DeactivateMagnetAfterDelay()
    {
        while (magnetElapsedTime < magnetDuration)
        {
            magnetElapsedTime += Time.deltaTime;
            yield return null;
        }
        isMagnetActive = false;
    }

    public void UpdateCoinMultiplier()
    {
        int coinUpgradeLevel = PlayerPrefs.GetInt(coinUpgradeName + "_CurrentLevel", 0);

        isCoinMultiplayerActive = true;
        UpgradesUpSound.Play();

        switch (coinUpgradeLevel)
        {
            case 1:
                CoinManager.instance.coinMultiplier = 2; // Double the coins
                CoinMDuration = 5f;
                break;
            case 2:
                CoinManager.instance.coinMultiplier = 3; // Triple the coins
                CoinMDuration = 7f;
                break;
            case 3:
                CoinManager.instance.coinMultiplier = 4; // And so on...
                CoinMDuration = 9f;
                break;
            case 4:
                CoinManager.instance.coinMultiplier = 5; // And so on...
                CoinMDuration = 11f;
                break;
            case 5:
                CoinManager.instance.coinMultiplier = 6; // And so on...
                CoinMDuration = 13f;
                break;
            case 6:
                CoinManager.instance.coinMultiplier = 7; // And so on...
                CoinMDuration = 15f;
                break;
            default:
                CoinManager.instance.coinMultiplier = 2;
                CoinMDuration = 5f;
                break;
        }

        coinMultiplierElapsedTime = 0f; // Reset elapsed time

        if (deactivateCoinMCoroutine != null)
        {
            StopCoroutine(deactivateCoinMCoroutine);
        }

        // Start a new coroutine to deactivate the magnet
        deactivateCoinMCoroutine = StartCoroutine(DeactivateCoinMAfterDelay());
    }

    public bool isCoinMultiPlayerActive()
    {
        return isCoinMultiplayerActive;
    }

    private IEnumerator DeactivateCoinMAfterDelay()
    {
        while (coinMultiplierElapsedTime < CoinMDuration)
        {
            coinMultiplierElapsedTime += Time.deltaTime;
            yield return null;
        }
        isCoinMultiplayerActive = false;
        CoinManager.instance.coinMultiplier = 1;
    }

    public float GetCoinMultiplierRemainingTime()
    {
        return Mathf.Max(CoinMDuration - coinMultiplierElapsedTime, 0);
    }
}