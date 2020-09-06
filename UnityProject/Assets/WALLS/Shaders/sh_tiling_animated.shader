Shader "BeatSaber/Tiling/Unlit Animated Spritesheet"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Tex("Texture", 2D) = "white" {}
		_Glow("Glow", Range(0, 1)) = 0
		_Cols("Cols Count", Int) = 1
		_Rows("Rows Count", Int) = 1
		_Speed("Animation Speed", Float) = 1

		_CustomColors("Use Custom Colors", Range(0, 1)) = 0
		_CustomColorStrength("Custom Color Strength", Float) = 1

		_ScaleMult("Scale Multiplier", Float) = 1.73

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

				float _Cols;
				float _Rows;
				float _Speed;

				v2f vert(appdata v)
				{
					v2f o;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.uv = tileUV(v.uv, _ScaleMult, v.normal, unity_ObjectToWorld);

					return o;
				}

				float2 Flipbook(float time, float nbRows, float nbColumns, float2 baseUV){
					float2 sheetSize = float2(nbColumns,nbRows);
					float2 nbFrames = float2(nbColumns*nbRows,nbRows);

					float2 result = baseUV / sheetSize;

					return result - (floor(nbFrames*frac(time))/sheetSize);
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed2 uv = Flipbook(_Time.y*_Speed,_Rows,_Cols,i.uv%1);
					
					fixed4 col = tex2D(_Tex, uv) * i.color; //* _Color;
					col.a = _Glow;

					return col;
				}
				ENDCG
			}
	}
}
