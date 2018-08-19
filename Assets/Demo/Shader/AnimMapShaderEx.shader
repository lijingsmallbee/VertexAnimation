/*
Created by jiadong chen
http://www.chenjd.me
*/

Shader "chenjd/AnimMapShaderEx"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AnimMap ("AnimMap", 2D) ="white" {}
		_AnimLen("Anim Length", Float) = 0
		_AnimLoop("Anim Loop", Float) = 0
		_AnimStart("Anim Start", Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100
			Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//开启gpu instancing
			#pragma multi_compile_instancing


			#include "UnityCG.cginc"

			struct appdata
			{
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _AnimMap;
			float4 _AnimMap_TexelSize;//x == 1/width

		//	float _AnimLen;
		//	float _AnimLoop;
		//	float _AnimStart;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float, _AnimStart)
			UNITY_INSTANCING_BUFFER_END(Props)

			UNITY_INSTANCING_BUFFER_START(Props1)
			UNITY_DEFINE_INSTANCED_PROP(float,_AnimLoop)
			UNITY_INSTANCING_BUFFER_END(Props1)

			UNITY_INSTANCING_BUFFER_START(Props2)
			UNITY_DEFINE_INSTANCED_PROP(float,_AnimLen)
			UNITY_INSTANCING_BUFFER_END(Props2)

			
			v2f vert (appdata v, uint vid : SV_VertexID)
			{
				UNITY_SETUP_INSTANCE_ID(v);

				float f = (_Time.y - UNITY_ACCESS_INSTANCED_PROP(Props,_AnimStart)) / UNITY_ACCESS_INSTANCED_PROP(Props2,_AnimLen);

				float looptime = fmod(f, 1.0);

				float clamptime = clamp(f,0,1);

				float AnimLoop = UNITY_ACCESS_INSTANCED_PROP(Props1,_AnimLoop);

				float time = looptime * AnimLoop + (1-AnimLoop)*clamptime;

				float animMap_x = (vid + 0.5) * _AnimMap_TexelSize.x;
				float animMap_y = time;

				float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));

				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.vertex = UnityObjectToClipPos(pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
