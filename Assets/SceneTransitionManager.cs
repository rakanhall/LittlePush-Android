using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public Animator transitionAnimator;
    public float transitionTime = 1f;
    public int SceneNum = 1;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        // Play the fade-in animation
        transitionAnimator.SetTrigger("FadeIn");

        // Wait for the animation to finish
        yield return new WaitForSeconds(transitionTime);

        // Load the new scene
        SceneManager.LoadScene(SceneNum);
    }
}
