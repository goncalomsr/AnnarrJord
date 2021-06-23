using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyObject : MonoBehaviour
{
    public Vector4[] positions = new Vector4[100];
    public float radius = 0.5f;
    public float softness = 0.5f;
    public float updateTime = 0.5f;
    private float timer = 0;
    public int positionIndex = 0;
    public bool StartDisapear = false;

    void Update()
    {
        if(StartDisapear == false)
        {
            timer += Time.deltaTime;
            if (timer > updateTime)
            {
                bool updateShader = false;
                timer = 0;
                if (positionIndex == 0)
                {
                    positions[positionIndex] = transform.position;
                    positionIndex++;
                    updateShader = true;
                }
                else if ((positions[positionIndex - 1].x != transform.position.x) ||
                         (positions[positionIndex - 1].y != transform.position.y) ||
                         (positions[positionIndex - 1].z != transform.position.z))
                {
                    positions[positionIndex] = transform.position;
                    positionIndex++;
                    updateShader = true;
                }

                if (positionIndex > positions.Length - 1)
                {
                    positionIndex = 0;
                }
                if (updateShader)
                {
                    Shader.SetGlobalFloat("_MaskRadius", radius);
                    Shader.SetGlobalFloat("_MaskSoftness", softness);
                    Shader.SetGlobalVectorArray("_MaskPositions", positions);
                    Shader.SetGlobalFloat("_MaskPositionCount", positions.Length);
                }

            }
        }
               
    }


}