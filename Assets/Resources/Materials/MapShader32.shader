Shader "Sprite/NormalDiffuse32" {

	//可视化参数列表
Properties{
//输入的纹理
_MainTex("Main Tex" , 2D) = "white" {}
//叠加色
_Color("MainColor",Color) = (1,1,1,1)
//法线纹理
_BumpMap("Normal Map", 2D) = "white" {}
_BumpScale("Bump Scale", float) = 0.5
//高度纹理
_HeightMap("Height Map", 2D) = "white" {}
_HeightScale("Height Scale", float) = 10
//阴影纹理
_ShadowMap("Shadow Map", 3D) = "white" {}
//环境明度
_Albedo("Albedo", float) = 0
}

SubShader{
	Tags{ "QUEUE" = "Transparent" "RenderType" = "Map" }

	CGINCLUDE
#include "Lighting.cginc"
//主纹理
sampler2D _MainTex;
float4 _MainTex_ST;
fixed4 _Color;
//法线纹理
sampler2D _BumpMap;
float _BumpScale;
//高度纹理
sampler2D _HeightMap;
float _HeightScale;
//阴影纹理
sampler3D _ShadowMap;
float4x4 _CamMatrix;
float _MainNum;
//光源
float _LightsNum = 0;
float4 _LightsColor_Start[64];
float4 _LightsPosW_End[64];
//线光源
float _LineLightsNum;
float4 _LineStart[64];
float4 _LineEnd[64];
float4 _LineColor[64];

//环境明度
float _Albedo;

struct a2v {
	float4 vertex : POSITION;
	float4 texcoord : TEXCOORD0;
};

struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	float3 posw : TEXCOORD1;
	float2 posv : TEXCOORD2;
};

v2f vert(a2v v) {
	v2f o;
	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
	o.posw = mul(unity_ObjectToWorld, v.vertex).xyz;
	o.posv = ComputeScreenPos(o.pos).xy;

	return o;
}

fixed4 frag(v2f i) : SV_TARGET{
	//计算新的采样坐标
	float2 sampleuv = float2(i.uv.x, i.uv.y);
	//光线总合
	float3 ColorSum = float3(0.0f,0.0f,0.0f);
	//纹理颜色值
	float4 albedo = tex2D(_MainTex, i.uv);
	//法线值
	float3 normalDir = tex2D(_BumpMap, sampleuv);
	normalDir.xyz = normalDir.xyz * 2 - 1;
	normalDir.xy *= _BumpScale;
	normalDir.z = sqrt(1 - saturate(dot(normalDir.xy, normalDir.xy)));
	normalDir.xyz = normalize(normalDir.xyz);
	//高度值
	float PosZ = (1 - tex2D(_HeightMap, sampleuv).r) * _HeightScale;

	//逐个计算点光源
	for (int j = 0; j < _LightsNum; j++)
	{
		//参数读取
		float3 LightPosW = _LightsPosW_End[j].xyz;
		float3 LightColor = _LightsColor_Start[j].xyz;
		float2 LightRange = float2(_LightsColor_Start[j].w, _LightsPosW_End[j].w);

		//计算光照衰减
		float3 PosW = float3(i.posw.x, i.posw.y, PosZ);
		float3 LightP2PosW = LightPosW - PosW;
		float dis = sqrt(dot(LightP2PosW, LightP2PosW));
		LightColor *= saturate((LightRange.y - dis) / (LightRange.y - LightRange.x));

		//计算漫反射系数
		float3 LightDir = normalize(LightP2PosW);
		float3 color = albedo.rgb * LightColor * saturate(dot(LightDir, normalDir));

		//在阴影贴图上采样
		float s = step(_MainNum, j); //如果超出范围的话
		float4 texCoordShadow = mul(_CamMatrix, float4(i.posw,1));
		float shadow = max(s,tex3D(_ShadowMap, float3((texCoordShadow.x + 1.0f) * 0.5f, (texCoordShadow.y + 1.0f) * 0.5f, (j+0.5f)/8.0f)));

		ColorSum += shadow * color;
	}

	//逐个计算线光源
	for (int n = 0; n < _LineLightsNum; n++)
	{
		//参数读取
		float4 pos1 = _LineStart[n];
		float4 pos2 = _LineEnd[n];
		float4 colorline = _LineColor[n];

		//计算这个点到pos1和pos2的距离
		float dist1 = distance(pos1.xyz, i.posw);
		float dist2 = distance(pos2.xyz, i.posw);

		//计算这个点到pos1和pos2的向量
		float3 vec1 = normalize(pos1.xyz - i.posw);
		float3 vec2 = normalize(pos2.xyz - i.posw);
		//计算直线方程
		float3 vecline = normalize(pos1.xyz - pos2.xyz);

		//利用余弦角计算垂点到pos1和pos2的距离
		float x2 = dist2 * dot(vec2, vecline);
		float x1 = dist1 * dot(vec1, vecline);
		float xline2 = sqrt(pow(dist2, 2) - pow(x2, 2));

		//定积分获取该点的光源强度
		float Lsame = abs((log(x2 + dist2) - log(x1 + dist1))) / xline2;

		ColorSum += Lsame * albedo.rgb * colorline.rgb / 10.0f;
	}

	float3 result = max(ColorSum * 6, float3(0, 0, 0)) * _Color.rgb;// +albedo.rgb * _Albedo;
	return float4(result,albedo.a);
}
ENDCG

Pass {
	Tags{ "LightMode" = "ForwardBase"  "QUEUE" = "Geometry" }

		ZWrite Off
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

		ENDCG
}
}
FallBack Off
}

