using UnityEngine;
using System; // Required for Events (Action)

public class InsectHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int health = 2;

    [Header("Effects")]
    public GameObject deathEffectPrefab;
    public AudioClip deathSound;
    
    // This is a broadcast signal. The new InsectSpawner will listen for it.
    public event Action OnInsectDied;

    // This public function will be called by the Weapon script.
    public void TakeDamage(int damageAmount)
    {
        if (health <= 0) return;

        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // 1. Broadcast the death signal to the spawner.
        if (OnInsectDied != null)
        {
            OnInsectDied();
        }

        // 2. Play death effects.
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // 3. Destroy the insect.
        Destroy(gameObject);
    }
}