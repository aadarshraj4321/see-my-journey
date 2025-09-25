
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10; // How much damage this bullet does

    // This function is automatically called by Unity when this object's collider hits another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we hit is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Find the PlayerHealth script on the player
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Deal damage to the player
                playerHealth.TakeDamage(damage);
            }
        }
        
        // Destroy the bullet after it hits anything (the player, a wall, the ground, etc.)
        Destroy(gameObject);
    }
}