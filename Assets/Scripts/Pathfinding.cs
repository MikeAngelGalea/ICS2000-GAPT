using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public PlayerController playerController;
    public Transform seeker;
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        FindClosestCollectible();
    }

    void FindClosestCollectible()
    {
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        Node closestCollectible = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject collectible in collectibles)
        {
            Node collectibleNode = grid.NodeFromWorldPoint(collectible.transform.position);
            float distanceToCollectible = Vector3.Distance(seeker.position, collectibleNode.worldPosition);

            if (distanceToCollectible < shortestDistance)
            {
                shortestDistance = distanceToCollectible;
                closestCollectible = collectibleNode;
            }
        }

        if (closestCollectible != null)
        {
            Vector3 closestCollectiblePosition = closestCollectible.worldPosition;
            //Debug.Log("Closest Collectible Position: " + closestCollectiblePosition);
            FindPath(seeker.position, closestCollectiblePosition);
        }
    }

    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // Debug output
        //Debug.Log("Start Node: " + startNode.worldPosition);
        //Debug.Log("Target Node: " + targetNode.worldPosition);


		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
					if (openSet[i].hCost < node.hCost)
						node = openSet[i];
				}
			}

            openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode) {
				RetracePath(startNode,targetNode);
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

		grid.path = path;

	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}

    // Method to set the path for the player
    public void SetPath(Vector3[] newPath)
    {
        // Pass the new path to the PlayerController script
        playerController.SetPath(newPath);
    }
}
