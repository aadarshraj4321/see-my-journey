using UnityEngine;

public class InsectHealth_v2 : MonoBehaviour
{
    [Header("Health Settings")]
    public int health = 2;

    [Header("Effects")]
    public GameObject deathEffectPrefab;
    public AudioClip deathSound;

    // This public function will be called by the Weapon script.
    public void TakeDamage(int damageAmount)
    {
        if (health <= 0) return;

        health -= damageAmount;

        // You can add hit effects or screen shake here if you want
        // if (ScreenShake.Instance != null) { ScreenShake.Instance.TriggerShake(0.1f, 0.05f); }

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Play death effects.
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        // Destroy the insect. The AI script will handle telling the system to respawn.
        Destroy(gameObject);
    }
}