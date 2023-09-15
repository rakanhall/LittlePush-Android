using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    public int captureWidth = 1920; // Set your desired width
    public int captureHeight = 1080; // Set your desired height

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Or any other key
        {
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            ScreenCapture.CaptureScreenshot("Screenshot" + timestamp + ".png", 1);
            Debug.Log("Screenshot taken!");
        }
    }
}

