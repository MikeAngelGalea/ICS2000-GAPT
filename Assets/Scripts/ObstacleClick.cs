using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleClick : MonoBehaviour
{
    public GameObject dotPrefab;

    void Start()
    {
        
    }

    // calling update once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject g = Instantiate(dotPrefab, (Vector2)spawnPosition, Quaternion.identity);
        }
        
    }
}
