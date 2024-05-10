using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public List<Node> path;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public int numGrids = 2; // Number of grids to spawn initially
    public float gridSpawnOffset = 10f; // Offset between grids

    Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    List<GameObject> gridObjects = new List<GameObject>();
    float currentGridOffset = 0;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        // Spawn initial grids
        for (int i = 0; i < numGrids; i++)
        {
            SpawnGrid();
        }
    }

    void Update()
    {
        Debug.Log("Update method called grid!");
        UpdateGridPosition();
    }

    void UpdateGridPosition()
    {
        // Only update grid position if the player moves a certain distance
        float playerYPosition = transform.position.y;
        float offset = playerYPosition - currentGridOffset;

        if (offset > gridSpawnOffset)
        {
            // Spawn a new grid behind the current grid
            SpawnGrid();
            // Destroy the first grid
            DestroyGrid();
            // Update the current grid offset
            currentGridOffset += gridSpawnOffset;
        }
    }

    void SpawnGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        GameObject gridObject = new GameObject("Grid");
        gridObject.transform.parent = transform;
        gridObject.transform.position = new Vector3(transform.position.x, currentGridOffset, transform.position.z);
        gridObjects.Add(gridObject);

        Vector3 worldBottomLeft = gridObject.transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = (Vector2)worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(new Vector3(worldPoint.x, worldPoint.y, 0), nodeRadius, unwalkableMask));

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    void DestroyGrid()
    {
        if (gridObjects.Count > 0)
        {
            Destroy(gridObjects[0]);
            gridObjects.RemoveAt(0);
        }
    }

    // Other methods for pathfinding and grid manipulation...

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        int[] dx = { -1, 1, 0, 0 }; // Left, Right, Up, Down
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < dx.Length; i++)
        {
            int checkX = node.gridX + dx[i];
            int checkY = node.gridY + dy[i];

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                neighbours.Add(grid[checkX, checkY]); // Get neighbours from the first grid
            }
        }

        return neighbours;
    }

    bool IsNodeWalkable(Node node)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(node.worldPosition, nodeRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Obstacles") || collider.gameObject.layer == LayerMask.NameToLayer("UnWalkable"))
            {
                return false;
            }
        }

        return true;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2f) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2f) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y]; // Return node from the first grid
    }

    public void UpdatePath(List<Node> newPath)
    {
        path = newPath;
    }

    void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                if (n == null || n.worldPosition == null) continue;

                if (!n.walkable)
                {
                    Gizmos.color = Color.red;
                }
                else if (path != null && path.Contains(n))
                {
                    Gizmos.color = Color.black;
                }
                else
                {
                    Gizmos.color = Color.white;
                }

                Gizmos.DrawCube(n.worldPosition, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0));
            }

            // Debugging the start and end nodes of the path
            if (path != null && path.Count > 0)
            {
                // Output the position of the start (blue) box
                Debug.Log("Start node position: " + path[0].worldPosition);

                // Output the position of the end (green) box
                Debug.Log("End node position: " + path[path.Count - 1].worldPosition);

                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[0].worldPosition, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0));

                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[path.Count - 1].worldPosition, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0));
            }
        }
    }
}
