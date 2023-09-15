using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : MonoBehaviour
{
    // Reference to the button
    public Button adsButton;

    private void Start()
    {
        // Add a listener to the button's click event
        adsButton.onClick.AddListener(ResetAds);
    }

    public void ResetAds()
    {
        // Reset the AdsRemoved property to false
        IAPManager.AdsRemoved = false;
        Debug.Log("Ads have been reset.");
    }
}

