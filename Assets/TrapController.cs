using UnityEngine;

public class TrapController : MonoBehaviour
{
    public Rigidbody2D rockRigidbody2D;
    public Swing swing; // Reference to the Swing script on the parent object

    private void Start()
    {
        swing = GetComponentInParent<Swing>(); // Get the Swing script from the parent object
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rockRigidbody2D.gravityScale = 2;

            rockRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

            if (swing != null)
            {
                swing.StopSwinging();
            }
        }
    }



   
}


