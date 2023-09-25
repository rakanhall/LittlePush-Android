using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToGame : MonoBehaviour
{
    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
