using UnityEngine;

public class AnimationSound : MonoBehaviour
{
    

    public void PlaySound()
    {
        SoundManager.instance.PlaySound(2);
    }
}

