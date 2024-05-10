using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour {

    public Transform seeker, target;
    Grid grid;

    Vector3 lastSeekerPosition;

    void Awake() {
        grid = GetComponent<Grid>();
        lastSeekerPosition = seeker.position;

        if (grid == null) {
            Debug.LogError("Grid component not found!");
        }
    }

    void Update() {
        Debug.Log("Update method called path!");
        if (seeker == null || target == null)
    {
        return;
    }

    Debug.Log("Seeker position: " + seeker.position);
    Debug.Log("Target position: " + target.position);

    // Update the seeker's position to match the player's position
    seeker.position = GetPlayerPosition();

    // Find a new path based on the updated seeker position and target position
    FindPath(seeker.position, target.position);
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

    void FindPath(Vector3 startPos, Vector3 targetPos) {
        if (grid == null) {
            Debug.LogError("Grid component not found! Aborting pathfinding.");
            return;
        }
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (targetNode == null) {
            Debug.LogError("Target node not found!");
            return;
        }

        Debug.Log("Start node: " + startNode.worldPosition);
        Debug.Log("Target node: " + targetNode.worldPosition);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0) {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
                    if (openSet[i].hCost < node.hCost)
                        node = openSet[i];
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == targetNode) {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(node)) {
                if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = node;
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode) {
    List<Node> path = new List<Node>();
    Node currentNode = endNode;

    while (currentNode != startNode) {
        path.Add(currentNode);
        currentNode = currentNode.parent;
    }
    path.Reverse();

    grid.UpdatePath(path); 
    }


    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return 10 * (dstX + dstY); 
    }

}

public static class TransformExtensions
{
    // Custom hasChanged property for Transform
    public static bool hasChanged { get; set; }
}
