using UnityEngine;

public class BalloonController : MonoBehaviour
{
    public bool isPlayerAlive = true;
    public float flySpeed = 2f;

    void Update()
    {
        if (!isPlayerAlive)
        {
            FlyAway();
        }
    }

    void FlyAway()
    {
        transform.Translate(Vector2.up * flySpeed * Time.deltaTime);
    }

    public void ReleaseBalloon()
    {
        isPlayerAlive = false;
    }
}

