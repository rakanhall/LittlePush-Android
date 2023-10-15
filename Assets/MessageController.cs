using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    public Animator MessageAnimation;
    public GameObject Message;
    public GameObject BlackScreen;

    public void ExitMessage()
    {
        MessageAnimation.SetTrigger("Close");
        BlackScreen.SetActive(false);

    }

}
