using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StroreController : MonoBehaviour
{
    public GameObject BlackScreem;
    public Animator animator;
    public Animator Message;

    public void openStore()
    {
        animator.SetTrigger("Open");
        BlackScreem.SetActive(true);
    }

    public void closeStore()
    {
        animator.SetTrigger("Close");
        BlackScreem.SetActive(false);
    }

    public void openMessageStore()
    {
        Message.SetTrigger("Close");
        animator.SetTrigger("Open");
        BlackScreem.SetActive(true);
    }

}
