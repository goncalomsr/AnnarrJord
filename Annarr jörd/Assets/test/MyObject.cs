using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyObject : MonoBehaviour
{
    private Vector4[] positions = new Vector4[100];
    public float radius = 0.5f;
    public float softness = 0.5f;
    public float updateTime = 0.5f;
    private float timer = 0;
    private int positionIndex = 0;

    void Update()
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



            /*Collider[] colliders = Physics.OverlapSphere(transform.position, (radius * 2));            
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    Terrain terrain = collider.gameObject.GetComponent<Terrain>();
                    if (terrain)
                    {
                        if ((updateShader) && (terrain))
                        {
                            MaterialPropertyBlock terrainMPB = new MaterialPropertyBlock();
                            terrain.GetSplatMaterialPropertyBlock(terrainMPB);
                            terrainMPB.SetFloat("_MaskRadius", radius);
                            terrainMPB.SetFloat("_MaskSoftness", softness);
                            terrainMPB.SetVectorArray("_MaskPositions", positions);
                            terrainMPB.SetFloat("_MaskPositionCount", positions.Length);
                            terrain.SetSplatMaterialPropertyBlock(terrainMPB);
                        }
                    }
                    else
                    {
                        Material material = collider.gameObject.GetComponent<Renderer>().material;
                        if (material) 
                        {
                            if (material.shader.name == "Custom/GrayScaleNormal")
                            {
                                material.SetFloat("_MaskRadius", radius);
                                material.SetFloat("_MaskSoftness", softness);
                                material.SetVectorArray("_MaskPositions", positions);
                                material.SetFloat("_MaskPositionCount", positions.Length);
                            }
                        }
                    }
                }
                
            }*/

        }
               
    }
}