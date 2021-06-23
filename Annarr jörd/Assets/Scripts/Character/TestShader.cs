using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShader : MonoBehaviour
{
    public Renderer rdr;
    public float triggerTime;
    private float initialDissolve = 0;

    void Start()
    {
        rdr = GetComponent<Renderer>();
    }

    void Update()
    {
        
        if (Time.time > 135f)
        {
            rdr.material.SetFloat("_DissolveThreshold", 0.2f * initialDissolve);
            initialDissolve += 0.01f;
            //rdr.material.SetFloat("_ExtrusionAmount", 0.00001f * Time.time);
        }
    }
}
