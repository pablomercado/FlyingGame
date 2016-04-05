// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/Transparent-UV-Alpha-Masked" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Color ("Color", Color) = (1,1,1,1)
	_MC("Mask Channel", Range(0, 3)) = 0
	//_USpeed ("U speed", Float) = 1
	//_TexNum ("Border Texture Number", Float) = 0
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	//Blend DstColor OneMinusSrcAlpha
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
				//UNITY_FOG_COORDS(1)
			};

			sampler2D _MainTex;
			half4 _MainTex_ST;
			fixed4 _Color;
			int _MC;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				//v.vertex.y += cos(_Time.z + (v.texcoord.x + v.texcoord.y/15) );
				v.vertex.y += sin((_Time.y + v.texcoord.x / 2) * 10) /10;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 msk = tex2D(_MainTex, i.texcoord);
				//fixed4 col = new Color();
				//col.rgb = _Color.rgb;
				fixed4 col = _Color;
				col.a = msk[_MC] * _Color.a;
				//col.rgb = _Color.rgb * col.g * i.color.a;
				//col.a *= i.color.a;
				return col;

			}
		ENDCG
	}
}

}
