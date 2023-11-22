using System.Collections;
using UnityEngine;

public class CoinMulti : MonoBehaviour
{
    public float probability = 0.9f;
    public CircleCollider2D circleCollider;

    private Vector3 originalPosition;

    private void Start()
    {
        bool isEnabled = Random.value < probability;

        gameObject.SetActive(isEnabled);

        if (isEnabled)
        {
            originalPosition = transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PowerUpController.instance.UpdateCoinMultiplier();
            Destroy(gameObject);
        }
    }

}
