using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    public GameObject obstacle;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    public float TimeBetweenSpawn;
    private float SpawnTime;

    public float colliderRadius = 1f;

    // Update is called once per frame
    void Update()
    {
        if(Time.time>SpawnTime){
            Spawn();
            SpawnTime = Time.time + TimeBetweenSpawn;
        }
        
    }

    void Spawn()
    {
        float X=Random.Range(minX,maxY);
        float Y=Random.Range(minY,maxX);

        GameObject spawnedObstacle = Instantiate(obstacle, transform.position + new Vector3(X, Y, 0), transform.rotation);
        
        // Add Circle Collider component
        CircleCollider2D collider = spawnedObstacle.AddComponent<CircleCollider2D>();
        collider.radius = colliderRadius;
    }
}
