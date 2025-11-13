using UnityEngine;
using UnityEngine.SceneManagement;

public class uiManager : MonoBehaviour
{
    // --- Singleton Pattern for easy access ---
    public static uiManager Instance { get; private set; }

    [Header("UI Panels")]
    [Tooltip("The GameObject/Panel for the Main Game HUD.")]
    public GameObject gameHUD;

    [Tooltip("The GameObject/Panel for the Death Screen.")]
    public GameObject deathScreen;

    private void Awake()
    {
        // Implement Singleton
        if (Instance == null)
        {
            Instance = this;
            // Ensure the UI panels are set up correctly on start
            ShowGameHUD();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- Core State Management ---

    /// <summary>
    /// Displays the main game HUD and resumes time.
    /// </summary>
    public void ShowGameHUD()
    {
        if (gameHUD != null) gameHUD.SetActive(true);
        if (deathScreen != null) deathScreen.SetActive(false);

        // Resume game time
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Displays the Death Screen and pauses the game.
    /// </summary>
    public void ShowDeathScreen()
    {
        if (gameHUD != null) gameHUD.SetActive(false);
        if (deathScreen != null) deathScreen.SetActive(true);

        // Stop game time when the death screen appears
        Time.timeScale = 0f;
    }

    // --- Button Functionality for Death Screen ---

    /// <summary>
    /// Restarts the current scene (used by the Restart Button).
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Loads the Main Menu scene (used by the Main Menu Button).
    /// </summary>
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        // IMPORTANT: Replace "MainMenu" with the actual name of your Main Menu scene
        SceneManager.LoadScene("MainMenu"); 
    }
}