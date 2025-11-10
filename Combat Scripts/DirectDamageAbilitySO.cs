// DirectDamageAbilitySO.cs (Modified)
using UnityEngine;

// Assumes this file inherits from AbilitySO or a similar base class that defines Execute()
public class DirectDamageAbilitySO : AbilitySO
{
    [Header("Damage Properties")]
    // FIX CS0108: Use 'new' keyword to explicitly hide the inherited baseDamage property from AbilitySO.
    [Tooltip("Base damage amount of the ability.")]
    public new int baseDamage = 20; // Hides the base class member
    
    [Tooltip("Multiplier applied to the source's Attack stat (e.g., 1.5 for 150% damage).")]
    public float damageMultiplier = 1.0f;

    /// <summary>
    /// Executes the direct damage ability on the primary target.
    /// </summary>
    public override void Execute(ShipUnit source, ShipUnit primaryTarget)
    {
        if (primaryTarget == null || primaryTarget.CurrentHealth <= 0)
        {
            Debug.LogWarning($"{source.ShipName}'s {abilityName} failed: No valid target.");
            return;
        }

        // --- 1. Calculate Raw Damage ---
        int rawDamage = baseDamage + Mathf.RoundToInt(source.shipData.baseAttack * damageMultiplier);
        
        Debug.Log($"<color=green>{source.ShipName}</color> fires {abilityName} at <color=red>{primaryTarget.ShipName}</color> for {rawDamage} base damage (Penetration: {shieldPenetration * 100}%).");

        // --- 2. Calculate Shield Damage and Health Penetration ---
        
        // Damage that bypasses shields and goes straight to health
        int penetrationDamage = Mathf.RoundToInt(rawDamage * shieldPenetration); 
        // Damage that must be absorbed by the shield
        int shieldAbsorbDamage = rawDamage - penetrationDamage;

        // --- 3. Resolve Shield Absorption ---
        int currentShield = primaryTarget.CurrentShield;
        int healthDamage = penetrationDamage; // Start with penetration damage

        if (currentShield > 0)
        {
            if (shieldAbsorbDamage > currentShield)
            {
                // Shield damage overflows: shields are completely depleted
                int absorbedDamage = currentShield;
                int overflowDamage = shieldAbsorbDamage - currentShield;
                
                healthDamage += overflowDamage; // Add overflow to health damage
                
                Debug.Log($"Shields broken! {absorbedDamage} absorbed. {overflowDamage} overflow damage added to health.");
                
                primaryTarget.UpdateShield(0); 
            }
            else
            {
                // Shields absorb all shield-targetting damage
                int newShieldValue = currentShield - shieldAbsorbDamage;
                
                Debug.Log($"Shields absorb {shieldAbsorbDamage}. New shield value: {newShieldValue}.");
                
                primaryTarget.UpdateShield(newShieldValue); 
            }
        }
        else
        {
            // No shields, all shield-absorb damage also goes to health
            healthDamage += shieldAbsorbDamage;
        }

        // --- 4. Apply Final Health Damage ---
        if (healthDamage > 0)
        {
            primaryTarget.TakeDamage(healthDamage, abilityName);
        }
    }
}