using UnityEngine;

public class RockController : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public float fallDistanceThreshold = 5.0f; // Distance the rock has to fall to play the sound

    private Vector3 originalPosition;
    private bool soundPlayed = false; // Flag to make sure sound is only played once

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (!soundPlayed && Vector3.Distance(originalPosition, transform.position) >= fallDistanceThreshold)
        {
            SoundManager.instance.PlaySound(1);
            soundPlayed = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the rock collided with the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            // Play the breaking animation
            animator.SetTrigger("Break");

            gameObject.tag = "Untagged";

            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

            GetComponent<Collider2D>().enabled = false;
        }
    }
}


