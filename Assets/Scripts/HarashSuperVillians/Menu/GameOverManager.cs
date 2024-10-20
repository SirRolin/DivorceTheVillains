using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Restarts the game
    /*public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    } */

    // Returns to the main menu
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}