using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private GridManager grid;
    private bool isMoving = false; 

    private Transform spriteTransform; // child transform for sprite
    private Vector2 lastMoveDir = Vector2.down; // stores last movement direction

    void Start()
    {
        grid = FindObjectOfType<GridManager>();
        spriteTransform = transform.Find("Sprite"); // make sure your sprite object is named "Sprite" in the hierarchy
    }

    void Update()
    {
        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPos = new Vector2(Mathf.Round(mouseWorldPos.x), Mathf.Round(mouseWorldPos.y));

            Tile clickedTile = grid.GetTileAtPosition(gridPos);

            if (clickedTile != null && !clickedTile.isObstacle)
            {
                StartCoroutine(MoveToTile(clickedTile));
            }
        }
    }

    IEnumerator MoveToTile(Tile destination)
    {
        isMoving = true;

        var path = AStarPathfinding.FindPath(grid, transform.position, destination.transform.position);
        if (path == null)
        {
            isMoving = false;
            yield break;
        }

        foreach (var step in path)
        {
            Vector3 targetPos = new Vector3(step.x, step.y, 0);

            // Move toward this step
            while ((transform.position - targetPos).sqrMagnitude > 0.01f)
            {
                Vector2 moveDir = ((Vector2)targetPos - (Vector2)transform.position).normalized;

                // Update facing direction
                if (moveDir.sqrMagnitude > 0.001f)
                {
                    lastMoveDir = moveDir;
                    UpdateSpriteFacing(moveDir);
                }

                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        isMoving = false;
    }

    private void UpdateSpriteFacing(Vector2 moveDir)
    {
        if (spriteTransform == null) return;

        // 4-directional facing
        if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
        {
            // Moving horizontally
            if (moveDir.x > 0)
                spriteTransform.localRotation = Quaternion.Euler(0, 0, -90); // right
            else
                spriteTransform.localRotation = Quaternion.Euler(0, 0, 90);  // left
        }
        else
        {
            // Moving vertically
            if (moveDir.y > 0)
                spriteTransform.localRotation = Quaternion.Euler(0, 0, 0);   // up
            else
                spriteTransform.localRotation = Quaternion.Euler(0, 0, 180); // down
        }
    }

}
