using System.Collections.Generic;
using UnityEngine;

public static class AStarPathfinding
{
    private class Node
    {
        public Vector2 pos;
        public float gCost;
        public float hCost;
        public float fCost => gCost + hCost;
        public Node parent;

        public Node(Vector2 pos) { this.pos = pos; }
    }

    public static List<Vector2> FindPath(GridManager grid, Vector2 start, Vector2 end)
    {
        Vector2 startPos = new Vector2(Mathf.Round(start.x), Mathf.Round(start.y));
        Vector2 endPos = new Vector2(Mathf.Round(end.x), Mathf.Round(end.y));

        var openList = new List<Node>();
        var closedList = new HashSet<Vector2>();

        Node startNode = new Node(startPos);
        Node endNode = new Node(endPos);

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node current = openList[0];
            foreach (var node in openList)
                if (node.fCost < current.fCost || (node.fCost == current.fCost && node.hCost < current.hCost))
                    current = node;

            openList.Remove(current);
            closedList.Add(current.pos);

            if (current.pos == endPos)
                return RetracePath(startNode, current);

            foreach (var neighbor in GetNeighbors(grid, current))
            {
                if (closedList.Contains(neighbor.pos)) continue;

                float newGCost = current.gCost + Vector2.Distance(current.pos, neighbor.pos);
                if (newGCost < neighbor.gCost || !openList.Exists(n => n.pos == neighbor.pos))
                {
                    neighbor.gCost = newGCost;
                    neighbor.hCost = Vector2.Distance(neighbor.pos, endPos);
                    neighbor.parent = current;

                    if (!openList.Exists(n => n.pos == neighbor.pos))
                        openList.Add(neighbor);
                }
            }
        }

        return null;
    }

    private static List<Vector2> RetracePath(Node startNode, Node endNode)
    {
        List<Vector2> path = new List<Vector2>();
        Node current = endNode;

        while (current != startNode)
        {
            path.Add(current.pos);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }

    private static List<Node> GetNeighbors(GridManager grid, Node node)
    {
        List<Node> neighbors = new List<Node>();
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        foreach (var dir in directions)
        {
            Vector2 checkPos = node.pos + dir;
            Tile tile = grid.GetTileAtPosition(checkPos);
            if (tile != null && !tile.isObstacle)
            {
                neighbors.Add(new Node(checkPos));
            }
        }
        return neighbors;
    }
}
