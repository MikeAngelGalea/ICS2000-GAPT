using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int maxHealth = 3; // Maximum health of the player
    private int currentHealth; // Current health of the player

    // Reference to the HealthBar component
    public HealthBar healthBar;

    // Reference to the Game Over canvas
    public GameObject gameOverCanvas;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacles"))
        {
            TakeDamage(1); // Reduce health by 1 when colliding with an obstacle
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0; // Ensure health doesn't go below 0
            GameOver(); // Call GameOver method
        }

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // Update the health value using a method from the HealthBar script
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogWarning("HealthBar component not found.");
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!"); // Placeholder for game over logic
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true); // Show the Game Over canvas
        }
        else
        {
            Debug.LogWarning("Game Over canvas not set in inspector.");
        }
    }
}
