using UnityEngine;
using UnityEngine.UI;

public class MusicButtonController : MonoBehaviour
{
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public AudioSource musicSource;  // Reference to your AudioSource for the music
    private bool isMusicOn;
    public AudioSource MusicSound;

    void Start()
    {
        isMusicOn = PlayerPrefs.GetInt("isMusicOn", 1) == 1 ? true : false;
        UpdateButtonSprite();
        musicSource.mute = !isMusicOn; // Set the initial mute state of the AudioSource
    }

    public void OnButtonPress()
    {
        isMusicOn = !isMusicOn;
        PlayerPrefs.SetInt("isMusicOn", isMusicOn ? 1 : 0);
        UpdateButtonSprite();
        musicSource.mute = !isMusicOn; // Toggle the mute state of the AudioSource when the button is pressed
        MusicSound.Play();
    }

    void UpdateButtonSprite()
    {
        GetComponent<Image>().sprite = isMusicOn ? musicOnSprite : musicOffSprite;
    }
}


