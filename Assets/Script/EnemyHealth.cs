// The complete, upgraded EnemyHealth.cs
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;

    [Header("Effects")]
    public GameObject hitEffectPrefab;
    public float hitEffectLifetime = 2f; // Auto-destroy time for hit effect
    public GameObject deathEffectPrefab; 
    public float deathEffectLifetime = 3f; // Auto-destroy time for death effect
    public AudioClip hitSound;
    public AudioClip deathSound;

    private AudioSource audioSource;
    private EncounterManager encounterManager;

    void Start()
    {
        encounterManager = FindObjectOfType<EncounterManager>();
        // Add an AudioSource component if one doesn't exist
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (health <= 0) return;

        health -= damageAmount;

        // --- Screen shake check ---
        if (ScreenShake.Instance != null)
        {
            ScreenShake.Instance.TriggerShake(0.1f, 0.05f);
        }

        // Spawn hit effect + auto destroy
        if (hitEffectPrefab != null)
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, hitEffectLifetime);
        }

        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Notify encounter manager
        if (encounterManager != null)
        {
            encounterManager.OnEnemyDestroyed(this.gameObject);
        }

        // Spawn death effect + auto destroy
        if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffectLifetime);
        }

        // Play death sound
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // Finally, destroy enemy
        Destroy(gameObject);
    }
}
