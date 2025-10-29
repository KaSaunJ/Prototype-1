using UnityEngine;

// Assumes this file inherits from AbilitySO or a similar base class that defines Execute()
public class DirectDamageAbilitySO : AbilitySO
{
    [Header("Damage Properties")]
    // FIX CS0108: Use 'new' keyword to explicitly hide the inherited baseDamage property from AbilitySO.
    [Tooltip("Base damage amount of the ability.")]
    public new int baseDamage = 20;
    
    [Tooltip("Multiplier applied to the source's Attack stat (e.g., 1.5 for 150% damage).")]
    public float damageMultiplier = 1.0f;

    // --- FIX CS0103: The OnEnable block is removed because 'requiresTarget' 
    // must be defined in the base class (AbilitySO) as 'protected' or 'public'.
    // We assume the logic is handled in AbilitySO's constructor or OnEnable. ---
    /* private void OnEnable()
    {
        // Default settings for an attack
        // requiresTarget = true; 
    }
    */

    /// <summary>
    /// Executes the direct damage ability on the primary target.
    /// </summary>
    public override void Execute(ShipUnit source, ShipUnit primaryTarget)
    {
        // The CombatManager is responsible for ensuring primaryTarget is not null before calling Execute.
        if (primaryTarget == null || primaryTarget.CurrentHealth <= 0)
        {
            Debug.LogWarning($"{source.ShipName}'s {abilityName} failed: No valid target.");
            return;
        }

        // --- 1. Calculate Base Damage ---
        // FIX CS1061: This line assumes you have now added 'public ShipDataSO shipData;' 
        // to your ShipUnit.cs component.
        int rawDamage = baseDamage + Mathf.RoundToInt(source.shipData.baseAttack * damageMultiplier);
        
        Debug.Log($"<color=green>{source.ShipName}</color> fires {abilityName} at <color=red>{primaryTarget.ShipName}</color> for {rawDamage} base damage.");

        // --- 2. Apply Damage to Shields First ---
        int currentShield = primaryTarget.CurrentShield;
        int healthDamage = 0; // The damage that overflows to health
        int shieldDamage;   // The damage applied to shields

        if (currentShield > 0)
        {
            if (rawDamage > currentShield)
            {
                // Damage overflows: shields are completely depleted
                shieldDamage = currentShield;
                healthDamage = rawDamage - currentShield;
                
                Debug.Log($"Shields broken! {shieldDamage} damage absorbed. {healthDamage} overflow damage remaining.");
                
                // Update shield value to 0
                primaryTarget.UpdateShield(0); 
            }
            else
            {
                // Shields absorb all damage
                shieldDamage = rawDamage; 
                int newShieldValue = currentShield - rawDamage;
                
                Debug.Log($"Shields absorb all damage. New shield value: {newShieldValue}.");
                
                // Update shield value
                primaryTarget.UpdateShield(newShieldValue); 
            }
        }
        else
        {
            // No shields, all damage goes to health
            shieldDamage = 0;
            healthDamage = rawDamage;
        }

        // --- 3. Apply Health Damage (if any overflowed) ---
        if (healthDamage > 0)
        {
            primaryTarget.TakeDamage(healthDamage, abilityName);
        }

        // --- 4. Handle Costs and Cooldowns (Assuming they are defined in AbilitySO) ---
        // source.CurrentEnergy -= energyCost;
        // source.CurrentAmmo -= ammoCost;
        // source.ApplyCooldown(cooldown);
    }
}
