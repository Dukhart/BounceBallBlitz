using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawnObjects;
    public float spawnRange = 9;
    // Start is called before the first frame update
    void Start()
    {
        SpawnObject();
    }

    // spawns the object at index
    void SpawnObject(int index = 0)
    {
        // check we have something to spawn
        if (spawnObjects == null || spawnObjects.Length == 0 || index >= spawnObjects.Length) return;
        // get random position in range
        Vector3 spawnPos = new Vector3(Random.Range(0.0f, spawnRange), transform.position.y, Random.Range(0.0f, spawnRange));
        // spawn object
        Instantiate(spawnObjects[index], spawnPos, spawnObjects[index].transform.rotation);
    }
    // Spawn a random object from the list
    void SpawnRandom ()
    {
        if (spawnObjects == null) return;
        // spawn random object from the array
        SpawnObject(Random.Range(0, spawnObjects.Length));
    }
    // spawns all objects in the list
    void SpawnObjects ()
    {
        if (spawnObjects == null) return;
        // loop through the array
        for (int i = 0; i < spawnObjects.Length; ++i)
        {
            SpawnObject(i);
        }
    }
}
