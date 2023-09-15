using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GuideController : MonoBehaviour
{
    public Button returnButton; // Assign this in the Inspector
    public float guideAnimationLength = 5.0f; // Set this to the length of your guide animation

    private void Start()
    {
        returnButton.interactable = false; // Disable the button
        Invoke("EnableReturnButton", guideAnimationLength); // Enable the button after the animation
    }

    private void EnableReturnButton()
    {
        returnButton.interactable = true; // Enable the button
    }

    public void ReturnToGame()
    {
        SceneManager.LoadScene("Game"); // Replace "GameScene" with the name of your game scene
    }
}

