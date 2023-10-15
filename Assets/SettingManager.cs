using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public Animator settingsMenuAnimator;
    public GameObject BlackScreen;

    public void OpenSettingsMenu()
    {
        BlackScreen.SetActive(true);
        settingsMenuAnimator.SetTrigger("Open");
    }

    public void CloseSettingsMenu()
    {
        BlackScreen.SetActive(false);
        settingsMenuAnimator.SetTrigger("Close");
    }
}