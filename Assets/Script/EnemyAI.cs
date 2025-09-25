using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Combat Settings")]
    public float attackRange = 2.0f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    private NavMeshAgent agent;
    private float lastAttackTime = 0f;
    private Animator animator; // Optional: If your enemy has animations

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>(); // Gets the animator, if one exists
    }

    void Update()
    {
        // If there is no target set by the manager, do nothing.
        if (EncounterManager.currentTarget == null) return;
        
        // --- CHASE LOGIC ---
        // The enemy's only job is to chase whatever the EncounterManager's current target is.
        agent.SetDestination(EncounterManager.currentTarget.position);
        
        // Optional: Update walking animation based on speed
        if (animator != null)
        {
            animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
        }

        // --- ATTACK LOGIC ---
        // Calculate the distance to the current target
        float distanceToTarget = Vector3.Distance(transform.position, EncounterManager.currentTarget.position);

        // Check if the target is within attack range AND enough time has passed
        if (distanceToTarget <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time; // Reset the attack timer
        }
    }

    void Attack()
    {
        // Before attacking, we must check if the target is the ACTUAL player.
        // We do not want to "attack" the fake target.
        if (EncounterManager.currentTarget.CompareTag("Player"))
        {
            // Optional: Trigger attack animation
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }
            
            // Get the PlayerHealth component from the target and deal damage
            PlayerHealth playerHealth = EncounterManager.currentTarget.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }
}