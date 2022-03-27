using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRendering : MonoBehaviour
{
    public float scrollSpeed = 0.05f;
    private Renderer waterRenderer;

    void Start()
    {
        this.waterRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        this.waterRenderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
