using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(InsectHealth_v2))]
[RequireComponent(typeof(NavMeshAgent))]
public class InsectAI_v2 : MonoBehaviour
{
    [Header("Behavior")]
    public float activationRange = 20f;
    
    [Header("Combat")]
    public int attackDamage = 5;
    public float attackCooldown = 1.0f;
    
    // --- Private Instance Variables ---
    private NavMeshAgent agent;
    private Transform player;
    private bool isSleeping = true;
    private float lastAttackTime = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;

        // Start asleep
        agent.isStopped = true;
    }

    // OnDestroy is called by Unity automatically when this GameObject is destroyed.
    void OnDestroy()
    {
        // When this insect is destroyed, it tells the manager.
        // We check if the Instance exists in case we are quitting the application.
        if (InsectManager.Instance != null)
        {
            InsectManager.Instance.ReportDeath(this.gameObject);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= activationRange)
        {
            if (isSleeping)
            {
                isSleeping = false;
                agent.isStopped = false;
            }
            agent.SetDestination(player.position);
        }
        else
        {
            if (!isSleeping)
            {
                isSleeping = true;
                agent.isStopped = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isSleeping || Time.time < lastAttackTime + attackCooldown) return;

        if (collision.gameObject.CompareTag("Player"))
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