Shader "Sprite/NormalDiffuse" {

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
		//环境光系数
		_AlbedoScale("albedoScale",float)=0.1
	}

	SubShader{
		Tags{ "QUEUE" = "Transparent" "RenderType"="Map"}

		CGINCLUDE
			#include "Lighting.cginc"
			//主纹理
			sampler2D _MainTex;
			float4 _MainTex_ST;
			//法线纹理
			sampler2D _BumpMap;
			float _BumpScale;
			//高度纹理
			sampler2D _HeightMap;
			float _HeightScale;
			//环境光系数
			float _AlbedoScale;
			//光源
			float _LightsNum = 0;
			float4 _LightsColor_Start[8];
			float4 _LightsPosW_End[8];

			struct a2v {
				float4 vertex : POSITION;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 posw : TEXCOORD1;
			};

			v2f vert(a2v v) {
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				o.posw = mul(unity_ObjectToWorld, v.vertex).xyz;

				return o;
			}

			fixed4 frag(v2f i) : SV_TARGET{
				//光线总合
				float3 ColorSum = float3(0.0f,0.0f,0.0f);
				//纹理颜色值
				float4 albedo = tex2D(_MainTex, i.uv);
				//法线值
				float3 normalDir = tex2D(_BumpMap, i.uv);
				normalDir.xyz = normalDir.xyz * 2 - 1;
				normalDir.xy *= _BumpScale;
				normalDir.z = sqrt(1 - saturate(dot(normalDir.xy, normalDir.xy)));
				normalDir.xyz = normalize(normalDir.xyz);
				//高度值
				float PosZ = (1 - tex2D(_HeightMap, i.uv).r) * _HeightScale;

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

					ColorSum += color;
				}
				return float4(ColorSum * 6+albedo.rgb*_AlbedoScale,albedo.a);
			}

			fixed4 frag2(v2f i) : SV_TARGET{
				//光线总合
				float3 ColorSum = float3(0.0f,0.0f,0.0f);
				//纹理颜色值
				float4 albedo = tex2D(_MainTex, i.uv);
				//法线值
				float3 normalDir = tex2D(_BumpMap, i.uv);
				normalDir.xyz = normalDir.xyz * 2 - 1;
				normalDir.xy *= _BumpScale;
				normalDir.z = sqrt(1 - saturate(dot(normalDir.xy, normalDir.xy)));
				normalDir.xyz = normalize(normalDir.xyz);
				//高度值
				float PosZ = (1 - tex2D(_HeightMap, i.uv).r) * _HeightScale;

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

					ColorSum += color;
				}
				return float4(float3(0,0,0),1.0f);
			}
		ENDCG

		Pass {
			Tags{ "LightMode" = "ForwardBase" }

			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			ENDCG

		}

		/*Pass {
			Tags{ "LightMode" = "ForwardBase"  "QUEUE" = "Geometry" }

			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag2

			ENDCG
		}*/
	}
	FallBack Off
}
