using UnityEngine;
using UnityEngine.UI;
using TMPro; // <-- ADD THIS LINE

/// <summary>
/// Manages the visual display of a ShipUnit's Health and Shield bars.
/// Attach this script to a UI element container.
/// </summary>
public class ShipHealthUI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The ShipUnit component this UI should track.")]
    public ShipUnit trackedShip;
    
    [Header("UI Elements (Drag Sliders/Text here)")]
    public Slider healthSlider;
    public Slider shieldSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;
    public TextMeshProUGUI shipNameText; // To display who's turn it is

    [Header("Turn Visuals")]
    public Color activeTurnColor = Color.yellow; // Highlight color for active turn
    public Color inactiveTurnColor = Color.white; // Default color for inactive turn
    public float activeScale = 1.05f; // Slight scale increase for emphasis

    private void Start()
    {
        if (trackedShip == null)
        {
            Debug.LogError("ShipHealthUI is missing a reference to the Tracked ShipUnit.");
            return;
        }

        // Initialize the display on Start
        shipNameText.text = trackedShip.ShipName;
        UpdateHealthDisplay(trackedShip.CurrentHealth, trackedShip.MaxHealth);
        UpdateShieldDisplay(trackedShip.CurrentShield, trackedShip.MaxShield);
        
        // Ensure UI is set to inactive state initially
        SetTurnStatus(false);
    }

    /// <summary>
    /// Visually highlights or un-highlights the UI to indicate the active turn.
    /// </summary>
    public void SetTurnStatus(bool isMyTurn)
    {
        if (shipNameText != null)
        {
            shipNameText.color = isMyTurn ? activeTurnColor : inactiveTurnColor;
        }

        // Apply a slight scale change to the entire UI block for emphasis
        transform.localScale = isMyTurn ? Vector3.one * activeScale : Vector3.one;

        // Optionally, you could disable/enable ability buttons here if you had them.
    }


    /// <summary>
    /// Updates the visual health bar and text. Called by ShipUnit.
    /// </summary>
    public void UpdateHealthDisplay(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}/{maxHealth}";
        }
    }

    /// <summary>
    /// Updates the visual shield bar and text. Called by ShipUnit.
    /// </summary>
    public void UpdateShieldDisplay(int currentShield, int maxShield)
    {
        if (shieldSlider != null)
        {
            shieldSlider.maxValue = maxShield;
            shieldSlider.value = currentShield;
        }
        
        if (shieldText != null)
        {
            shieldText.text = $"SH: {currentShield}/{maxShield}";
        }
    }
}
