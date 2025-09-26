// using UnityEngine;
// using UnityEngine.AI;

// [RequireComponent(typeof(NavMeshAgent))]
// [RequireComponent(typeof(EnemyHealth))]
// [RequireComponent(typeof(Collider))]
// public class InsectAI : MonoBehaviour
// {
//     [Header("Combat Settings")]
//     [Tooltip("How much damage the insect deals when it touches the player.")]
//     public int attackDamage = 5;
//     [Tooltip("How long to wait between each attack (in seconds) while touching the player.")]
//     public float attackCooldown = 1.0f;

//     // --- NEW: AUDIO SETTINGS ---
//     [Header("Audio")]
//     [Tooltip("The looping humming sound the insect makes while alive.")]
//     public AudioClip hummingSoundLoop;
//     [Tooltip("An optional sound to play when the insect first spawns.")]
//     public AudioClip spawnSound;
    
//     // --- Private Variables ---
//     private NavMeshAgent agent;
//     private Transform player;
//     private EnemyHealth enemyHealth;
//     private float lastAttackTime = 0f;

//     // A private variable to hold the insect's own AudioSource
//     private AudioSource audioSource;

//     void Start()
//     {
//         // --- SETUP THE AUDIO SOURCE ---
//         // Add an AudioSource component to this insect object
//         audioSource = gameObject.AddComponent<AudioSource>();
//         // Configure it for 3D sound so it gets quieter as it gets farther away
//         audioSource.spatialBlend = 1.0f; 
//         audioSource.minDistance = 3.0f;
//         audioSource.maxDistance = 40.0f;
//         audioSource.volume = 0.6f; // Insect sounds can be a bit louder

//         // --- PLAY THE SPAWN SOUND ---
//         // Play the one-shot spawn sound if it has been assigned
//         if (spawnSound != null)
//         {
//             audioSource.PlayOneShot(spawnSound);
//         }

//         // --- START THE LOOPING SOUND ---
//         // Set the clip for the looping hum, but don't play it immediately
//         if (hummingSoundLoop != null)
//         {
//             audioSource.clip = hummingSoundLoop;
//             audioSource.loop = true; // Tell the AudioSource to loop this clip
            
//             // Play the looping sound after a short delay (e.g., after the spawn sound finishes)
//             float delay = (spawnSound != null) ? spawnSound.length : 0.5f;
//             audioSource.PlayDelayed(delay);
//         }

//         // --- Get other components (same as before) ---
//         agent = GetComponent<NavMeshAgent>();
//         enemyHealth = GetComponent<EnemyHealth>();

//         GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
//         if (playerObject != null)
//         {
//             player = playerObject.transform;
//         }
//     }

//     void Update()
//     {
//         // If the insect is dead, we need to make sure the sound stops.
//         if (enemyHealth != null && enemyHealth.health <= 0)
//         {
//             // If the sound is still playing, stop it.
//             if (audioSource.isPlaying)
//             {
//                 audioSource.Stop();
//             }
//             // Stop the NavMeshAgent if it's still moving
//             if (agent.isStopped == false)
//             {
//                 agent.isStopped = true;
//             }
//             return; // Stop running the rest of the Update logic
//         }

//         // If there's no player, do nothing.
//         if (player == null) return;

//         // --- CHASE LOGIC (same as before) ---
//         agent.SetDestination(player.position);
//     }
    
//     // This function is called continuously while another collider is touching this one.
//     private void OnCollisionStay(Collision collision)
//     {
//         // We add a check here to make sure dead insects can't deal damage.
//         if (enemyHealth != null && enemyHealth.health <= 0) return;

//         if (collision.gameObject.CompareTag("Player"))
//         {
//             if (Time.time >= lastAttackTime + attackCooldown)
//             {
//                 PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
//                 if (playerHealth != null)
//                 {
//                     playerHealth.TakeDamage(attackDamage);
//                     lastAttackTime = Time.time;
//                 }
//             }
//         }
//     }
// }






using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(Collider))]
public class InsectAI : MonoBehaviour
{
    [Header("Combat Settings")]
    public int attackDamage = 5;
    public float attackCooldown = 1.0f;

    [Header("Audio")]
    public AudioClip hummingSoundLoop;
    public AudioClip spawnSound;
    
    // --- Private Variables ---
    private NavMeshAgent agent;
    private Transform player;
    private EnemyHealth enemyHealth;
    private float lastAttackTime = 0f;
    private AudioSource audioSource;

    void Start()
    {
        // --- Setup Audio ---
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f; 
        audioSource.minDistance = 3.0f;
        audioSource.maxDistance = 40.0f;
        audioSource.volume = 0.6f;

        if (spawnSound != null)
        {
            audioSource.PlayOneShot(spawnSound);
        }

        if (hummingSoundLoop != null)
        {
            audioSource.clip = hummingSoundLoop;
            audioSource.loop = true;
            float delay = (spawnSound != null) ? spawnSound.length : 0.5f;
            audioSource.PlayDelayed(delay);
        }
        
        // --- Setup Components ---
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        // If the insect is dead, stop all sounds and movement.
        if (enemyHealth != null && enemyHealth.health <= 0)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            if (agent.isStopped == false)
            {
                agent.isStopped = true;
            }
            return;
        }

        if (player == null) return;

        // --- Chase Logic ---
        agent.SetDestination(player.position);
    }
    
    // Handles the "touch" attack.
    private void OnCollisionStay(Collision collision)
    {
        if (enemyHealth != null && enemyHealth.health <= 0) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(attackDamage);
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}