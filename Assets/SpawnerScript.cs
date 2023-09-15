using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public Transform playerTransform;
    public float spawnThreshold; // Set this to correspond to score increments for spawning prefabs
    public float prefabHeight;
    public float cooldown = 0f;
    public SpawnManager spawnManager;

    public PlayerController playerScript;

    private float offsetDistance; // Will be set based on camera size
    private float lastSpawnHeight;
    private int currentThreshold;
    private float lastSpawnTime;
    private GameManager1 gameManager;

    private void Start()
    {
        lastSpawnHeight = transform.position.y;
        currentThreshold = Mathf.FloorToInt(playerTransform.position.y / spawnThreshold);
        lastSpawnTime = Time.time;
        gameManager = GameManager1.instance;
        offsetDistance = Camera.main.orthographicSize + prefabHeight; // Set offset to be above camera view
    }

    private void Update()
    {
        if (playerScript.IsDead)
        {
            return;
        }

        transform.position = new Vector3(transform.position.x, playerTransform.position.y - offsetDistance, transform.position.z);

        int currentScore = gameManager.GetCurrentScore();
        int scoreThreshold = Mathf.FloorToInt(currentScore / spawnThreshold);
        if (scoreThreshold > currentThreshold && Time.time > lastSpawnTime + cooldown)
        {
            currentThreshold = scoreThreshold;
            SpawnPrefab();
            lastSpawnTime = Time.time;
        }
    }


    private void SpawnPrefab()
    {
        List<GameObject> prefabListToChooseFrom;
        int currentScore = gameManager.GetCurrentScore();

        if (currentScore < 30)
        {
            prefabListToChooseFrom = spawnManager.prefabsLowScore;
        }
        else if (currentScore < 100)
        {
            prefabListToChooseFrom = spawnManager.prefabsLowMidScore;
            
        }
        else if (currentScore < 200)
        {
            prefabListToChooseFrom = spawnManager.prefabsMidScore;
            
        }
        else if (currentScore < 300)
        {
            prefabListToChooseFrom = spawnManager.prefabsHighMidScore;
            
        }
        else if (currentScore < 400)
        {
            prefabListToChooseFrom = spawnManager.prefabsHigh1;
            
        }
        else
        {
            prefabListToChooseFrom = spawnManager.prefabsHigh2;
            
        }

        GameObject prefabToSpawn = prefabListToChooseFrom[Random.Range(0, prefabListToChooseFrom.Count)];
        GameObject newPrefab = spawnManager.Spawn(prefabToSpawn, new Vector3(transform.position.x, lastSpawnHeight - prefabHeight, transform.position.z), Quaternion.identity);
        lastSpawnHeight = newPrefab.transform.position.y;
    }
}


















