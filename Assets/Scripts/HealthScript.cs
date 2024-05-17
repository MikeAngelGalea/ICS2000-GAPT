using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int maxHealth = 3; // max health of  player
    private int currentHealth; // current health of player

    // healthbar component
    public HealthBar healthBar;

    // game over canvas
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
            TakeDamage(1); // reducing health by 1 when colliding with an obstacle (meteordoids)
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0; // ensuring health doesn't go below 0
            GameOver(); // calling gameover method
        }

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        // updating the health value using a method from the healthbar script
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
        Debug.Log("Game Over!"); // placeholder for game over logic
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true); // show the gameover canvas
        }
        else
        {
            Debug.LogWarning("Game Over canvas not set in inspector.");
        }
    }
}
