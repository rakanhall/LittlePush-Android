using UnityEngine;
using UnityEngine.UI;

public class ContinuousFade : MonoBehaviour
{
    [SerializeField]
    private float minAlpha = 0.2f;  // Minimum alpha value.
    [SerializeField]
    private float maxAlpha = 1.0f;  // Maximum alpha value.
    [SerializeField]
    private float fadeDuration = 1.0f;  // Duration for the fade effect.

    private CanvasRenderer canvasRenderer;
    private bool isFadingIn = false;  // Track fade direction.

    private void Awake()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();
    }

    private void Update()
    {
        HandleFade();
    }

    private void HandleFade()
    {
        float alphaChange = (maxAlpha - minAlpha) * (Time.deltaTime / fadeDuration);

        if (isFadingIn)
        {
            canvasRenderer.SetAlpha(canvasRenderer.GetAlpha() + alphaChange);
            if (canvasRenderer.GetAlpha() >= maxAlpha)
            {
                canvasRenderer.SetAlpha(maxAlpha);
                isFadingIn = false;
            }
        }
        else
        {
            canvasRenderer.SetAlpha(canvasRenderer.GetAlpha() - alphaChange);
            if (canvasRenderer.GetAlpha() <= minAlpha)
            {
                canvasRenderer.SetAlpha(minAlpha);
                isFadingIn = true;
            }
        }
    }
}


