using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Pathfinding pathfinding;
    public float moveSpeed = 5f;

    private int currentPathIndex = 0;
    private Vector3[] path;

    private Grid grid;

    public GameObject GameOverPanel;

    public HealthScript healthScript;
//calling start before first frame update
    void Start()
    {
        Debug.Log("PlayerController Start method called.");
        //finding and initializing of grid component
        grid = GameObject.FindObjectOfType<Grid>();
        if (grid == null)
        {
            Debug.LogError("Grid not found in the scene.");
        }
    }
//calling update once per frame
    private void Update()
    {
        Debug.Log("PlayerController Update method called.");
        //checking if path exists
        if (path != null)
        {
            Debug.Log("Path is not null.");
            if (currentPathIndex < path.Length)
            {
                Debug.Log("Current path index is within bounds: " + currentPathIndex);
                if (CanMove())
                {
                    Debug.Log("Player can move.");
                    MoveAlongPath();
                }
                else
                {
                    Debug.Log("Player cannot move. Handling obstacle.");
                    HandleObstacle();
                }
            }
            else
            {
                Debug.Log("Current path index is out of bounds: " + currentPathIndex);
            }
        }
        else
        {
            Debug.Log("Path is null.");
            // even if path is null, check for obstacles and move accordingly
            HandleObstacle();
        }
    }

    private void MoveAlongPath()
    {
        Debug.Log("Moving along path...");
        Vector3 targetPosition = path[currentPathIndex];
        Vector3 currentPosition = transform.position;

        // moving towards the target position
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

        Debug.Log("Moving towards: " + targetPosition); 

        // checking if the player reached the target position
        if (Vector3.Distance(currentPosition, targetPosition) < 0.1f)
        {
            Debug.Log("Reached target position: " + targetPosition);
            currentPathIndex++;
        }
    }
//setting of new path
    public void SetPath(Vector3[] newPath)
    {
        Debug.Log("Setting new path.");
        path = newPath;
        currentPathIndex = 0;

        if (path != null)
        {
            Debug.Log("New path set with " + path.Length + " waypoints."); // Debug statement
        }
        else
        {
            Debug.LogError("New path is null.");
        }
    }

    private bool CanMove()
    {
        Debug.Log("Checking if player can move.");
        // calculating the position of the player in grid coordinates
        Vector3 playerPosition = transform.position;
        Node playerNode = grid.NodeFromWorldPoint(playerPosition);
        Debug.Log("Player node: " + (playerNode != null ? playerNode.worldPosition.ToString() : "null"));

        // checking if there is a colored box to the right of the player
        Vector3 rightOffset = playerPosition + Vector3.right * grid.NodeDiameter; // Use NodeDiameter
        Node rightNode = grid.NodeFromWorldPoint(rightOffset);
        Debug.Log("Right node: " + (rightNode != null ? rightNode.worldPosition.ToString() : "null"));

        if (rightNode != null && IsColoredBox(rightNode))
        {
            Debug.Log("Colored box detected to the right at: " + rightNode.worldPosition); // Debug statement
            return false; // cannot move to the right
        }

        // checking if there is a colored box to the left of the player
        Vector3 leftOffset = playerPosition + Vector3.left * grid.NodeDiameter; // Use NodeDiameter
        Node leftNode = grid.NodeFromWorldPoint(leftOffset);
        Debug.Log("Left node: " + (leftNode != null ? leftNode.worldPosition.ToString() : "null"));

        if (leftNode != null && IsColoredBox(leftNode))
        {
            Debug.Log("Colored box detected to the left at: " + leftNode.worldPosition); // Debug statement
            return false; // cannot move to the left
        }

        // player can move only if no colored boxes are detected
        Debug.Log("No colored boxes detected, player can move.");
        return true;
    }

    private void HandleObstacle()
    {
        Vector3 playerPosition = transform.position;

        // checking for a colored box to the right
        Vector3 rightOffset = playerPosition + Vector3.right * grid.NodeDiameter;
        Node rightNode = grid.NodeFromWorldPoint(rightOffset);
        Debug.Log("HandleObstacle - Right node: " + (rightNode != null ? rightNode.worldPosition.ToString() : "null"));

        if (rightNode != null && IsColoredBox(rightNode))
        {
            Debug.Log("Moving player 5mm to the right into the colored box.");
            transform.position += Vector3.right * 0.5f; // move 5mm to the right
            return;
        }

        // checking for a colored box to the left
        Vector3 leftOffset = playerPosition + Vector3.left * grid.NodeDiameter;
        Node leftNode = grid.NodeFromWorldPoint(leftOffset);
        Debug.Log("HandleObstacle - Left node: " + (leftNode != null ? leftNode.worldPosition.ToString() : "null"));

        if (leftNode != null && IsColoredBox(leftNode))
        {
            Debug.Log("Moving player 5mm to the left into the colored box.");
            transform.position += Vector3.left * 0.5f; // move 5mm to the left
            return;
        }

        Debug.Log("No colored boxes detected on either side, no movement necessary.");
    }

    private bool IsColoredBox(Node node)
    {
        Debug.Log("Checking if node is colored box: " + node.worldPosition); 
        // checking the node's walkable status and path membership to determine color
        if (node != null)
        {
            if (!node.walkable)
            {
                // unwalkable node (represented as red in on drawgizmos) (meteoroids)
                return false;
            }
            else if (grid.path != null && grid.path.Contains(node))
            {
                // node in path (black in on drawgizmos)
                return true;
            }
            else if (IsNodeAboveCollectible(node.worldPosition))
            {
                // node above a collectible (green in on drawgizmos) (spanner or astronaut)
                return true;
            }
        }
        return false;
    }

    private bool IsNodeAboveCollectible(Vector2 worldPosition)
    {
        // checking if the node is above a collectible object (spanner or astronaut)
        Collider2D[] colliders = Physics2D.OverlapCircleAll(worldPosition, grid.NodeDiameter / 2);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.CompareTag("Collectible"))
            {
                return true;
            }
        }
        return false;
    }

    // making the takedamage method accessible from other classes
    public void TakeDamage(int damage)
    {
        // calling the takedamage method of healthscript
        healthScript.TakeDamage(damage);
    }
}
