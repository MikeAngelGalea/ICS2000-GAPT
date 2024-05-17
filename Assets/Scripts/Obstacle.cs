using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float CameraSpeed;

    private GameObject Player;

    // calling start before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Border")
        {
            Destroy(this.gameObject);
        }
    }

    // calling update once per frame
    void Update()
    {
        // moving the camera downwards
        transform.position -= new Vector3(0, CameraSpeed * Time.deltaTime, 0);
    }
}
