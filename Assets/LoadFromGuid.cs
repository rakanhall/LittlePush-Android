using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFromGuid : MonoBehaviour
{
    public int SceneNum = 0;

    public void ReturnHome()
    {
        SceneManager.LoadScene(SceneNum);
    }
}
