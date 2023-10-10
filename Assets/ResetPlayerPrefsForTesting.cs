using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayerPrefsForTesting : MonoBehaviour
{
    public void Deleteem()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs have been reset!");
    }

}
