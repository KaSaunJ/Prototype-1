using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages the overall combat flow, turn sequence, and player input.
/// Attach this to an empty GameObject in the scene.
/// </summary>
public class CombatManager : MonoBehaviour
{
    [Header("Ship References (Drag GameObjects Here)")]
    public ShipUnit playerShip;
    public ShipUnit enemyShip;

    [Header("Ability References (Auto-loaded from Player Ship)")]
    private AbilitySO ability1;
    private AbilitySO ability2;
    private AbilitySO ability3;
    private AbilitySO ability4; // NEW: Added support for a 4th ability

    private bool isPlayerTurn = false;

    void Start()
    {
        SetupCombat();
        StartTurn();
    }

    void SetupCombat()
    {
        if (playerShip == null || enemyShip == null)
        {
            Debug.LogError("Player or Enemy Ship references are missing in the Combat Manager!");
            return;
        }

        playerShip.ResetState();
        enemyShip.ResetState();

        // Load the first four abilities from the player ship's list.
        if (playerShip.Abilities.Count >= 1) ability1 = playerShip.Abilities[0];
        if (playerShip.Abilities.Count >= 2) ability2 = playerShip.Abilities[1];
        if (playerShip.Abilities.Count >= 3) ability3 = playerShip.Abilities[2];
        if (playerShip.Abilities.Count >= 4) ability4 = playerShip.Abilities[3]; // Check for 4th ability
        
        if (playerShip.Abilities.Count < 3)
        {
            Debug.LogWarning("Player Ship does not have at least 3 abilities assigned in the inspector!");
        }

        Debug.Log("--- Combat Initiated ---");
        Debug.Log($"<color=yellow>{playerShip.ShipName}</color> HP/SH: {playerShip.CurrentHealth}/{playerShip.CurrentShield}");
        Debug.Log($"<color=cyan>{enemyShip.ShipName}</color> HP/SH: {enemyShip.CurrentHealth}/{enemyShip.CurrentShield}");
    }

    void StartTurn()
    {
        if (playerShip.CurrentHealth <= 0 || enemyShip.CurrentHealth <= 0)
        {
            Debug.Log("Game Over. Press R to Restart.");
            isPlayerTurn = false;
            return;
        }

        isPlayerTurn = true;
        Debug.Log($"\n<color=yellow>--- PLAYER TURN ---</color>");
        
        // Show available abilities for player input
        string abilitiesPrompt = "Press 1 for " + (ability1 != null ? ability1.abilityName : "MISSING") +
                                 ", 2 for " + (ability2 != null ? ability2.abilityName : "MISSING") +
                                 ", 3 for " + (ability3 != null ? ability3.abilityName : "MISSING");
        
        if (ability4 != null)
        {
             abilitiesPrompt += ", 4 for " + ability4.abilityName;
        }
        
        Debug.Log(abilitiesPrompt);
    }

    void EndTurn()
    {
        isPlayerTurn = false;
        // Check for end of game conditions again before starting AI turn
        if (playerShip.CurrentHealth > 0 && enemyShip.CurrentHealth > 0)
        {
            Invoke("AITurn", 1.5f); // 1.5 second delay for AI turn
        }
    }

    void AITurn()
    {
        Debug.Log($"\n<color=cyan>--- AI TURN ---</color>");

        // --- AI Logic: Randomly select an available ability ---
        if (enemyShip.Abilities.Count > 0)
        {
            int attackIndex = Random.Range(0, enemyShip.Abilities.Count);
            AbilitySO aiAbility = enemyShip.Abilities[attackIndex];
            
            enemyShip.UseAbility(aiAbility, playerShip);
        }
        else
        {
            Debug.LogWarning("Enemy ship does not have any abilities assigned!");
        }

        // Display current HP/Shields after the AI attack
        Debug.Log($"<color=yellow>{playerShip.ShipName}</color> HP/SH: {playerShip.CurrentHealth}/{playerShip.CurrentShield}");
        Debug.Log($"<color=cyan>{enemyShip.ShipName}</color> HP/SH: {enemyShip.CurrentHealth}/{enemyShip.CurrentShield}");

        // Check for end of game conditions after AI attack
        if (playerShip.CurrentHealth > 0 && enemyShip.CurrentHealth > 0)
        {
            StartTurn();
        }
    }

    void Update()
    {
        if (isPlayerTurn)
        {
            HandlePlayerInput();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SetupCombat();
            StartTurn();
        }
    }

    void HandlePlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && ability1 != null)
        {
            playerShip.UseAbility(ability1, enemyShip);
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && ability2 != null)
        {
            playerShip.UseAbility(ability2, enemyShip);
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && ability3 != null)
        {
            playerShip.UseAbility(ability3, enemyShip); 
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && ability4 != null) // NEW: Input for 4th ability
        {
            playerShip.UseAbility(ability4, enemyShip); 
            EndTurn();
        }
        
        if (!isPlayerTurn && playerShip.CurrentHealth > 0 && enemyShip.CurrentHealth > 0)
        {
            // After player attacks, display current HP/Shields
            Debug.Log($"<color=yellow>{playerShip.ShipName}</color> HP/SH: {playerShip.CurrentHealth}/{playerShip.CurrentShield}");
            Debug.Log($"<color=cyan>{enemyShip.ShipName}</color> HP/SH: {enemyShip.CurrentHealth}/{enemyShip.CurrentShield}");
        }
    }
}