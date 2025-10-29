using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the simultaneous-turn flow.
/// Expects a Player Unit (your existing player) and one or more AIShips.
/// </summary>
public class TurnManager : MonoBehaviour
{
    public Unit playerUnit;                 // assign your player Unit in Inspector
    public AIShip[] aiShips;                // assign enemy AI ships in Inspector
    public Text turnText;                   // optional UI text to show turn number
    public Button endTurnButton;            // optional: user can press End Turn

    private int turnNumber = 1;
    private bool waitingForPlayer = true;

    // Player intent storage
    public Tile playerTargetTile = null;
    public PlayerIntent playerIntent = new PlayerIntent();

    void Start()
    {
        if (endTurnButton != null)
            endTurnButton.onClick.AddListener(OnEndTurnButton);
        UpdateTurnText();
    }

    void UpdateTurnText()
    {
        if (turnText != null) turnText.text = $"Turn {turnNumber}";
    }

    void OnEndTurnButton()
    {
        // If player hasn't chosen anything, treat as 'idle' turn
        if (playerIntent.action == PlayerAction.None && playerTargetTile == null)
        {
            playerIntent.action = PlayerAction.Idle;
        }
        TryResolveTurn();
    }

    /// <summary>
    /// Called externally when the player clicks a tile to move (hook this from your tile click handler).
    /// </summary>
    public void OnPlayerClickedTile(Tile clicked)
    {
        playerTargetTile = clicked;
        playerIntent.action = PlayerAction.Move;
        TryResolveTurn();
    }

    /// <summary>
    /// Called externally when the player picks an ability via UI.
    /// </summary>
    public void OnPlayerPickedAbility(PlayerAction action)
    {
        playerIntent.action = action;
        // For ability actions we don't set playerTargetTile (unless attack requires a tile)
        TryResolveTurn();
    }

    void TryResolveTurn()
    {
        // Only resolve once we've got a player choice (move or ability).
        // If you want the player to always explicitly press End Turn, require that instead.
        if (playerIntent.action == PlayerAction.None)
            return;

        StartCoroutine(ResolveTurnCoroutine());
    }

    IEnumerator ResolveTurnCoroutine()
    {
        waitingForPlayer = false;

        // 1) Ask each AI to pick their intent
        foreach (var ai in aiShips)
        {
            ai.ChooseIntent(playerUnit);
        }

        // short delay so you can see choices chosen (optional)
        yield return new WaitForSeconds(0.15f);

        // 2) Execute moves simultaneously:
        // Store previous positions for potential resolution logic (collisions, swaps).
        Vector2Int playerOld = playerUnit.gridPosition;
        Vector2Int playerNew = playerOld;
        if (playerIntent.action == PlayerAction.Move && playerTargetTile != null)
        {
            playerNew = playerTargetTile.GridPos;
        }

        Vector2Int[] aiOld = new Vector2Int[aiShips.Length];
        Vector2Int[] aiNew = new Vector2Int[aiShips.Length];
        for (int i = 0; i < aiShips.Length; i++)
        {
            aiOld[i] = aiShips[i].unitRef.gridPosition;
            aiNew[i] = aiOld[i];
            if (aiShips[i].intent.action == AIAction.Move && aiShips[i].intent.targetTile != null)
                aiNew[i] = aiShips[i].intent.targetTile.GridPos;
        }

        // 3) Commit movement (simultaneous)
        // Simple policy: allow movement into any empty tile; if two units swap tiles, allow.
        // Implement more complex conflict resolution here if needed.
        playerUnit.MoveToGridPosition(playerNew);
        for (int i = 0; i < aiShips.Length; i++)
            aiShips[i].unitRef.MoveToGridPosition(aiNew[i]);

        // short delay for movement animation (if you have one)
        yield return new WaitForSeconds(0.25f);

        // 4) Resolve abilities (again simultaneously):
        // Player ability
        ResolvePlayerAbility();

        // AI abilities
        foreach (var ai in aiShips)
            ResolveAIAbility(ai);

        // 5) End of turn cleanup
        playerIntent = new PlayerIntent(); // reset
        playerTargetTile = null;
        foreach (var ai in aiShips) ai.intent = new AIIntent(); // reset AI intents

        turnNumber++;
        UpdateTurnText();
        waitingForPlayer = true;
    }

    void ResolvePlayerAbility()
    {
        if (playerIntent.action == PlayerAction.Attack)
        {
            // attack any AI in range (adjacent)
            foreach (var ai in aiShips)
            {
                if (Vector2Int.Distance(playerUnit.gridPosition, ai.unitRef.gridPosition) <= 1.5f)
                {
                    ai.unitRef.TakeDamage(playerUnit.attackPower);
                }
            }
        }
        else if (playerIntent.action == PlayerAction.Freeze)
        {
            // freeze nearest AI in range (or all in range)
            foreach (var ai in aiShips)
            {
                if (Vector2Int.Distance(playerUnit.gridPosition, ai.unitRef.gridPosition) <= 2f)
                {
                    ai.unitRef.ApplyFreeze(1); // freeze for 1 turn
                }
            }
        }
        else if (playerIntent.action == PlayerAction.Repair)
        {
            playerUnit.Repair(playerUnit.repairAmount);
        }
    }

    void ResolveAIAbility(AIShip ai)
    {
        var intent = ai.intent;
        if (intent.action == AIAction.Attack)
        {
            if (Vector2Int.Distance(ai.unitRef.gridPosition, playerUnit.gridPosition) <= 1.5f)
            {
                playerUnit.TakeDamage(ai.unitRef.attackPower);
            }
        }
        else if (intent.action == AIAction.Freeze)
        {
            if (Vector2Int.Distance(ai.unitRef.gridPosition, playerUnit.gridPosition) <= 2f)
                playerUnit.ApplyFreeze(1);
        }
        else if (intent.action == AIAction.Repair)
        {
            ai.unitRef.Repair(ai.unitRef.repairAmount);
        }
    }
}

/// <summary>
/// Simple containers for intents
/// </summary>
public enum PlayerAction { None, Idle, Move, Attack, Freeze, Repair }
public enum AIAction { None, Idle, Move, Attack, Freeze, Repair }

public class PlayerIntent
{
    public PlayerAction action = PlayerAction.None;
    // additional fields if needed (target tile, target unit)
}

public class AIIntent
{
    public AIAction action = AIAction.None;
    public Tile targetTile = null;
}

/// <summary>
/// Small helper to access Unit grid pos (your Unit class must expose an int Vector2Int gridPosition).
/// </summary>
public partial class Unit
{
    // This partial is provided only as a guide. If your Unit already has these members,
    // remove/reconcile these declarations.
    public Vector2Int gridPosition;
    public int health;
    public int attackPower;
    public int repairAmount;

    public void MoveToGridPosition(Vector2Int newPos) { /* your existing MoveTo implementation */ }
    public void TakeDamage(int dmg) { /* your implementation */ }
    public void Repair(int amt) { /* your implementation */ }
    public void ApplyFreeze(int turns) { /* implement a freeze/status system on Unit */ }
}
