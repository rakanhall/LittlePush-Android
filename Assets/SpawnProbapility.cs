using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevitatingCoin : MonoBehaviour
{
    public float probability = 0.9f;
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider2D;
    public TextMeshPro plusOneTextPrefab;

    private Vector3 originalPosition;

    private void Start()
    {
        bool isEnabled = Random.value < probability;

        // Disable the entire GameObject if the probability condition is not met
        if (!isEnabled)
        {
            gameObject.SetActive(false);
            return; // Early return to skip setting up the coin
        }

        // If enabled, record the original position
        originalPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
            Destroy(gameObject);
        }
    }

    private void CollectCoin()
    {
        CoinManager coinManager = CoinManager.instance; // Get a reference to the CoinManager
        if (coinManager != null)
        {
            int coinMultiplier = coinManager.GetCoinMultiplier(); // Get the coin multiplier value
            TextMeshPro plusOneText = Instantiate(plusOneTextPrefab, transform.position, Quaternion.identity);
            plusOneText.text = "+" + coinMultiplier.ToString(); // Set the text to the coin multiplier
                                                                // You may want to set the sorting order of plusOneText here to make it appear above other objects.
        }
        else
        {
            Debug.LogWarning("CoinManager not found in the scene.");
        }
    }
}






