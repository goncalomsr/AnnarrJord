Shader "Custom/GrayScaleNormal" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _NoiseSize("Noise size", float) = 1
    }
        SubShader{
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows alpha:fade
            #pragma target 3.0

            sampler2D _MainTex;

            struct Input {
                float2 uv_MainTex;
                float3 worldPos;
            };

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;

            float _NoiseSize;

            float3 _MaskPositions[100];
            int _MaskPositionCount;
            half _MaskRadius;
            half _MaskSoftness;                   

            void surf(Input IN, inout SurfaceOutputStandard o) {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;             
                o.Albedo = c.rgb;
                for (int i = 0; i < _MaskPositionCount; i++)
                {
                    if (distance(_MaskPositions[i], float3(0, 0, 0)) > 0.001)
                    {
                        half dist = distance(_MaskPositions[i], IN.worldPos);
                        half sphere = 1 - saturate((dist - _MaskRadius) / _MaskSoftness);
                        if (sphere - 0.05 >= 0)
                        {
                            o.Albedo = (c.r + c.g + c.b) / 3;
                        }
                    }
                }                
                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}