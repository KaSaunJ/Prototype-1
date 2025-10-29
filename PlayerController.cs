using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private Unit unit;

    void Start()
    {
        unit = GetComponent<Unit>();
        if (unit == null)
            Debug.LogError("No Unit component found on PlayerController object.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>();

                if (tile != null && tile.walkable && unit != null && unit.currentTile != null && unit.grid != null)
                {
                    List<Tile> path = Pathfinding.FindPath(unit.currentTile, tile, unit.grid);
                    if (path != null && path.Count <= unit.movementRange)
                        unit.MoveAlongPath(path);
                }
            }
        }
    }
}
