using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float CameraSpeed;

    // it is updating once per frame
    void Update()
    {
        transform.position -= new Vector3(0, CameraSpeed * Time.deltaTime, 0);
    }
}

