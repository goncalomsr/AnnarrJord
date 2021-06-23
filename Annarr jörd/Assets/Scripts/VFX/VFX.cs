using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    public GameObject fog;
    public GameObject expansiveWave;
    public GameObject transformation;
    public GameObject Ray;
    public GameObject KillRay;
    public float EnableFogTime;
    public float EnableTransformationTime;
    public float EnableRayTime;
    public float EnableKillRayTime;
    public float timer;


    void Start()
    {
        fog.SetActive(false);
        expansiveWave.SetActive(false);
        transformation.SetActive(false);
        Ray.SetActive(false);
        KillRay.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > EnableFogTime)
        {
            fog.SetActive(true);
            expansiveWave.SetActive(true);
        }

        if (timer > EnableTransformationTime)
        {
            transformation.SetActive(true);
        }
        
        if (timer > EnableRayTime)
        {
            Ray.SetActive(true);
        }

        if (timer > EnableKillRayTime)
        {
            KillRay.SetActive(true);
        }
    }
}