using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Make sure to include this for TextMesh Pro

public class AfterBuying : MonoBehaviour
{
    public GameObject BlackScreenAfter;
    public GameObject purchaseMessage;
    public Animator PurshaseMessgaeAnimation; 
    public TMP_Text purchaseText; // Use TMP_Text instead of Text

    public void TransactionStart()
    {
        BlackScreenAfter.SetActive(true);
    }

    public void TransactionSuccessfull()
    {
        purchaseText.text = "Purchase successful"; // Corrected this line
        BlackScreenAfter.SetActive(false);
        PurshaseMessgaeAnimation.SetTrigger("Go");
    }

    public void Transactionfailed()
    {
        purchaseText.text = "Purchase failed"; // Corrected this line
        BlackScreenAfter.SetActive(false);
        PurshaseMessgaeAnimation.SetTrigger("Go");
    }
}

