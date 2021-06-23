using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxHandler : MonoBehaviour
{
    public Color colorToUse;

    public float duration;
    public float step;
    public float triggerTime;

    public Material skyboxS;

    private void Start()
    {
        skyboxS.SetColor("_SkyTint", Color.white);
    }
    
    private void Update()
    {
        if (Time.time > triggerTime)
        {
            step += Time.deltaTime / duration;
            colorToUse = Color.Lerp(Color.white, Color.grey, step);
            skyboxS.SetColor("_SkyTint", colorToUse);
        }
        
    }
}
