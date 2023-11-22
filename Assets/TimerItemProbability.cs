using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerItemProbability : MonoBehaviour
{
    public float probability = 0.9f;  // The chance that the Timer Item is enabled. Set this between 0 and 1 in the Inspector.

    private Vector3 originalPosition;

    private void Start()
    {
        // Determine whether or not the Timer Item should be enabled
        bool isEnabled = Random.value < probability;

        // Enable or disable the entire Timer Item game object
        gameObject.SetActive(isEnabled);

        if (isEnabled)
        {
            originalPosition = transform.position;
        }
    }
}

