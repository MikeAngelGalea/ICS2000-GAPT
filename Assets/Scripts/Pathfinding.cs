using UnityEngine;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public PlayerController playerController;
    public Transform seeker;
    Grid grid;

//awake called when instance is loaded
    void Awake()
    {
        grid = GetComponent<Grid>();
    }
//calling update once per frame
    void Update()
    {
        //continuously finding the closest collectible item
        FindClosestCollectible();
    }

    void FindClosestCollectible()
    {
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
        Node closestCollectible = null;
        float shortestDistance = Mathf.Infinity;
//iterate through each collectible
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
//if collectible is found, find path to collectible
        if (closestCollectible != null)
        {
            Vector3 closestCollectiblePosition = closestCollectible.worldPosition;
            FindPath(seeker.position, closestCollectiblePosition);
        }
    }
// method for generating path from start pos to target pos
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);



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
//calculation of new cost to reach neighbour
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
    //retracing path from end node to start node
    void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;
//traverse end node using the parent nodes
		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
        //reversing path 
		path.Reverse();
//setting path
		grid.path = path;

	}
//calculation of distance between 2 nodes
	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
//return distance with weighting for diagonal movement
		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}

    // setting the path for the player
    public void SetPath(Vector3[] newPath)
    {
        // passing the new path to the playercontroller script
        playerController.SetPath(newPath);
    }
}
