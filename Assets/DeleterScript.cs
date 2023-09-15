using UnityEngine;
using System.Collections.Generic;

public class DeleterScript : MonoBehaviour
{
    public Transform playerTransform;
    public float offsetDistance;
    public float deleteThreshold;

    public PlayerController playerScript;
    public SpawnManager spawnManager;  // Reference to the SpawnManager script

    private void Update()
    {
        // Check if the player is dead
        if (playerScript.IsDead)
        {
            return; // If the player is dead, don't execute the rest of the Update function
        }

        // Rest of the code
        transform.position = new Vector3(transform.position.x, playerTransform.position.y - offsetDistance, transform.position.z);

        // Access all the spawned objects directly from the SpawnManager
        List<GameObject> objectsToDelete = new List<GameObject>();

        foreach (GameObject spawnedObject in spawnManager.spawnedObjects)
        {
            if (spawnedObject.transform.position.y > transform.position.y + deleteThreshold)
            {
                objectsToDelete.Add(spawnedObject);
            }
        }

        foreach (GameObject objectToDelete in objectsToDelete)
        {
            spawnManager.Despawn(objectToDelete);
        }
    }
}









