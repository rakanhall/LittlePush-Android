using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    public float levitationSpeed = 1.0f;
    public float fadeSpeed = 1.0f;
    public float duration = 1.0f;

    private Vector3 originalPosition;
    private float startTime;

    private TextMeshPro textObject;

    private void Start()
    {
        originalPosition = transform.position;
        startTime = Time.time;
        textObject = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        if (textObject == null)
            return;

        float journeyLength = (Time.time - startTime) * levitationSpeed;

        // Calculate the new Y position for levitation
        float newY = originalPosition.y + journeyLength;

        // Update the text's position
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);

        // Gradually decrease the alpha over time
        float newAlpha = 1.0f - (journeyLength * fadeSpeed);
        textObject.alpha = Mathf.Clamp01(newAlpha);

        if (journeyLength >= duration)
        {
            // Destroy the text object when the animation is complete
            Destroy(gameObject);
        }
    }
}

