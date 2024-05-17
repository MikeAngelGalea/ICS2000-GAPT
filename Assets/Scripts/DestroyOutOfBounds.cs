using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    // screen boundaries definitions
    private float topBoundary;
    private float bottomBoundary;
    private float leftBoundary;
    private float rightBoundary;

    // delay by using bufferzone
    public float bufferZone = 1f;

    void Start()
    {
        // screen boundaries based on the camera's perspective are calculated
        CalculateScreenBoundaries();
    }

    void Update()
    {
        // here we are checking if the obstacle is outside the screen boundaries with buffer zone
        if (transform.position.y < bottomBoundary - bufferZone - 100 ||
            transform.position.y > topBoundary + bufferZone + 100||
            transform.position.x < leftBoundary - bufferZone - 100||
            transform.position.x > rightBoundary + bufferZone + 100 )
        {
            // obsatcle is destroyed if it is outside
            Destroy(gameObject);
        }
    }

    void CalculateScreenBoundaries()
    {
        // camera's viewport boundaries
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(-0.1f, -0.1f, Camera.main.nearClipPlane));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, Camera.main.nearClipPlane));

        // assigning screen boundaries with buffer zone
        leftBoundary = bottomLeft.x;
        bottomBoundary = bottomLeft.y;
        rightBoundary = topRight.x;
        topBoundary = topRight.y;
    }
}
