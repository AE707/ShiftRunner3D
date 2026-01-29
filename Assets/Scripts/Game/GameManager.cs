using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameUIController uiController;
    public Transform playerTransform; // Assign in Inspector

    private bool isGameOver = false;
    private float distanceTraveled = 0f;
    private float bestDistance = 0f;
    private const string BEST_DISTANCE_KEY = "BestDistance";

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Load best distance from PlayerPrefs
        bestDistance = PlayerPrefs.GetFloat(BEST_DISTANCE_KEY, 0f);
        
        // Find player if not assigned
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
        
        // Update UI with initial values
        if (uiController != null)
        {
            uiController.UpdateBestDistance(bestDistance);
            uiController.UpdateDistance(0f);
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            // Check for restart input
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            return;
        }

        // Update distance traveled
        if (playerTransform != null)
        {
            distanceTraveled = playerTransform.position.z;
            
            // Update UI
            if (uiController != null)
            {
                uiController.UpdateDistance(distanceTraveled);
                uiController.UpdateSpeed(GetCurrentSpeed());
            }
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Time.timeScale = 0f;
        
        // Check if new best distance
        if (distanceTraveled > bestDistance)
        {
            bestDistance = distanceTraveled;
            PlayerPrefs.SetFloat(BEST_DISTANCE_KEY, bestDistance);
            PlayerPrefs.Save();
            Debug.Log($"New Best Distance: {bestDistance:F1}m");
        }
        
        // Show game over UI with stats
        if (uiController != null)
        {
            uiController.ShowGameOver(distanceTraveled, bestDistance);
        }
        
        Debug.Log("GAME OVER - Press R to Restart");
    }

    void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Public getters for other systems
    public float GetDistance()
    {
        return distanceTraveled;
    }

    public float GetBestDistance()
    {
        return bestDistance;
    }

    public float GetDifficulty()
    {
        // Calculate difficulty based on current player speed (0 = easy, 1 = max difficulty)
        PlayerMovement playerMovement = playerTransform?.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            return playerMovement.GetNormalizedSpeed();
        }
        return 0f;
    }

    public float GetCurrentSpeed()
    {
        PlayerMovement playerMovement = playerTransform?.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            return playerMovement.GetCurrentSpeed();
        }
        return 0f;
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
