Shader "Custom/GrayScaleTerrain" {
    Properties{
        [HideInInspector] _MainTex("BaseMap (RGB)", 2D) = "white" {}
        [HideInInspector] _Color("Main Color", Color) = (1,1,1,1)
        [HideInInspector] _TerrainHolesTexture("Holes Map (RGB)", 2D) = "white" {}
    }

        SubShader{
            Tags {
                "Queue" = "Geometry-100"
                "RenderType" = "Opaque"
            }

            CGPROGRAM
            #pragma surface surf Standard vertex:SplatmapVert finalcolor:SplatmapFinalColor finalgbuffer:SplatmapFinalGBuffer addshadow fullforwardshadows
            #pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
            #pragma multi_compile_fog 
            #pragma target 3.0
            #include "UnityPBSLighting.cginc"

            #pragma multi_compile_local __ _ALPHATEST_ON
            #pragma multi_compile_local __ _NORMALMAP
            #define TERRAIN_STANDARD_SHADER
            #define TERRAIN_INSTANCED_PERPIXEL_NORMAL
            #define TERRAIN_SURFACE_OUTPUT SurfaceOutputStandard
            #include "TerrainSplatmapCommon.cginc"

            half _Metallic0;
            half _Metallic1;
            half _Metallic2;
            half _Metallic3;

            half _Smoothness0;
            half _Smoothness1;
            half _Smoothness2;
            half _Smoothness3;

            float3 _MaskPositions[100];
            int _MaskPositionCount;
            half _MaskRadius;
            half _MaskSoftness;

            void surf(Input IN, inout SurfaceOutputStandard o) {
                half4 splat_control;
                half weight;
                fixed4 mixedDiffuse;
                half4 defaultSmoothness = half4(_Smoothness0, _Smoothness1, _Smoothness2, _Smoothness3);
                SplatmapMix(IN, defaultSmoothness, splat_control, weight, mixedDiffuse, o.Normal);
                o.Albedo = mixedDiffuse.rgb;
                for (int i = 0; i < _MaskPositionCount; i++)
                {
                    if (distance(_MaskPositions[i], float3(0, 0, 0)) > 0.001)
                    {
                        half dist = distance(_MaskPositions[i], IN.worldPos);
                        half sphere = 1 - saturate((dist - _MaskRadius) / _MaskSoftness);
                        if (sphere - 0.05 >= 0)
                        {
                            o.Albedo = (mixedDiffuse.r + mixedDiffuse.g + mixedDiffuse.b) / 3;
                        }
                    }
                }
                o.Alpha = weight;
                o.Smoothness = mixedDiffuse.a;
                o.Metallic = dot(splat_control, half4(_Metallic0, _Metallic1, _Metallic2, _Metallic3));
            }
        ENDCG

        UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
        UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
    }

    Dependency "AddPassShader"    = "Hidden/TerrainEngine/Splatmap/Standard-AddPass"
    Dependency "BaseMapShader"    = "Hidden/TerrainEngine/Splatmap/Standard-Base"
    Dependency "BaseMapGenShader" = "Hidden/TerrainEngine/Splatmap/Standard-BaseGen"

    Fallback "Nature/Terrain/Diffuse"
}
