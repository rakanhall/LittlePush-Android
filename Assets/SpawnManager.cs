using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> prefabsLowScore = new List<GameObject>();
    public List<GameObject> prefabsLowMidScore = new List<GameObject>();
    public List<GameObject> prefabsMidScore = new List<GameObject>();
    public List<GameObject> prefabsHighMidScore = new List<GameObject>();
    public List<GameObject> prefabsHigh1 = new List<GameObject>();
    public List<GameObject> prefabsHigh2 = new List<GameObject>();

    public List<GameObject> spawnedObjects = new List<GameObject>();

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newObj = Instantiate(prefab, position, rotation);
        spawnedObjects.Add(newObj);
        return newObj;
    }

    public void Despawn(GameObject obj)
    {
        spawnedObjects.Remove(obj);
        Destroy(obj);
    }
}



