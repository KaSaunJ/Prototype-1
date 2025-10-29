using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Pathfinding
{
    public static List<Tile> FindPath(Tile start, Tile target, GridManager grid)
    {
        if (start == null || target == null || grid == null)
        {
            Debug.LogError("FindPath received null input: start, target, or grid.");
            return null;
        }

        List<Tile> openSet = new List<Tile> { start };
        HashSet<Tile> closedSet = new HashSet<Tile>();

        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
        Dictionary<Tile, int> gScore = new Dictionary<Tile, int>();
        Dictionary<Tile, int> fScore = new Dictionary<Tile, int>();

        foreach (var tile in grid.grid)
        {
            if (tile == null) continue;

            gScore[tile] = int.MaxValue;
            fScore[tile] = int.MaxValue;
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, target);

        while (openSet.Count > 0)
        {
            Tile current = openSet.OrderBy(t => fScore.ContainsKey(t) ? fScore[t] : int.MaxValue).First();

            if (current == target)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Tile neighbor in GetNeighbors(current, grid))
            {
                if (neighbor == null || !neighbor.walkable || closedSet.Contains(neighbor))
                    continue;

                int elevationPenalty = Mathf.RoundToInt(Mathf.Abs(neighbor.elevation - current.elevation));
                int tentativeGScore = gScore[current] + 1 + elevationPenalty;

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + Heuristic(neighbor, target);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; // no path found
    }

    private static int Heuristic(Tile a, Tile b)
    {
        return Mathf.Abs(a.gridX - b.gridX) + Mathf.Abs(a.gridY - b.gridY);
    }

    public static List<Tile> GetNeighbors(Tile tile, GridManager grid)
    {
        List<Tile> neighbors = new List<Tile>();

        int[,] directions = new int[,]
        {
            { 0, 1 },
            { 1, 0 },
            { 0, -1 },
            { -1, 0 }
        };

        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int checkX = tile.gridX + directions[i, 0];
            int checkY = tile.gridY + directions[i, 1];

            if (checkX >= 0 && checkX < grid.width && checkY >= 0 && checkY < grid.height)
            {
                Tile neighbor = grid.grid[checkX, checkY];
                if (neighbor != null)
                    neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    private static List<Tile> ReconstructPath(Dictionary<Tile, Tile> cameFrom, Tile current)
    {
        List<Tile> path = new List<Tile> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();
        return path;
    }
}
