Shader "Sprite/MonsterShander" {
	Properties {
		_MainTex ("Albedo", 2D) = "white" {}
		_NoiseTex("Noise", 2D) = "white" {}
		_Dam("Damage", float) = 1
	}
	SubShader {
			Tags{ "QUEUE" = "Transparent" "RenderType" = "Map" }

			CGINCLUDE
		#include "Lighting.cginc"
		//主纹理
		sampler2D _MainTex;
		float4 _MainTex_ST;
		//噪声
		sampler2D _NoiseTex;
		float4 _NoiseTex_ST;
		float _Dam;

		struct a2v {
			float4 vertex : POSITION;
			float4 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float4 uv :TEXCOORD0;
			float3 posw : TEXCOORD1;
		};

		v2f vert(a2v v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			o.uv.zw = v.texcoord.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw;
			o.posw = mul(unity_ObjectToWorld, v.vertex).xyz;

			return o;
		}

		fixed4 fragS(v2f i) : SV_TARGET{
			//采样底色
			float4 c = tex2D(_MainTex, i.uv.xy);
			//采样噪声纹理
			float noise = tex2D(_NoiseTex, i.uv.zw).r;
			//计算差值
			float light = step(noise, _Dam);
			//混合
			float3 cxa = (1 - light) * c + float3(1, 1, 1)  * light;

			return float4(cxa,c.a);
		}

			ENDCG

			Pass {
			Tags{ "LightMode" = "ForwardBase" }

				ZWrite Off
				Cull Off
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM

					#pragma vertex vert
					#pragma fragment fragS

				ENDCG

		}
		}
			FallBack Off
}
