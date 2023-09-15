using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScreenResolutionScaler : MonoBehaviour
{
    [SerializeField]
    private float targetAspect = 9f / 16f; // Target aspect ratio (width / height)

    [SerializeField]
    private float targetOrthographicSize = 5f; // The orthographic size at the target aspect ratio

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        float currentAspect = (float)Screen.width / Screen.height;
        _camera.orthographicSize = targetOrthographicSize * targetAspect / currentAspect;
    }
}

