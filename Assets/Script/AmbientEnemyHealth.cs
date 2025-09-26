using UnityEngine;

// This script is ONLY for ambient enemies like insects that should NOT count towards
// the main story wave progression.
public class AmbientEnemyHealth : MonoBehaviour
{
    public int health = 3;

    [Header("Effects")]
    public GameObject hitEffectPrefab;
    public float hitEffectLifetime = 2f;
    public GameObject deathEffectPrefab; 
    public float deathEffectLifetime = 3f;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private AudioSource audioSource;
    // We have REMOVED the reference to the EncounterManager.

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void TakeDamage(int damageAmount)
    {
        if (health <= 0) return;

        health -= damageAmount;

        if (ScreenShake.Instance != null)
        {
            ScreenShake.Instance.TriggerShake(0.1f, 0.05f);
        }

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
        // --- THIS IS THE KEY DIFFERENCE ---
        // We have REMOVED the line that notifies the EncounterManager.
        // if (encounterManager != null) { encounterManager.OnEnemyDestroyed(this.gameObject); }

        // The rest of the function is the same.
        if (deathEffectPrefab != null)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffectLifetime);
        }

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
        
        Destroy(gameObject);
    }
}