using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This component is attached to the physical ship GameObjects.
/// It holds the current state (health, shield) and links the abilities.
/// </summary>
public class ShipUnit : MonoBehaviour
{
    [Header("Ship Stats")]
    public string ShipName = "New Ship";
    public bool IsPlayer = false;
    public int MaxHealth = 100;
    public int MaxShield = 50;

    [Header("Static Data")]
    public ShipDataSO shipData; // Assumes ShipDataSO exists

    [Header("Current State (Runtime)")]
    public int CurrentHealth;
    public int CurrentShield;

    [Header("Abilities (Set in Inspector)")]
    public List<AbilitySO> Abilities = new List<AbilitySO>();

    // REMOVED: healthUI reference and all related code
    
    // Optional VFX references (kept for future use)
    [Header("VFX References")]
    [Tooltip("Played when this ship is attacked.")]
    public ParticleSystem hitEffect;
    [Tooltip("Played when this ship is destroyed.")]
    public ParticleSystem explosionEffect;


    void Awake()
    {
        ResetState();
    }

    public void ResetState()
    {
        CurrentHealth = MaxHealth;
        CurrentShield = MaxShield;
        
        // REMOVED: UI update calls
    }

    /// <summary>
    /// Player or AI uses an ability against a target.
    /// </summary>
    public void UseAbility(AbilitySO ability, ShipUnit target)
    {
        ability.Execute(this, target);
        // Damage resolution happens inside the ability and calls TakeDamage/UpdateShield on the target.
    }

    /// <summary>
    /// Processes incoming damage from an ability.
    /// </summary>
    public void TakeDamage(int damage, string abilityName)
    {
        if (CurrentHealth <= 0) return;

        // 1. Play hit effect (for visual feedback)
        if (hitEffect != null)
        {
            hitEffect.Play();
        }

        // 2. Apply damage
        CurrentHealth -= damage;
        
        Debug.Log($"<color=red>{ShipName}</color> takes <color=red>{damage}</color> damage from {abilityName}!");

        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }
        
        // REMOVED: UI update calls
    }

    /// <summary>
    /// Updates the shield value (used by the DirectDamageAbilitySO).
    /// </summary>
    public void UpdateShield(int newShieldValue)
    {
        CurrentShield = newShieldValue;
        
        // REMOVED: UI update calls
    }

    /// <summary>
    /// Handles ship destruction.
    /// </summary>
    private void Die()
    {
        Debug.Log($"<color=red>!!! {ShipName} HAS BEEN DESTROYED !!!</color>");
        
        if (explosionEffect != null)
        {
            explosionEffect.transform.parent = null; 
            explosionEffect.Play();
        }
        
        gameObject.SetActive(false); 
    }
}