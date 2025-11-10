using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int width, height;
    public Tile tilePrefab;
    public GameObject asteroidPrefab;

    private Dictionary<Vector2, Tile> tiles;

    [Range(0f, 1f)]
    public float obstacleChance = 0.15f;

    // Optional: where the player should spawn
    public Vector2 playerSpawnPosition = Vector2.zero; // default: bottom-left corner

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
                var pos = new Vector2(x, y);

                // Create tile
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.transform.parent = transform;

                bool isObstacle = false;

                // Only randomly place obstacle if it's NOT the player spawn
                if (pos != playerSpawnPosition)
                {
                    isObstacle = Random.value < obstacleChance;
                }

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

                tiles[pos] = spawnedTile;
            }
        }
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        tiles.TryGetValue(pos, out var tile);
        return tile;
    }
}
