using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InsectSpawner : MonoBehaviour
{
    [Header("Insect Prefabs")]
    public GameObject[] insectPrefabs; // Your two insect types

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public int maxInsects = 10;
    public float spawnInterval = 5.0f;

    private int currentInsectCount = 0;

    void Start()
    {
        if (insectPrefabs.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogError("Insect Spawner is not configured!", this);
            this.enabled = false;
            return;
        }
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (currentInsectCount < maxInsects)
            {
                SpawnInsect();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnInsect()
    {
        GameObject randomInsectPrefab = insectPrefabs[Random.Range(0, insectPrefabs.Length)];
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newInsect = Instantiate(randomInsectPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        
        // Get the NEW InsectHealth script from the insect we just spawned.
        InsectHealth insectHealth = newInsect.GetComponent<InsectHealth>();
        if (insectHealth != null)
        {
            // Subscribe our "HandleInsectDeath" function to its death signal.
            insectHealth.OnInsectDied += HandleInsectDeath;
        }

        currentInsectCount++;
    }

    // This function is called by the death signal from InsectHealth.
    void HandleInsectDeath()
    {
        currentInsectCount--;
    }
}