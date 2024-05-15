using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject GameOverPanel;

    // Method to handle game over
    public void GameOver()
    {
        // Display the game over panel or perform any other game over logic
        GameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

