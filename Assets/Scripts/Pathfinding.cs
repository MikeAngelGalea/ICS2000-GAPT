using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    // Define class member variable to store the last seeker position
    private Vector3 lastSeekerPosition;

    // Define a threshold for considering seeker position changes
    public float updateThreshold = 0.1f;

    public Transform seeker, target;

    // Define the size of the grid world
    public Vector2 gridWorldSize;

    // Define grid size variables
    private int gridSizeX;
    private int gridSizeY;
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();

        if (grid == null)
        {
            Debug.LogError("Grid component not found!");
        }

        // Assign values to gridWorldSize, gridSizeX, and gridSizeY
        gridWorldSize = new Vector2(10, 10);
        gridSizeX = 20;
        gridSizeY = 20;
    }

    void Update()
    {
        // Check if seeker and target are assigned
        if (seeker == null || target == null)
        {
            return;
        }

        // Round the seeker position to avoid minor differences causing continuous updates
        Vector3 currentPosition = new Vector3(Mathf.Round(seeker.position.x), Mathf.Round(seeker.position.y), Mathf.Round(seeker.position.z));

        // Check if the seeker position has significantly changed before triggering a pathfinding update
        if (Vector3.Distance(currentPosition, lastSeekerPosition) > updateThreshold)
        {
            lastSeekerPosition = currentPosition;
            FindPathForSeeker();
        }

        // Debug output to check the seeker position
        Debug.Log("Seeker Position: " + currentPosition);
    }

    public void FindPathForSeeker()
{
    if (seeker == null || target == null)
    {
        return;
    }

<<<<<<< Updated upstream
    Debug.Log("Seeker position: " + seeker.position);
    Debug.Log("Target position: " + target.position);
=======
    // Get the current seeker position
    Vector3 seekerPosition = seeker.position;
>>>>>>> Stashed changes

    // Get the middle value of the seeker's position for the X coordinate
    float middleX = seekerPosition.x;

    // Round the Y coordinate of the seeker's position
    float roundedY = Mathf.Round(seekerPosition.y);

    // Create a new vector using the calculated middle X and rounded Y
    Vector3 startPosition = new Vector3(middleX, roundedY, seekerPosition.z);

    // Get the target position
    Vector3 targetPosition = target.position;

    // Find the path for the seeker
    FindPath(startPosition, targetPosition);

    // Update the seeker's position
    seeker.position = startPosition;

    // Update the seeker's blue box position to match the seeker's position
    // Assuming that the blue box is a child of the seeker GameObject
    seeker.GetChild(0).position = startPosition;
}

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Convert the world position to the closest node in the grid
        return grid.NodeFromWorldPoint(worldPosition);
    }

    Vector3 GetPlayerPosition()
    {
        if (seeker == null)
        {
            Debug.LogWarning("Player object is missing!");
            return Vector3.zero; // Return a default position if the player object is missing
        }
        // Return the current player's position
        Vector3 playerPosition = seeker.transform.position;
        Debug.Log("Player position: " + playerPosition); // Output player position for debugging
        return playerPosition;
    }

<<<<<<< Updated upstream
    void FindPath(Vector3 startPos, Vector3 targetPos) {
        if (grid == null) {
            Debug.LogError("Grid component not found! Aborting pathfinding.");
=======
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Debug.Log("Finding path from " + startPos + " to " + targetPos);
        if (grid == null)
        {
            //Debug.LogError("Grid component not found! Aborting pathfinding.");
>>>>>>> Stashed changes
            return;
        }
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

<<<<<<< Updated upstream
        if (targetNode == null) {
            Debug.LogError("Target node not found!");
=======
        if (targetNode == null)
        {
            //Debug.LogError("Target node not found!");
>>>>>>> Stashed changes
            return;
        }

        Debug.Log("Start node: " + startNode.worldPosition);
        Debug.Log("Target node: " + targetNode.worldPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(node))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        grid.UpdatePath(path);
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return 10 * (dstX + dstY);
    }
}
