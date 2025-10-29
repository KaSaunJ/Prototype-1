using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GridManager grid;
    public int movementRange = 5;
    public Tile currentTile;

    void Start()
    {
        // Try to auto-assign GridManager if not manually assigned
        if (grid == null)
        {
            grid = FindObjectOfType<GridManager>();
            if (grid == null)
            {
                Debug.LogError("GridManager not found in scene!");
                return;
            }
        }

        Vector3 pos = transform.position;
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.z);

        // Clamp to grid bounds (prevents out of bounds crash)
        x = Mathf.Clamp(x, 0, grid.width - 1);
        y = Mathf.Clamp(y, 0, grid.height - 1);

        currentTile = grid.grid[x, y];

        if (currentTile == null)
        {
            Debug.LogError($"No tile found at position ({x},{y})!");
        }
        else
        {
            // Snap unit to tile's position + small offset
            transform.position = currentTile.transform.position + Vector3.up * 1f;
            Debug.Log("Unit assigned to tile: " + currentTile.name);
        }
    }

    public void ShowMovementRange()
    {
        if (currentTile == null) return;

        List<Tile> inRange = GetTilesInRange(currentTile, movementRange);
        foreach (var tile in inRange)
        {
            Renderer rend = tile.GetComponent<Renderer>();
            if (rend != null)
                rend.material.color = Color.cyan;
        }
    }

    List<Tile> GetTilesInRange(Tile start, int range)
    {
        Queue<Tile> frontier = new Queue<Tile>();
        Dictionary<Tile, int> costSoFar = new Dictionary<Tile, int>();
        List<Tile> results = new List<Tile>();

        frontier.Enqueue(start);
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Tile current = frontier.Dequeue();
            results.Add(current);

            foreach (Tile next in Pathfinding.GetNeighbors(current, grid))
            {
                if (!next.walkable) continue;

                int newCost = costSoFar[current] + 1;
                if (!costSoFar.ContainsKey(next) && newCost <= range)
                {
                    costSoFar[next] = newCost;
                    frontier.Enqueue(next);
                }
            }
        }

        return results;
    }

    public void MoveAlongPath(List<Tile> path)
    {
        if (path != null && path.Count > 0)
            StartCoroutine(MoveRoutine(path));
    }

    IEnumerator MoveRoutine(List<Tile> path)
    {
        foreach (Tile t in path)
        {
            Vector3 start = transform.position;
            Vector3 end = t.transform.position + Vector3.up * 1f;
            float tElapsed = 0f;
            float duration = 0.3f;

            while (tElapsed < duration)
            {
                transform.position = Vector3.Lerp(start, end, tElapsed / duration);
                tElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
            currentTile = t;
        }
    }
}
