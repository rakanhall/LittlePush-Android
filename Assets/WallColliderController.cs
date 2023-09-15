using UnityEngine;

public class WallColliderController : MonoBehaviour
{
    public BoxCollider2D leftWallCollider;
    public BoxCollider2D rightWallCollider;
    public Transform player;
    public float activateHeight = -9f;
    public static WallColliderController instance;

    private bool shouldMove;
    private float adjustment = 10f; // Adjust this value as needed
    private float highestWallYPosition; // Store the highest Y position the walls have reached

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // Initialize the highest wall Y position
        highestWallYPosition = leftWallCollider.transform.position.y;
    }

    private void Update()
    {
        if (!shouldMove && player.position.y <= activateHeight)
        {
            shouldMove = true;
        }

        if (shouldMove)
        {
            // Compute the new wall Y position, keeping it above the player
            float newWallYPosition = player.position.y + adjustment;

            // Ensure the walls do not move upwards
            if (newWallYPosition < highestWallYPosition)
            {
                // Position the wall colliders
                leftWallCollider.transform.position = new Vector2(leftWallCollider.transform.position.x, newWallYPosition);
                rightWallCollider.transform.position = new Vector2(rightWallCollider.transform.position.x, newWallYPosition);

                // Update the highest wall Y position
                highestWallYPosition = newWallYPosition;
            }
        }
    }

    public void DisableWalls()
    {
        leftWallCollider.enabled = false;
        rightWallCollider.enabled = false;
    }

    public void EnableWalls()
    {
        if (leftWallCollider != null && rightWallCollider != null)
        {
            leftWallCollider.enabled = true;
            rightWallCollider.enabled = true;
        }
    }
}








