Shader "Sprite/ShadowCastShader" {
	//可视化参数列表
	Properties{
	//输入的纹理
	_MainTex("Main Tex" , 2D) = "white" {}
	//法线纹理
	_BumpMap("Normal Map", 2D) = "white" {}
	_BumpScale("Bump Scale", float) = 0.5
	//高度纹理
	_HeightMap("Height Map", 2D) = "white" {}
	_HeightScale("Height Scale", float) = 10
	}

	SubShader{
	Tags{ "QUEUE" = "Transparent" "RenderType"="Map" }

	CGINCLUDE
	#include "Lighting.cginc"
	//主纹理
	sampler2D _MainTex;
	float4 _MainTex_ST;
	//法线纹理
	sampler2D _BumpMap;
	float4 _BumpMap_ST;
	float _BumpScale;
	//高度纹理
	sampler2D _HeightMap;
	float _HeightScale;

	struct a2v {
		float4 vertex : POSITION;
		float4 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv :TEXCOORD0;
		float3 posw : TEXCOORD1;
	};

	v2f vert(a2v v) {
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;

		o.posw = mul(unity_ObjectToWorld, v.vertex).xyz;

		return o;
	}

	fixed4 fragS(v2f i) : SV_TARGET{
		//输出参数
		float4 Result = float4(0,0,0,1);
		float alpha = tex2D(_MainTex, i.uv).a;
		//高度值
		float2 sampleuv = float2(i.uv.x , i.uv.y);
		Result.x = (1 - tex2D(_HeightMap, sampleuv).r) * alpha;
		//遮挡物标记
		Result.y = (1 - tex2D(_HeightMap, sampleuv).g) * alpha;
		Result.z = (1 - tex2D(_HeightMap, sampleuv).b) * alpha;

		return float4(Result);
	}

		ENDCG

		Pass {
		Tags{ "LightMode" = "ForwardBase" }

			ZWrite Off
			Cull Off

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment fragS

			ENDCG

	}
	}
		FallBack Off
}
