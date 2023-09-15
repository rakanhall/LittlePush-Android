using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public GameObject backgroundPrefab;  // The Prefab for the backgrounds
    public float spawnDistance = -10f;  // The distance below the player at which to spawn new backgrounds
    

    private Transform playerTransform;
    private float backgroundHeight;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        backgroundHeight = 9.7f;  // Replace this with your background's height
    }

    private void Update()
    {
        // Spawn new backgrounds
        if (playerTransform.position.y - backgroundHeight < spawnDistance)
        {
            Instantiate(backgroundPrefab, new Vector2(0, playerTransform.position.y + spawnDistance), Quaternion.identity);
            spawnDistance -= backgroundHeight;
        }
    }


}












