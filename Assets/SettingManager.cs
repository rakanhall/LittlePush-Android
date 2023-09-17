using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public Animator settingsMenuAnimator;
    public GameObject BlackScreen;

    // Start is called before the first frame update
    void Start()
    {
        // Disable the GameObject to prevent the "Closed" animation from playing
        gameObject.SetActive(false);
        BlackScreen.SetActive(false);
    }

    public void OpenSettingsMenu()
    {
        // Enable the GameObject before opening the menu
        gameObject.SetActive(true);
        BlackScreen.SetActive(true);
        settingsMenuAnimator.SetBool("Open", true);
    }

    public void CloseSettingsMenu()
    {
        settingsMenuAnimator.SetBool("Open", false);
        BlackScreen.SetActive(false);
    }
}