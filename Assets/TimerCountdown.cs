using TMPro;
using UnityEngine;

public class TimerCountdown : MonoBehaviour
{
    [SerializeField] private TextMeshPro timerText; // Reference to the TextMeshPro component

    private GameManager1 gameManager; // Reference to the GameManager

    private void Start()
    {
        // Find the GameManager in the scene and store a reference to it
        gameManager = FindObjectOfType<GameManager1>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    private void Update()
    {
        // Update the text with the current timer value from the GameManager
        if (gameManager != null && timerText != null)
        {
            timerText.text = Mathf.RoundToInt(gameManager.GetTimerValue()).ToString();
        }
    }
}



