// ShipDataSO.cs
using UnityEngine;

/// <summary>
/// Defines static ship stats. Inheriting from ScriptableObject allows 
/// asset creation for different ship types.
/// </summary>
[CreateAssetMenu(fileName = "ShipData", menuName = "ScriptableObjects/Ship Data")]
public class ShipDataSO : ScriptableObject
{
    [Header("Base Stats")]
    // Used in damage calculation by DirectDamageAbilitySO
    public int baseAttack = 10; 
    public int baseDefense = 5; 
    public int turnspeed = 0;
    // Add other static stats here (e.g., max speed, turn rate)
}