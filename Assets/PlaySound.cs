using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource jumpSound;
    public AudioSource clickSound;

    public void PlayJumpSound()
    {
        jumpSound.Play();
    }

    public void PlayClickSound()
    {
        clickSound.Play();
    }
}

