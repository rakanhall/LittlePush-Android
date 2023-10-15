using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioSource Buttonmainsound;
    public AudioSource Buttonexitsound;

    public void ButtonMain()
    {
        Buttonmainsound.Play();
    }

    public void Buttonexit()
    {
        Buttonexitsound.Play();
    }

}
