using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class SoundButtonController : MonoBehaviour
{
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public GameObject playerObject;  // Reference to your player GameObject
    public GameObject[] otherSoundObjects; // Array for other sound objects
    private AudioSource[] soundSources;  // Array of AudioSources for the sound effects
    private bool isSoundOn;
    public AudioSource SoundSound;

    void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("isSoundOn", 1) == 1 ? true : false;
        List<AudioSource> sources = new List<AudioSource>(); // Initialize list of AudioSources

        // Add playerObject's AudioSources to list
        sources.AddRange(playerObject.GetComponentsInChildren<AudioSource>());

        // Add otherSoundObjects' AudioSources to list
        foreach (GameObject soundObject in otherSoundObjects)
        {
            sources.AddRange(soundObject.GetComponentsInChildren<AudioSource>());
        }

        soundSources = sources.ToArray(); // Convert list to array
        UpdateButtonSprite();
        SetSoundMuteState(!isSoundOn); // Set the initial mute state of the AudioSources
    }

    public void OnButtonPress()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("isSoundOn", isSoundOn ? 1 : 0);
        UpdateButtonSprite();
        SetSoundMuteState(!isSoundOn); // Toggle the mute state of the AudioSources when the button is pressed
        SoundSound.Play();
    }

    void UpdateButtonSprite()
    {
        GetComponent<Image>().sprite = isSoundOn ? soundOnSprite : soundOffSprite;
    }

    void SetSoundMuteState(bool mute)
    {
        foreach (AudioSource source in soundSources)
        {
            source.mute = mute;
        }
    }
}



