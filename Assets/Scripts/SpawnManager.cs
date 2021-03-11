using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyObjects;
    [SerializeField] GameObject[] powerUpObjects;
    [SerializeField] float spawnRange = 9;
    [SerializeField] int waveIncrement = 1;
    [SerializeField] int powerUpSpawnMin = 1;
    [SerializeField] int powerUpSpawnMax = 3;
    [SerializeField] TextMeshProUGUI waveNumberText;
    int waveNumber;
    bool isGameOver = false;
    List<GameObject> spawnedObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        waveNumber = 1;
        spawnedObjects.Clear();
        SpawnWave(waveNumber);
    }
    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
    }
    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
    }
    // spawns the object at index
    void SpawnObject(int index = 0)
    {
        // check we have something to spawn
        if (enemyObjects == null || enemyObjects.Length == 0 || index >= enemyObjects.Length) return;
        // get random position in range
        Vector3 spawnPos = new Vector3(Random.Range(0.0f, spawnRange), transform.position.y, Random.Range(0.0f, spawnRange));
        // spawn object
        GameObject spawned = Instantiate(enemyObjects[index], spawnPos, enemyObjects[index].transform.rotation);
        spawned.GetComponent<EnemyController>().spawnManager = this;
        spawnedObjects.Add(spawned);
    }
    void SpawnPowerUp(int index)
    {
        // check we have something to spawn
        if (powerUpObjects == null || powerUpObjects.Length == 0 || index >= powerUpObjects.Length) return;
        // get random position in range
        Vector3 spawnPos = new Vector3(Random.Range(0.0f, spawnRange), 0, Random.Range(0.0f, spawnRange));
        // spawn object
        GameObject spawned = Instantiate(powerUpObjects[index], spawnPos, powerUpObjects[index].transform.rotation);
    }
    // Spawn a random object from the list
    void SpawnRandom ()
    {
        if (enemyObjects == null) return;
        // spawn random object from the array
        SpawnObject(Random.Range(0, enemyObjects.Length));
    }
    // spawns all objects in the list
    void SpawnObjects ()
    {
        if (enemyObjects == null) return;
        // loop through the array
        for (int i = 0; i < enemyObjects.Length; ++i)
        {
            SpawnObject(i);
        }
    }
    // spawns a wave equal to number in size of objects at index;
    void SpawnWave (int number, int index = 0)
    {
        if (isGameOver) return;
        waveNumberText.text = "Wave " + waveNumber;
        // loop through spawning
        for (int i = 0; i < number; ++i)
        {
            // spawn the object
            SpawnObject(index);
        }
        // spawn a random range of power ups
        int powerUps = Random.Range(powerUpSpawnMin, powerUpSpawnMax);
        for (int i = 0; i < powerUps; ++i)
        {
            // spawn a random pwer up
            SpawnPowerUp(Random.Range(0, powerUpObjects.Length));
        }
    }
    // removes a spawned object from the list
    public void  RemoveSpawned (GameObject spawned)
    {
        spawnedObjects.Remove(spawned);
        spawnedObjects.TrimExcess();
        // if the last object was removed
        if (spawnedObjects.Count <= 0)
        {
            //spawn the next wave
            ++waveNumber;
            SpawnWave(waveNumber * waveIncrement);
        }
    }
    void OnGameOver ()
    {
        StopAllCoroutines();
        isGameOver = true;
    }
}
