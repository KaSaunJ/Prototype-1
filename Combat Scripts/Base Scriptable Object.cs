using UnityEngine;

/// <summary>
/// Base class for all combat abilities. Inheriting from ScriptableObject allows us
/// to define ability data assets in the Unity Editor.
/// </summary>
public abstract class AbilitySO : ScriptableObject
{
    [Header("Ability Core Data")]
    public string abilityName = "New Ability";
    public string description = "A basic combat action.";
    public int baseDamage = 30;

    [Range(0f, 1f)]
    [Tooltip("The percentage of damage that bypasses the shield and goes directly to health.")]
    public float shieldPenetration = 0.0f; // 0.0 = fully absorbed by shield, 1.0 = fully ignores shield

    /// <summary>
    /// The execution logic for the ability. This is implemented in concrete subclasses.
    /// </summary>
    /// <param name="source">The ship using the ability.</param>
    /// <param name="target">The ship being targeted.</param>
    public abstract void Execute(ShipUnit source, ShipUnit target);
}
