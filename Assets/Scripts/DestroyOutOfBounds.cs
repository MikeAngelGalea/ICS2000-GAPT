using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // Define the screen boundaries
    private float topBoundary;
    private float bottomBoundary;
    private float leftBoundary;
    private float rightBoundary;

    // Buffer zone to delay destruction after passing the screen boundaries
    public float bufferZone = 1f;

    void Start()
    {
        // Calculate screen boundaries based on the camera's perspective
        CalculateScreenBoundaries();
    }

    void Update()
    {
        // Check if the obstacle is outside the screen boundaries with buffer zone
        if (transform.position.y < bottomBoundary - bufferZone - 100 ||
            transform.position.y > topBoundary + bufferZone + 100||
            transform.position.x < leftBoundary - bufferZone - 100||
            transform.position.x > rightBoundary + bufferZone + 100 )
        {
            // If it's outside, destroy it
            Destroy(gameObject);
        }
    }

    void CalculateScreenBoundaries()
    {
        // Get the camera's viewport boundaries
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, Camera.main.nearClipPlane));

        // Assign screen boundaries with buffer zone
        leftBoundary = bottomLeft.x;
        bottomBoundary = bottomLeft.y;
        rightBoundary = topRight.x;
        topBoundary = topRight.y;
    }
}
