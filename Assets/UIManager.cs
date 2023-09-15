using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject scoreMenu;
    public float duration = 1f;
    public Vector3 targetPosition = new Vector3(0, 0, 0);

    public void ShowScoreMenu()
    {
        LeanTween.move(scoreMenu.GetComponent<RectTransform>(), targetPosition, duration)
            .setEase(LeanTweenType.easeInOutQuad);
    }
}



