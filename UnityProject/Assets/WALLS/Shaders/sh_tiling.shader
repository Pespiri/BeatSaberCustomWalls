Shader "BeatSaber/Tiling/Unlit Glow"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Tex("Texture", 2D) = "white" {}
		_Glow("Glow", Range(0, 1)) = 0

		_CustomColors("Use Custom Colors", Range(0, 1)) = 0
		_CustomColorStrength("Custom Color Strength", Float) = 1

		_ScaleMult("Scale Multiplier", Float) = 1
	}
		SubShader
		{
			Tags{ "RenderType" = "Opaque" }
			LOD 100

			Pass
				{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#include "tiling.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					uint vid : SV_VertexID;
					float3 normal : NORMAL;
					fixed4 color : COLOR;
					float2 uv : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
					half4 color : COLOR;
				};

				float4 _Color;
				float _Glow;

				sampler2D _Tex;
				float4 _Tex_ST;
				float _ScaleMult;
				float _CustomColors;
				float _CustomColorStrength;
				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.uv = tileUV(v.uv, _ScaleMult, v.normal, unity_ObjectToWorld);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_Tex, i.uv) * i.color;
					if (_CustomColors == 1) {
						fixed4 customColor = col * _Color;
						col = lerp(col, customColor, _CustomColorStrength);
					}
					col.a = _Glow;

					return col;
				}
				ENDCG
			}
	}
}
