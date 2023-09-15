using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed = 1.0f;  // Speed of cloud movement
    public float startX = 10.0f;  // The starting x position (offscreen right)
    public float endX = -10.0f;  // The ending x position (offscreen left)

    public Transform cameraTransform;  // The transform of your camera

    private void Update()
    {
        // Set the Y position to match the camera's Y position
        float newY = cameraTransform.position.y;

        // Move the object to the left, but maintain the new Y position
        Vector3 newPos = new Vector3(transform.position.x - speed * Time.deltaTime, newY, transform.position.z);
        transform.position = newPos;

        // If the object is off the left side of the screen
        if (transform.position.x <= endX)
        {
            // Loop it back to the right side of the screen, but keep the Y position matched with the camera
            transform.position = new Vector3(startX, newY, transform.position.z);
        }
    }
}






