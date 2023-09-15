using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingButton : MonoBehaviour
{
    public Animator animator;
    public Button button;
    public AudioSource SettingSound;
    private bool isPressed = false;

    void Start()
    {
        button.onClick.AddListener(PressButton);
    }

    public void PressButton()
    {
        isPressed = !isPressed;
        animator.SetBool("isPressed", isPressed);
        SettingSound.Play();
    }

    public void HideButton()
    {
        animator.SetBool("isGone", true);
        
    }
}






