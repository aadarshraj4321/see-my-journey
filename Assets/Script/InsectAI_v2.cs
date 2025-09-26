using UnityEngine;
using UnityEngine.AI;

// This script requires the NEW InsectHealth script to be on the same object.
[RequireComponent(typeof(InsectHealth))]
[RequireComponent(typeof(NavMeshAgent))]
public class InsectAI_v2 : MonoBehaviour
{
    [Header("Combat Settings")]
    public int attackDamage = 5;
    public float attackCooldown = 1.0f;

    [Header("Audio")]
    public AudioClip hummingSoundLoop;
    
    // Private variables
    private NavMeshAgent agent;
    private Transform player;
    private InsectHealth insectHealth; // Reference to the NEW health script
    private AudioSource audioSource;
    private float lastAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        insectHealth = GetComponent<InsectHealth>();

        // Setup Audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.volume = 0.6f;
        if (hummingSoundLoop != null)
        {
            audioSource.clip = hummingSoundLoop;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Find the player
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        // If the insect is dead, stop everything.
        if (insectHealth != null && insectHealth.health <= 0)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            if (agent.isStopped == false) agent.isStopped = true;
            return;
        }

        if (player == null) return;

        // Chase the player
        agent.SetDestination(player.position);
    }

    // Handle touch-based attacks
    private void OnCollisionStay(Collision collision)
    {
        if (insectHealth != null && insectHealth.health <= 0) return;

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