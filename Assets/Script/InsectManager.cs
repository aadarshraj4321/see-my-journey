using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InsectManager : MonoBehaviour
{
    // A static "singleton" instance. This allows any insect to easily find the manager.
    public static InsectManager Instance;

    [Header("Insect Prefabs")]
    public GameObject[] insectPrefabs;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public int maxInsects = 20;
    public float respawnTime = 10f;

    // A list to keep track of all living insects.
    private List<GameObject> activeInsects = new List<GameObject>();

    void Awake()
    {
        // Set up the singleton instance.
        Instance = this;
    }

    void Start()
    {
        // Initial spawn: create the starting population of insects.
        for (int i = 0; i < maxInsects; i++)
        {
            SpawnInsect();
        }
    }

    void SpawnInsect()
    {
        if (insectPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject randomPrefab = insectPrefabs[Random.Range(0, insectPrefabs.Length)];

        GameObject newInsect = Instantiate(randomPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        
        // Add the new insect to our list.
        activeInsects.Add(newInsect);
    }

    // This is the public function that an insect will call when it dies.
    public void ReportDeath(GameObject deadInsect)
    {
        // Remove the dead insect from our list.
        activeInsects.Remove(deadInsect);
        
        // Start the timer to spawn a replacement.
        StartCoroutine(RespawnAnInsect());
    }

    private IEnumerator RespawnAnInsect()
    {
        yield return new WaitForSeconds(respawnTime);

        // Double-check if we are still under the max limit before spawning.
        if (activeInsects.Count < maxInsects)
        {
            SpawnInsect();
        }
    }
}