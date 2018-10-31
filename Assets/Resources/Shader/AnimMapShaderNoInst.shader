/*
Created by jiadong chen
http://www.chenjd.me
*/

Shader "chenjd/AnimMapShaderNoInst"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AnimMap ("AnimMap", 2D) ="white" {}
		[PerRendererData]_AnimLen("Anim Length", Float) = 0
		[PerRendererData]_AnimLoop("Anim Loop", Float) = 0
		[PerRendererData]_AnimStart("Anim Start", Float) = 0
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


			#include "UnityCG.cginc"

			struct appdata
			{
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _AnimMap;
			float4 _AnimMap_TexelSize;//x == 1/width

			float _AnimLen;
			float _AnimLoop;
			float _AnimStart;

			
			v2f vert (appdata v, uint vid : SV_VertexID)
			{
				float f = (_Time.y - _AnimStart) / _AnimLen;

				float looptime = fmod(f, 1.0);

				float clamptime = clamp(f,0,1);

				float AnimLoop = _AnimLoop;

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
