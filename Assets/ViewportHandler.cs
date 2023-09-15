using UnityEngine;

public class ViewportHandler : MonoBehaviour
{
    private Camera mainCamera;

    public float desiredWorldWidth = 10f;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        ScaleCameraToWidth();
    }

    private void ScaleCameraToWidth()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        mainCamera.orthographicSize = desiredWorldWidth / (2f * aspectRatio);
    }
}


