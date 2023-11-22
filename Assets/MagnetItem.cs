using System.Collections;
using UnityEngine;

public class MagnetItem : MonoBehaviour
{
    public float probability = 0.9f;
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;

    private Vector3 originalPosition;

    private void Start()
    {
        bool isEnabled = Random.value < probability;
        spriteRenderer.enabled = isEnabled;
        circleCollider.enabled = isEnabled;

        if (isEnabled)
        {
            originalPosition = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpController.instance.ActivateMagnet();
            Destroy(gameObject);
        }
    }

}

