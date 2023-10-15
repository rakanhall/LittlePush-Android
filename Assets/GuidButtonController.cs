using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuidButtonController : MonoBehaviour
{
    public void OnGuidButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}
