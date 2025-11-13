using UnityEngine;

public class PlayHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    void Awake()
    {
        // Initialize current health
        currentHealth = maxHealth;
    }

    // Call this function to damage the player
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // prevent going below 0

        Debug.Log("Current Health: " + currentHealth); // <-- Check console

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has died!");

        if(uiManager.Instance != null)
        {
            uiManager.Instance.ShowDeathScreen();
        }
        // You can add death animations, destroy object, reload scene, etc.
    }
    
}
