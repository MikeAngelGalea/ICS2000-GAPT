using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    public float BackgroundSpeed;
    public Renderer BackgroundRenderer;

    // Update is called once per frame
    void Update()
    {
        BackgroundRenderer.material.mainTextureOffset += new Vector2(0f, BackgroundSpeed * Time.deltaTime);
    }
}
