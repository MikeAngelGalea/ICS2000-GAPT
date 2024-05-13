// Grid.cs
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
                bool walkable = IsNodeWalkable(worldPoint); // Use the method to check if node is walkable

                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    bool IsNodeWalkable(Vector2 worldPosition)
    {
         Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPosition, nodeRadius);
    foreach (Collider2D collider in colliders)
    {
        if (collider != null && (collider.CompareTag("Obstacles") || collider.gameObject.layer == LayerMask.NameToLayer("Unwalkable")))
        {
            Debug.Log("Unwalkable node detected at position: " + worldPosition);
            return false;
        }
    }
    Debug.Log("Walkable node detected at position: " + worldPosition);
    return true;
    }

    void DestroyGrid()
    {
        if (gridObjects.Count > 0)
        {
            Destroy(gridObjects[0]);
            gridObjects.RemoveAt(0);
        }
    }

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

    public Node NodeFromWorldPoint(Vector3 worldPosition)
{
    Node closestNode = null;
    float closestDistance = float.MaxValue;

    foreach (GameObject gridObject in gridObjects)
    {
        // Calculate the position of the bottom left corner of the grid
        Vector3 worldBottomLeft = gridObject.transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        // Convert the world position to grid coordinates within the current grid object
        float percentX = Mathf.Clamp01((worldPosition.x - worldBottomLeft.x) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y - worldBottomLeft.y) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        // Ensure the coordinates are within bounds
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);

        // Get the node at the calculated coordinates
        Node node = grid[x, y];

        // Calculate the distance from the world position to the node
        float distance = Vector3.Distance(worldPosition, node.worldPosition);

        // Update the closest node if the current node is closer
        if (distance < closestDistance)
        {
            closestNode = node;
            closestDistance = distance;
        }
    }

    return closestNode;
}

    public void UpdatePath(List<Node> newPath)
    {
        path = newPath;
    }

    void OnDrawGizmos()
{
    Debug.Log("Drawing gizmos...");
    if (grid != null)
    {
        foreach (Node n in grid)
        {
            //Debug.Log("Processing node at position: " + n.worldPosition);

            if (!n.walkable)
            {
                //Debug.Log("Node at position " + n.worldPosition + " is unwalkable. Setting color to red.");
                Gizmos.color = Color.red;
            }
            else if (path != null && path.Contains(n))
            {
                //Debug.Log("Node at position " + n.worldPosition + " is part of the path. Setting color to black.");
                Gizmos.color = Color.black;
            }
            else
            {
                //Debug.Log("Node at position " + n.worldPosition + " is walkable and not part of the path. Setting color to white.");
                Gizmos.color = Color.white;
            }

            Gizmos.DrawCube(n.worldPosition, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0));
        }
    



            // Debugging the start and end nodes of the path
            if (path != null && path.Count > 0)
            {
                // Output the position of the start (blue) box
                //Debug.Log("Start node position: " + path[0].worldPosition);

                // Output the position of the end (green) box
                //Debug.Log("End node position: " + path[path.Count - 1].worldPosition);

                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[0].worldPosition, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0));

                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[path.Count - 1].worldPosition, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0));
            }
        }
    }
}