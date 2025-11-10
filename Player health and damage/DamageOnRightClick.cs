using UnityEngine;

public class DamageOnRightClick : MonoBehaviour
{
    public PlayHealth playerHealth;
    public float damageAmount = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Debug.Log("Right-click detected"); // <-- test if input works
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
