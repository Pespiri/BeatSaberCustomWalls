// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "BeatSaber/Tiling/Lit Glow"
{
	Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_Tex ("Texture", 2D) = "white" {}
		_Glow ("Glow", Range (0, 1)) = 0
		_Ambient ("Ambient Lighting", Range (0, 1)) = 0
		_LightDir ("Light Direction", Vector) = (-1,-1,0,1)

		_CustomColors("Use Custom Colors", Range(0, 1)) = 0
		_CustomColorStrength("Custom Color Strength", Float) = 1

		_ScaleMult("Scale Multiplier", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "tiling.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
				float3 normal : NORMAL;
			};

			float4 _Color;
			float _Glow;
			float _Ambient;
			float4 _LightDir;

			float _ScaleMult;
			float _CustomColors;
			float _CustomColorStrength;

			sampler2D _Tex;
			float4 _Tex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = tileUV(v.uv, _ScaleMult, v.normal, unity_ObjectToWorld);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos));
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 lightDir = normalize(_LightDir.xyz) * -1.0;
				float shadow = max(dot(lightDir,i.normal),0);
				// sample the texture
				fixed4 col = tex2D(_Tex, TRANSFORM_TEX(i.uv, _Tex));
				if (_CustomColors == 1) {
					fixed4 customColor = col * _Color;
					col = lerp(col, customColor, _CustomColorStrength);
				}
				col = col * clamp(col * _Ambient + shadow,0.0,1.0);

				return col * float4(1.0,1.0,1.0,_Glow);
			}
			ENDCG
		}
	}
}
