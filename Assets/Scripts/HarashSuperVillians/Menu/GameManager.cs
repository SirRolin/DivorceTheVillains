using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject playerPrefab;
    private GameObject currentPlayer;
    private bool isGameOver = false;

    // Singleton pattern to ensure only one instance of the GameManager exists.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Spawns the player at the start of the game if the scene is GameScene(Rolins).
    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Rolins")
        {
            SpawnPlayer(); // Default spawn position
        }
    }


    //solely for testing purposes.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Instance.OnDeath();
        }
    }

    // Spawns the player at a specific location in the scene and subscribes to the OnDeath event.
    public void SpawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefab, new Vector3(19,2,-8), Quaternion.identity);
        currentPlayer.GetComponent<Assets.Scripts.HarashSuperVillains.Player.DeathManager>().OnDeath += OnDeath;
    }

     public GameObject GetCurrentPlayer()
    {
        return currentPlayer;
    }

    // Destroys the player and calls the HandleGameOver method.
    private void OnDeath()
    {
        Destroy(currentPlayer);
        HandleGameOver();
    }

    // Loads the GameOver scene when the player dies.
    private void HandleGameOver()
    {
        isGameOver = true;
        SceneManager.LoadScene("GameOver");
    }

    // Didnt need this code after all. Supposed to be used for restarting the game and going back and forth from scenes.
    /*public void RestartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        isGameOver = false;
        SceneManager.LoadScene("Rolins");
    }

    public void GoToMainMenu()
    {
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            isGameOver = false; // Reset game over state when returning to MainMenu
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    */
}
