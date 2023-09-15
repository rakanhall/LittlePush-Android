using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRestItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // After resetting timer, destroy this item
            Destroy(gameObject);
        }
    }
}
