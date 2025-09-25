using UnityEngine;
using System.Collections; // Required for Coroutines

public class FoodSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [Tooltip("The food prefab that this spawner will create.")]
    public GameObject foodPrefab;
    [Tooltip("How many seconds to wait before respawning the food after it's picked up.")]
    public float respawnTime = 30.0f;

    // A private variable to keep track of the food this spawner has created.
    private GameObject currentFoodInstance;
    // A flag to prevent the spawner from starting multiple respawn timers.
    private bool isRespawning = false;

    void Start()
    {
        // Spawn the first food item as soon as the game starts.
        SpawnFood();
    }

    void Update()
    {
        // This check runs every frame.
        // If our food instance has been destroyed (picked up) AND we are not already in the process of respawning...
        if (currentFoodInstance == null && !isRespawning)
        {
            // ...then start the respawn process.
            StartCoroutine(RespawnFood());
        }
    }

    // This is a coroutine that handles the waiting period.
    private IEnumerator RespawnFood()
    {
        // Set the flag to true so the Update method doesn't call this coroutine again.
        isRespawning = true;
        Debug.Log("Food picked up. Respawning in " + respawnTime + " seconds.");
        
        // Wait for the specified amount of time.
        yield return new WaitForSeconds(respawnTime);
        
        // After waiting, spawn a new food item.
        SpawnFood();
    }

    // This function creates a new instance of the food prefab.
    void SpawnFood()
    {
        Debug.Log("A new food item has spawned at " + transform.position);
        
        // Instantiate the food at this spawner's position and rotation.
        currentFoodInstance = Instantiate(foodPrefab, transform.position, transform.rotation);
        
        // Reset the respawning flag so the Update method can detect when this new food is picked up.
        isRespawning = false;
    }
}