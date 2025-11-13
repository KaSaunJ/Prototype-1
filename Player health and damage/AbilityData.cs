using UnityEngine;

// This allows you to create new AbilityData assets by right-clicking 
// in your Project Window: Create -> Abilities -> New Ability
[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/New Ability Data")]
public class AbilityData : ScriptableObject
{
    [Header("UI Info")]
    public string abilityName = "New Skill";
    public string description = "A new ship ability.";


    [Header("Stats")]
    [Tooltip("The time in turns before the ability can be used again.")]
    // Cooldown is now measured in discrete turns (e.g., 3 turns)
    public int cooldownTurns = 3;

    [Tooltip("The amount of damage this ability inflicts.")]
    public float damageAmount = 10f;
    
   /*
   Note: Not sure if we want to implement these but hey have at it if you want. Or we can work on it together.
   public boolean damage effect
    */
    //Note: I wanted to add range however I dont know how the grid works and there should be a highlight voer it so the player can dedtermine where to go instead of clicking randomly.
    //public float range = ;
    
    [Tooltip("The Unity input key that triggers this ability.")]
    public KeyCode triggerKey = KeyCode.Alpha1; // Default to '1' key
    
    // Add more stats here (e.g., energyCost, range, duration, visualEffectPrefab)
    // public float energyCost = 20f;
}