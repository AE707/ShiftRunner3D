using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public Text finalDistanceText;    // "Final Distance: 123m"
    public Text finalBestText;        // "Best: 456m"
    public Button restartButton;      // Restart button (optional)
    public Button quitButton;         // Quit button (optional)

    [Header("HUD (In-Game)")]
    public Text distanceText;         // "Distance: 123m"
    public Text bestDistanceText;     // "Best: 456m"
    public Text speedText;            // "Speed: 8.5" (optional)

    void Start()
    {
        // Hide game over panel initially
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // Wire up button events if assigned
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitClicked);
    }

    // Called by GameManager each frame to update HUD
    public void UpdateDistance(float distance)
    {
        if (distanceText != null)
            distanceText.text = $"Distance: {distance:F0}m";
    }

    public void UpdateBestDistance(float best)
    {
        if (bestDistanceText != null)
            bestDistanceText.text = $"Best: {best:F0}m";
    }

    public void UpdateSpeed(float speed)
    {
        if (speedText != null)
            speedText.text = $"Speed: {speed:F1}";
    }

    // Called by GameManager on game over
    public void ShowGameOver(float finalDistance, float bestDistance)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Update game over panel texts
        if (finalDistanceText != null)
            finalDistanceText.text = $"Final Distance: {finalDistance:F0}m";
        if (finalBestText != null)
            finalBestText.text = $"Best: {bestDistance:F0}m";
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Button callbacks
    void OnRestartClicked()
    {
        if (GameManager.Instance != null)
        {
            // GameManager will handle restart via R-key
            // Or you can add a public Restart method to GameManager
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
            );
            Time.timeScale = 1f; // Reset time scale before reload
        }
    }

    void OnQuitClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
