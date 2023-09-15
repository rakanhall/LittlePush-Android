using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuidButtonController : MonoBehaviour
{
    public AudioSource guidbuttonsound;
    public void OnGuidButtonClicked()
    {
        guidbuttonsound.Play();
        SceneManager.LoadScene(1);
    }
}
