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
    public GameObject Aura;
    public float EnableFogTime;
    public float EnableTransformationTime;
    public float EnableRayTime;
    public float EnableKillRayTime;
    public float EnableAura;
    public float timer;
    public bool secondEnding;

    void Start()
    {
        fog.SetActive(false);
        expansiveWave.SetActive(false);
        transformation.SetActive(false);
        Ray.SetActive(false);
        KillRay.SetActive(false);
        Aura.SetActive(false);
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

        if (secondEnding == false)
        {
            if (timer > EnableKillRayTime)
            {
                KillRay.SetActive(true);
            }
        }
        else if (secondEnding == true)
        {
            if (timer > EnableAura)
            {
                Aura.SetActive(true);
            }
        }
    }
}