using UnityEngine;

public class TimerItemProbability : MonoBehaviour
{
    public float probability = 0.9f;  // The chance that the Timer Item is enabled. Set this between 0 and 1 in the Inspector.
    public SpriteRenderer spriteRenderer;  // The Timer Item's SpriteRenderer
    public CircleCollider2D circleCollider2D;  // The Timer Item's CircleCollider2D

    private void Start()
    {
        // Determine whether or not the Timer Item should be enabled
        bool isEnabled = Random.value < probability;

        // Enable or disable the Timer Item's SpriteRenderer and CircleCollider2D
        spriteRenderer.enabled = isEnabled;
        circleCollider2D.enabled = isEnabled;
    }
}
