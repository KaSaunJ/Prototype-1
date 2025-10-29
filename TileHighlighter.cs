using UnityEngine;

public class TileHighlighter : MonoBehaviour
{
    private Renderer rend;
    private Color originalColor;
    public Color hoverColor = Color.yellow;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = originalColor;
    }
}
