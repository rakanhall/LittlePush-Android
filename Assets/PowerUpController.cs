using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerUpController : MonoBehaviour
{
    public PlayerController playerController; // Reference to the PlayerController
    public AudioSource ShieldBreakSound;
    public AudioSource ShieldButtonSound;
    public ParticleSystem ShieldBreak;
    public GameObject ShieldButton;
    public string shieldName;
    public float invincibilityDuration = 3f;
    public float blinkSpeed = 0.1f;

    private GameObject shieldSprite;
    private bool hasShield = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        shieldSprite = transform.Find(shieldName).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Load shield status
        hasShield = PlayerPrefs.GetInt("hasShield", 0) == 1;

        if (hasShield)
        {
            shieldSprite.SetActive(true); // Enable the shield sprite
            ShieldButton.SetActive(false);
        }
        else
        {
            shieldSprite.SetActive(false); // Disable the shield sprite
            ShieldButton.SetActive(true);
        }
    }

    public void EnableShield()
    {
        PlayerPrefs.SetInt("hasShield", 1); // Save shield status
        hasShield = true;
        shieldSprite.SetActive(true); // Enable the shield sprite
        ShieldButtonSound.Play();
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
        PlayerPrefs.SetInt("hasShield", 0); // Save shield status
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
}

