using UnityEngine;
using System.Collections.Generic;


public class Grid : MonoBehaviour
{
    public List<Node> path;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;


    GameObject player; // Reference to the player object
    GameObject collectible; // Reference to the collectible object


    Camera mainCamera;
//michele
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

        mainCamera = Camera.main;


        player = GameObject.FindGameObjectWithTag("Player");
        collectible = GameObject.FindGameObjectWithTag("Collectible");

        if (player == null)
            Debug.LogError("Player object not found! Make sure it is tagged as 'Player'.");
        
        if (collectible == null)
            Debug.LogError("Collectible object not found! Make sure it is tagged as 'Collectible'.");

    }

    void Update()
    {
        UpdateGrid();
    }

    void UpdateGridPosition()
    {
        //transform.position = mainCamera.transform.position;
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

    public void UpdatePath(List<Node> newPath)
{
    path = newPath;
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


   void OnDrawGizmos()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                // Set color based on walkability and obstacles
                if (!n.walkable)
                {
                    Gizmos.color = Color.red; // Color nodes with obstacles red
                }
                else if (path != null && path.Contains(n))
                {
                    Gizmos.color = Color.black; // Color path nodes black
                }
                else if (IsNodeSeeker(n))
                {
                    Gizmos.color = new Color(1.0f, 0.84f, 0.0f); // Color player node gold
                }
                else if (IsNodeObstacle(n))
                {
                    Gizmos.color = Color.blue; // Color obstacle nodes blue
                }
                else if (IsNodeTarget(n))
                {
                    Gizmos.color = Color.green; // Color target nodes green
                }
                else
                {
                    Gizmos.color = Color.white; // Color walkable nodes white
                }

                // Draw cube as a square in 2D
                Gizmos.DrawCube(n.worldPosition, new Vector3(nodeDiameter - 0.1f, nodeDiameter - 0.1f, 0));
            }
        }
    }


bool IsNodeObstacle(Node node)
{
    // Check if there is any collider at the node's position
    Collider2D[] colliders = Physics2D.OverlapCircleAll(node.worldPosition, nodeRadius);
    foreach (Collider2D collider in colliders)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Unwalkable"))
        {
            return true;
        }
    }
    return false;
}

bool IsNodeSeeker(Node node)
    {
        // Check if the node's position matches the player's position
        if (player != null && Vector3.Distance(node.worldPosition, player.transform.position) < nodeRadius)
        {
            return true;
        }
        return false;
    }

bool IsNodeTarget(Node node)
    {
        // Check if the node's position matches the collectible's position
        if (collectible != null && Vector3.Distance(node.worldPosition, collectible.transform.position) < nodeRadius)
        {
            return true;
        }
        return false;
    }
}




