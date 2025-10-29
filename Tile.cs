using UnityEngine;


public class Tile : MonoBehaviour
{
    public int gridX, gridY;
    public bool walkable = true;
    public float elevation => transform.position.y;
}
