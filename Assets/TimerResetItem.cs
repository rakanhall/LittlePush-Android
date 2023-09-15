using UnityEngine;

public class TimerResetItem : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        if (other.gameObject.CompareTag("Player"))
        {
            
            // Call the ResetTimer method in GameManager1
            GameManager1.instance.ResetTimer();  // Access Singleton instance directly

            // After resetting timer, destroy this item
            Destroy(gameObject);
        }
    }
}



