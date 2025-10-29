using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 50;
    public int height = 50;
    public GameObject tilePrefab;

    public Tile[,] grid;

    void Awake()
    {
        if (tilePrefab == null)
        {
            Debug.LogError("Tile Prefab is not assigned in the GridManager.");
            return;
        }

        GenerateGrid();
    }

    void GenerateGrid()
    {
        Debug.Log($"Generating grid of size: {width} x {height}");

        grid = new Tile[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject tileGO = Instantiate(tilePrefab, position, Quaternion.identity);
                tileGO.name = $"Tile {x} {y}";

                Tile tile = tileGO.GetComponent<Tile>();
                if (tile == null)
                {
                    tile = tileGO.AddComponent<Tile>();
                }

                tile.gridX = x;
                tile.gridY = y;
                tile.walkable = true; // default walkable

                grid[x, y] = tile;

                Renderer rend = tileGO.GetComponent<Renderer>();
            }
        }

        Debug.Log($"Grid generated: {width}x{height}");
    }
}
