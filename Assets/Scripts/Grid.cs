using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    Camera mainCamera;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
        //test
        mainCamera = Camera.main;
    }

    void Update()
    {
        UpdateGridPosition();
        UpdateGrid();
    }

    void UpdateGridPosition()
    {
        transform.position = mainCamera.transform.position;
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridWorldSize.x / 2 - Vector2.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = (Vector2)worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(new Vector3(worldPoint.x, worldPoint.y, 0), nodeRadius, unwalkableMask));

                grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }

    public List<Node> GetNeighbours(Node node) {
    List<Node> neighbours = new List<Node>();

    // Define the directions to check for neighbours
    int[] dx = { -1, 1, 0, 0 }; // Left, Right, Up, Down
    int[] dy = { 0, 0, 1, -1 };

    for (int i = 0; i < dx.Length; i++) {
        int checkX = node.gridX + dx[i];
        int checkY = node.gridY + dy[i];

        // Check if the neighbour is within the grid bounds
        if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
            neighbours.Add(grid[checkX, checkY]);
        }
    }

    return neighbours;
}


    void UpdateGrid()
    {
        // Iterate through each node in the grid
        foreach (Node node in grid)
        {
            // Check if the node is walkable based on game logic
            bool walkable = IsNodeWalkable(node);

            // Update the node's walkable status
            node.walkable = walkable;
        }
    }

    bool IsNodeWalkable(Node node)
    {
        // Check if any colliders intersect with the node
        Collider2D[] colliders = Physics2D.OverlapCircleAll(node.worldPosition, nodeRadius);

        // Iterate through each collider
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider belongs to an obstacle prefab by tag or layer
            if (collider.CompareTag("Obstacles") || collider.gameObject.layer == LayerMask.NameToLayer("UnWalkable"))
            {
                return false; // Node is unwalkable if obstacle is found
            }
        }

        return true; // Node is walkable if no obstacles are found
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition) {
    float percentX = (worldPosition.x + gridWorldSize.x / 2f) / gridWorldSize.x;
    float percentY = (worldPosition.z + gridWorldSize.y / 2f) / gridWorldSize.y; // Using z for 2D

    percentX = Mathf.Clamp01(percentX);
    percentY = Mathf.Clamp01(percentY);

    int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
    int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

    return grid[x, y];
}

public List<Node> path;
    void OnDrawGizmos()
{
    // Get the current camera position
    Vector3 cameraPosition = mainCamera.transform.position;

    // Draw the grid wireframe
    Vector3 gridPosition = transform.position;
    Gizmos.DrawWireCube(gridPosition, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));

    if (grid != null)
    {
        foreach (Node n in grid)
        {
            // Calculate the node's position relative to the camera movement
            Vector3 nodePosition = new Vector3(n.worldPosition.x + gridPosition.x - cameraPosition.x,
                                               n.worldPosition.y + gridPosition.y - cameraPosition.y,
                                               0);

            // Draw the node with different colors based on walkability
            Gizmos.color = (n.walkable) ? Color.white : Color.red;
            if (path != null && path.Contains(n))
                Gizmos.color = Color.black;

            // Draw cube as a square in 2D
            Gizmos.DrawCube(nodePosition, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0));
        }
    }
}


}
