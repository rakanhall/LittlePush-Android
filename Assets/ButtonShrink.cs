using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonShrink : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    private float shrinkScale = 0.9f;
    [SerializeField]
    private float animationDuration = 0.1f;

    private Vector3 originalScale;
    private Vector3 targetScale;
    private Coroutine animationCoroutine;

    private void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale * shrinkScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartAnimation(targetScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartAnimation(originalScale);
    }

    // This event ensures that the button returns to its original size even if the touch/mouse pointer moves outside the button before releasing.
    public void OnPointerExit(PointerEventData eventData)
    {
        StartAnimation(originalScale);
    }

    private void StartAnimation(Vector3 target)
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            transform.localScale = target; // Ensure you set to the desired target scale when stopping.
        }
        animationCoroutine = StartCoroutine(ScaleAnimation(target));
    }

    private System.Collections.IEnumerator ScaleAnimation(Vector3 target)
    {
        float timeElapsed = 0;
        Vector3 startScale = transform.localScale;

        while (timeElapsed < animationDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, target, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = target;
    }
}


