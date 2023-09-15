using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    public Vector2 offset = new Vector2(0f, 0f);

    // Camera movement limit
    public float minX;
    public float maxX;

    private Vector3 lastTargetPosition;
    private PlayerController playerController;
    private bool isTransitioning = true; // Flag to mark if the camera is in transition

    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: No target assigned!");
            return;
        }

        playerController = target.GetComponent<PlayerController>();
        lastTargetPosition = target.position;

        // Start the camera transition
        StartCoroutine(TransitionToTarget(2f));
    }

    private void Update()
    {
        // If camera is transitioning or target is null or the player is dead, we return early to avoid following behavior
        if (isTransitioning || target == null || playerController.IsDead) return;

        float targetPositionX = Mathf.Clamp(target.position.x + offset.x, minX, maxX);
        Vector3 targetPosition = new Vector3(targetPositionX, target.position.y + offset.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
        lastTargetPosition = target.position;
    }


    IEnumerator TransitionToTarget(float duration)
    {
        Vector3 startingPosition = transform.position;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            // Calculate the current progress as a percentage
            float progress = time / duration;

            Vector3 targetPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

            // Update the camera's position
            transform.position = Vector3.Lerp(startingPosition, targetPosition, progress);

            yield return null;
        }

        // Ensure the camera ends at the target position
        Vector3 finalTargetPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        transform.position = finalTargetPosition;

        // Transition is finished, enable the normal camera following
        isTransitioning = false;
    }
}



