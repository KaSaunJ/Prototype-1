using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isObstacle;
    private SpriteRenderer rend;

    void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    public void Init(bool obstacle)
    {
        isObstacle = obstacle;
        rend.color = obstacle ? Color.black : Color.white;
    }
}
