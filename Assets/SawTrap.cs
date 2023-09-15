using UnityEngine;

public class SawTrap : MonoBehaviour
{
    public Transform[] path;
    public float speed = 5.0f;

    private int currentWaypointIndex = 0;
    private int direction = 1;

    void Start()
    {
        // Play the sound effect (assuming it's the first sound in your array)
        SoundManager.instance.PlaySound(0);
    }

    void Update()
    {
        // Get the current waypoint.
        Transform currentWaypoint = path[currentWaypointIndex];

        // Move the saw towards the current waypoint.
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, speed * Time.deltaTime);

        // If the saw has reached the current waypoint...
        if (transform.position == currentWaypoint.position)
        {
            // If we've reached the last waypoint, reverse direction.
            // Or if we've reached the first waypoint, set direction to forward.
            if (currentWaypointIndex == path.Length - 1)
            {
                direction = -1;
            }
            else if (currentWaypointIndex == 0)
            {
                direction = 1;
            }

            // Move to the next waypoint in the current direction.
            currentWaypointIndex += direction;

            

        }
    }
}


