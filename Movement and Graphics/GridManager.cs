using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int width, height;
    public Tile tilePrefab;
    public GameObject asteroidPrefab; // <-- assign your asteroid prefab here
    public Transform cam;

    private Dictionary<Vector2, Tile> tiles;

    [Range(0f, 1f)]
    public float obstacleChance = 0.15f; // 15% chance per tile

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Create a walkable tile first
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.transform.parent = transform;

                // Randomly assign obstacle
                bool isObstacle = Random.value < obstacleChance;

                if (isObstacle)
                {
                    spawnedTile.Init(true);

                    // Spawn asteroid visual
                    var asteroid = Instantiate(asteroidPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    asteroid.name = $"Asteroid {x} {y}";
                    asteroid.transform.parent = spawnedTile.transform;
                }
                else
                {
                    spawnedTile.Init(false);
                }

                tiles[new Vector2(x, y)] = spawnedTile;
            }
        }

        // Center the camera
        cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        tiles.TryGetValue(pos, out var tile);
        return tile;
    }
}
