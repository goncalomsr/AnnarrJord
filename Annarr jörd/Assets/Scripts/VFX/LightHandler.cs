using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHandler : MonoBehaviour
{
    public Color SunLight = new Color32(255, 244, 214, 1);
    public Color lightToUse = new Color32(160, 160, 160, 1);

    public float duration;
    public float step;
    public float triggerTime;
    public float intensityLvl;
    public float intensityEnd;

    public Light light1;

    private void Start()
    {
        light1.GetComponent<Light>();
    }

    private void Update()
    {
        if (Time.time > triggerTime)
        {
            step += Time.deltaTime / duration;
            light1.color = Color.Lerp(SunLight, lightToUse, step);
            light1.intensity = Mathf.Lerp(intensityLvl, intensityEnd, step);
        }

    }
}
