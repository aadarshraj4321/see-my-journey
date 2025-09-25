using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    
    // The new private variable for storing health
    private int _currentHealth;
    // The new public property that other scripts can safely read from
    public int currentHealth { get { return _currentHealth; } }

    [Header("UI References")]
    public Slider healthBar;
    public GameObject retryPanel;

    private EncounterManager encounterManager;

    void Start()
    {
        _currentHealth = maxHealth;
        UpdateHealthBar();
        encounterManager = FindObjectOfType<EncounterManager>();
        if (retryPanel != null)
        {
            retryPanel.SetActive(false);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (_currentHealth <= 0) return;
        _currentHealth -= damageAmount;
        UpdateHealthBar();
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)_currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (retryPanel != null)
        {
            retryPanel.SetActive(true);
        }
        if (encounterManager != null)
        {
            encounterManager.OnPlayerDied();
        }
    }
    
    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        UpdateHealthBar();
        if (retryPanel != null)
        {
            retryPanel.SetActive(false);
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lethal"))
        {
            TakeDamage(9999);
        }
    }

    public void RestoreHealth(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
        UpdateHealthBar();
        Debug.Log("Player healed for " + amount + ". Current health: " + _currentHealth);
    }
}