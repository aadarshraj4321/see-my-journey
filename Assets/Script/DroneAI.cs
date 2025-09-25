// using UnityEngine;

// public class DroneAI : MonoBehaviour
// {
//     [Header("Movement")]
//     [Tooltip("How fast the drone moves towards the player.")]
//     public float speed = 5.0f;
//     [Tooltip("How high above the player the drone tries to stay.")]
//     public float hoverHeight = 3.0f;
//     [Tooltip("How close the drone will get to the player before stopping.")]
//     public float stoppingDistance = 4.0f;

//     [Header("Combat")]
//     [Tooltip("How much damage the drone deals per attack.")]
//     public int attackDamage = 5;
//     [Tooltip("How often the drone can attack (in seconds).")]
//     public float attackCooldown = 2.0f;
//     [Tooltip("A reference to the drone's weapon muzzle or firing point.")]
//     public Transform muzzlePoint;
//     [Tooltip("The particle effect to play when the drone fires.")]
//     public GameObject muzzleFlashPrefab;

//     // --- Private Variables ---
//     private Transform player;
//     private PlayerHealth playerHealth;
//     private float lastAttackTime = 0f;
//     private EnemyHealth enemyHealth; // Reference to its own health

//     void Start()
//     {
//         // Find the player using their tag.
//         GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
//         if (playerObject != null)
//         {
//             player = playerObject.transform;
//             playerHealth = playerObject.GetComponent<PlayerHealth>();
//         }

//         // Get a reference to its own health script to check if it's dead
//         enemyHealth = GetComponent<EnemyHealth>();
//     }

//     void Update()
//     {
//         // If there's no player or the drone is dead, do nothing.
//         if (player == null || (enemyHealth != null && enemyHealth.health <= 0))
//         {
//             return;
//         }

//         // --- MOVEMENT LOGIC ---

//         // 1. Determine the target position: above the player's head.
//         Vector3 targetPosition = player.position + Vector3.up * hoverHeight;

//         // 2. Calculate the distance to the target position.
//         float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

//         // 3. Always look at the player.
//         transform.LookAt(player);

//         // 4. Move towards the target only if we are outside the stopping distance.
//         if (distanceToTarget > stoppingDistance)
//         {
//             transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
//         }

//         // --- ATTACK LOGIC ---

//         // Check if enough time has passed since the last attack AND the player is within range.
//         // We use the stopping distance as our attack range.
//         if (Time.time >= lastAttackTime + attackCooldown && distanceToTarget <= stoppingDistance)
//         {
//             Attack();
//             lastAttackTime = Time.time; // Reset the attack timer
//         }
//     }

//     void Attack()
//     {
//         // Make sure we have a reference to the player's health before trying to damage it.
//         if (playerHealth != null)
//         {
//             Debug.Log(gameObject.name + " is attacking the player!");

//             // Play the muzzle flash effect at the firing point
//             if (muzzleFlashPrefab != null && muzzlePoint != null)
//             {
//                 Instantiate(muzzleFlashPrefab, muzzlePoint.position, muzzlePoint.rotation);
//             }

//             // Deal damage to the player
//             playerHealth.TakeDamage(attackDamage);
//         }
//     }
// }




// using UnityEngine;

// public class DroneAI : MonoBehaviour
// {
//     [Header("Movement")]
//     [Tooltip("How fast the drone moves towards the player.")]
//     public float speed = 5.0f;
//     [Tooltip("How high above the player the drone tries to stay.")]
//     public float hoverHeight = 3.0f;
//     [Tooltip("How close the drone needs to be to attack.")]
//     public float attackRange = 2.0f; // Renamed from stoppingDistance for clarity

//     [Header("Combat")]
//     [Tooltip("How much damage the drone deals per attack.")]
//     public int attackDamage = 5;
//     [Tooltip("How often the drone can attack (in seconds).")]
//     public float attackCooldown = 2.0f;
//     [Tooltip("The PARTICLE PREFAB to spawn on the player when attacking.")]
//     // public GameObject impactEffectPrefab; // This is the new slot for your particle effect

//     // --- Private Variables ---
//     private Transform player;
//     private PlayerHealth playerHealth;
//     private float lastAttackTime = 0f;
//     private EnemyHealth enemyHealth;

//     void Start()
//     {
//         // Find the player and their health script using their tag.
//         GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
//         if (playerObject != null)
//         {
//             player = playerObject.transform;
//             playerHealth = playerObject.GetComponent<PlayerHealth>();
//         }

//         // Get a reference to its own health script to check if it's dead
//         enemyHealth = GetComponent<EnemyHealth>();
//     }

//     void Update()
//     {
//         // If there's no player or the drone is already dead, do nothing.
//         if (player == null || (enemyHealth != null && enemyHealth.health <= 0))
//         {
//             return;
//         }

//         // --- MOVEMENT LOGIC ---

//         // 1. Determine the target position: above the player's head.
//         Vector3 targetPosition = player.position + Vector3.up * hoverHeight;

//         // 2. Always look directly at the player for aiming.
//         transform.LookAt(player);

//         // 3. Move towards the target position.
//         transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);


//         // --- ATTACK LOGIC ---
        
//         // 1. Calculate the distance to the player.
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//         // 2. Check if enough time has passed since the last attack AND the player is within range.
//         if (Time.time >= lastAttackTime + attackCooldown && distanceToPlayer <= attackRange)
//         {
//             Attack();
//             lastAttackTime = Time.time; // Reset the attack timer
//         }
//     }

//     // --- THIS IS THE NEW, SIMPLIFIED ATTACK FUNCTION ---
//     void Attack()
//     {
//         // Make sure we have a reference to the player's health before trying to damage it.
//         if (playerHealth != null)
//         {
//             Debug.Log(gameObject.name + " is dealing damage to the player!");

//             // 1. Play the impact particle effect AT THE PLAYER'S POSITION
//             // if (impactEffectPrefab != null)
//             // {
//             //     // Create an instance of your particle prefab right where the player is.
//             //     // Quaternion.identity means "no rotation".
//             //     Instantiate(impactEffectPrefab, player.position, Quaternion.identity);
//             // }

//             // 2. Deal damage to the player
//             playerHealth.TakeDamage(attackDamage);
//         }
//     }
// }







using UnityEngine;
using System.Collections; // Required for Coroutines

public class DroneAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5.0f;
    public float hoverHeight = 3.0f;
    public float attackRange = 2.0f;

    [Header("Combat")]
    public int attackDamage = 5;
    public float attackCooldown = 2.0f;
    public GameObject impactEffectPrefab;

    // --- NEW: AUDIO SETTINGS ---
    [Header("Audio")]
    [Tooltip("The looping sound the drone makes while alive.")]
    public AudioClip droneHumLoop;
    [Tooltip("An optional sound to play when the drone first spawns.")]
    public AudioClip spawnSound;
    
    // --- Private Variables ---
    private Transform player;
    private PlayerHealth playerHealth;
    private float lastAttackTime = 0f;
    private EnemyHealth enemyHealth;
    
    // A private variable to hold the drone's own AudioSource
    private AudioSource audioSource;

    void Start()
    {
        // --- SETUP THE AUDIO SOURCE ---
        // Add an AudioSource component to this drone object
        audioSource = gameObject.AddComponent<AudioSource>();
        // Configure it for 3D sound so it gets quieter as it gets farther away
        audioSource.spatialBlend = 1.0f; 
        audioSource.minDistance = 5.0f;
        audioSource.maxDistance = 50.0f;
        audioSource.volume = 0.5f; // Drones shouldn't be too loud

        // --- PLAY THE SPAWN SOUND ---
        // Play the one-shot spawn sound if it exists
        if (spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        // --- START THE LOOPING SOUND ---
        // Start the looping hum sound after a short delay (e.g., after the spawn sound finishes)
        if (droneHumLoop != null)
        {
            // We use a coroutine to delay the start of the loop
            StartCoroutine(StartLoopingSound());
        }

        // --- Find player and own health (same as before) ---
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            playerHealth = playerObject.GetComponent<PlayerHealth>();
        }
        enemyHealth = GetComponent<EnemyHealth>();
    }
    
    // This coroutine waits for the spawn sound to finish before starting the loop
    private IEnumerator StartLoopingSound()
    {
        // Wait for the duration of the spawn sound, or just 1 second if there is no spawn sound
        float delay = (spawnSound != null) ? spawnSound.length : 1.0f;
        yield return new WaitForSeconds(delay);

        // Now, set the clip to the looping sound and play it
        audioSource.clip = droneHumLoop;
        audioSource.loop = true;
        audioSource.Play();
    }

    void Update()
    {
        // If the drone is dead, we need to make sure the sound stops.
        if (enemyHealth != null && enemyHealth.health <= 0)
        {
            // If the sound is still playing, stop it.
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            return; // Stop running the rest of the Update logic
        }

        // If there's no player, do nothing.
        if (player == null)
        {
            return;
        }

        // --- MOVEMENT LOGIC (same as before) ---
        Vector3 targetPosition = player.position + Vector3.up * hoverHeight;
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // --- ATTACK LOGIC (same as before) ---
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (Time.time >= lastAttackTime + attackCooldown && distanceToPlayer <= attackRange)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }
    
    void Attack()
    {
        if (playerHealth != null)
        {
            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, player.position, Quaternion.identity);
            }
            playerHealth.TakeDamage(attackDamage);
        }
    }
}