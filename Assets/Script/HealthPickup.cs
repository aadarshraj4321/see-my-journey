using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How much health this pickup restores.")]
    public int healthToRestore = 10;

    [Header("Effects (Optional)")]
    [Tooltip("A particle effect to play when the food is picked up.")]
    public GameObject pickupEffectPrefab;
    [Tooltip("A sound to play when the food is picked up.")]
    public AudioClip pickupSound;

    // This is a built-in Unity function that is called when another
    // collider enters this object's trigger.
    private void OnTriggerEnter(Collider other)
    {
        // First, check if the object that touched this food is the player.
        if (other.CompareTag("Player"))
        {
            // Find the PlayerHealth script on the player object.
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Check if the player's health script was found and if they are not already at max health.
            if (playerHealth != null && playerHealth.currentHealth < playerHealth.maxHealth)
            {
                // Call the public function on the player's health to heal them.
                playerHealth.RestoreHealth(healthToRestore);

                // --- PLAY EFFECTS ---
                if (pickupEffectPrefab != null)
                {
                    Instantiate(pickupEffectPrefab, transform.position, Quaternion.identity);
                }
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                // --- DESTROY THE FOOD ---
                // After healing the player, destroy this food GameObject.
                Destroy(gameObject);
            }
        }
    }
}