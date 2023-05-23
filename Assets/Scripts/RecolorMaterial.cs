using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolorMaterial : MonoBehaviour
{
    //material to recolor
    public Material recolorMaterial;
    public float minHue = 0.0f;
    public float maxHue = 0.5f;
    public float minSaturation = 0.25f;
    public float maxSaturation = 0.75f;
    public float minValue = 0.25f;
    public float maxValue = 0.5f;

    void Start()
    {
        //randomize color
        if (recolorMaterial != null)
        {
            Color color = Random.ColorHSV(minHue, maxHue, minSaturation, maxSaturation, minValue, maxValue); //randomly pick a new for color
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) //apply it to every match with the recolor material
            {
                if (renderer.sharedMaterial == this.recolorMaterial)
                {
                    renderer.material.color = color;
                }
            }
        }
    }
}
