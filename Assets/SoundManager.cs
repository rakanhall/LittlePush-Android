using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] audioSources;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // You could add DontDestroyOnLoad here if you want it to persist between scenes
    }

    public void PlaySound(int soundToPlay)
    {
        if (soundToPlay < audioSources.Length)
        {
            audioSources[soundToPlay].Play();
        }
    }
}

