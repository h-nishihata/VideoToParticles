using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Depth : MonoBehaviour
{
    public Material mat;
    void Start()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

    public void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        Graphics.Blit(source, dest, mat);
    }
}