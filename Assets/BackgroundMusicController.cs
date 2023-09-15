using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusicController : MonoBehaviour
{
    public AudioClip gameClip; // assign in inspector, plays throughout the game

    public float startVolume = 1f; // volume at the start of the game
    public float gameplayVolume = 0.7f; // volume during gameplay
    public float deathVolume = 0.3f; // volume when player dies

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameClip;
        audioSource.loop = true;
        audioSource.volume = startVolume;
        audioSource.Play();
    }

    public void ChangeToGameplayVolume()
    {
        audioSource.volume = gameplayVolume;
    }

    public void ChangeToDeathVolume()
    {
        audioSource.volume = deathVolume;
    }
}


