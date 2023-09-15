using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxFactor; // This should be a small value, adjust to achieve desired effect

    private float previousCameraPosition;

    private void Start()
    {
        previousCameraPosition = cameraTransform.position.y;
    }

    private void LateUpdate()
    {
        // Calculate the difference in camera's movement
        float deltaY = cameraTransform.position.y - previousCameraPosition;

        // The parallax effect is achieved by moving the object a fraction of the camera's movement
        transform.position += new Vector3(0, deltaY, 0) * parallaxFactor;

        previousCameraPosition = cameraTransform.position.y;
    }
}





