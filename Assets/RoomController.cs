using UnityEngine;

public class RoomController : MonoBehaviour
{
    private WallColliderController wallController;

    private void Awake()
    {
        GameObject wallControllerObject = GameObject.FindGameObjectWithTag("WallController");
        if (wallControllerObject != null)
        {
            wallController = wallControllerObject.GetComponent<WallColliderController>();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            wallController.DisableWalls();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the other collider is the player
        if (other.CompareTag("Player"))
        {
            // Check if wallController is not null before trying to enable the walls
            if (wallController != null)
            {
                wallController.EnableWalls();
            }
            
        }
    }

}

