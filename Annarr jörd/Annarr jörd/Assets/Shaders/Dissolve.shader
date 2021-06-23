Shader "MyShaders/Dissolve"{
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise Texture", 2D) = "gray" {}
		_DissolveThreshold("Dissolve Threshold", Range(0, 1)) = 1
	}

		SubShader{
			Pass{
				CGPROGRAM
				#pragma vertex MyVertexProgram
				#pragma fragment MyFragmetProgram
				#include "UnityCG.cginc"

				struct VextexData {
					float4 position : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct VetexToFragment {
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
					float2 uvNoise : TEXCOORD1;
				};

				float4 _Color;
				sampler2D _MainTex, _NoiseTex;
				float4 _MainTex_ST, _NoiseTex_ST;
				float _DissolveThreshold;

				VetexToFragment MyVertexProgram(VextexData vertex)
				{
					VetexToFragment v2f;
					v2f.position = UnityObjectToClipPos(vertex.position);
					v2f.uv = vertex.uv * _MainTex_ST.xy + _MainTex_ST.zw;
					v2f.uvNoise = vertex.uv * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
					return v2f;
				}

				float4 MyFragmetProgram(VetexToFragment v2f) : SV_TARGET
				{
					float4 colorValue = tex2D(_MainTex, v2f.uv);
					float4 noiseColorValue = tex2D(_NoiseTex, v2f.uvNoise);
					clip(noiseColorValue.rgb - _DissolveThreshold);
					return colorValue * _Color;
				}

				ENDCG
			}
		}
}
