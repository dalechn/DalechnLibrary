// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Normal/Rim"
{
	Properties
	{
		_MainTex("main tex",2D) = "black"{}
		_RimColor("rim color",Color) = (1,1,1,1)//边缘颜色
		_RimPower("rim power",range(1,10)) = 2//边缘强度
					[Toggle]_ShowGray("show gray", Range(0,1)) = 0 //是否置灰

		_OutlineCol("OutlineCol", Color) = (1,0,0,1)
		_OutlineFactor("OutlineFactor", Range(0,1)) = 0.1

	}

		SubShader
		{
			//outline使用两个Pass，第一个pass沿法线挤出一点，只输出描边的颜色  
		Pass
		{
			//剔除正面，只渲染背面，对于大多数模型适用，不过如果需要背面的，就有问题了  
			Cull Front
			CGPROGRAM

			#pragma vertex vert  
			#pragma fragment frag  
			#include "UnityCG.cginc"  

			fixed4 _OutlineCol;
			float _OutlineFactor;

			struct v2f
			{
				float4 pos : SV_POSITION;
			};

			 v2f vert(appdata_full v)
			{
				v2f o;

				//在vertex阶段，每个顶点按照法线的方向偏移一部分，不过这种会造成近大远小的透视问题  
				//v.vertex.xyz += v.normal * _OutlineFactor;  

				o.pos = UnityObjectToClipPos(v.vertex);

				//将法线方向转换到视空间  
				float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);

				//将视空间法线xy坐标转化到投影空间，只有xy需要，z深度不需要了  
				float2 offset = TransformViewToProjection(vnormal.xy);

				//在最终投影阶段输出进行偏移操作  
				o.pos.xy += offset * _OutlineFactor;

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//这个Pass直接输出描边颜色  
				return _OutlineCol;
			}

			ENDCG

			}

			// 正常pass
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include"UnityCG.cginc"

				struct v2f
				{
					float4 vertex:SV_POSITION;
					float4 uv:TEXCOORD0;
					float4 NdotV:COLOR;
				};

				sampler2D _MainTex;
				float4 _RimColor;
				float _RimPower;
				fixed _ShowGray;

				//float4 vert(float4 vertex:POSITION) :SV_POSITION
				//{
				//	return UnityObjectToClipPos(vertex);
				//}

					//float4 frag() : SV_TARGET
					//	{
					//		return _RimColor;
					//	}

					void vert(in appdata_base v,out v2f o)
					{
						//v2f o;
						o.vertex = UnityObjectToClipPos(v.vertex);
						o.uv = v.texcoord;

						// 计算rim需要用到
						float3 V = WorldSpaceViewDir(v.vertex);
						V = mul(unity_WorldToObject,V);//视方向从世界到模型坐标系的转换
						o.NdotV.x = saturate(dot(v.normal,normalize(V)));//必须在同一坐标系才能正确做点乘运算

						//return o;
					}

					//v2f vert(appdata_base v)
					//{
					//	v2f o;
					//	o.vertex = UnityObjectToClipPos(v.vertex);
					//	o.uv = v.texcoord;

					//	// 计算rim需要用到
					//	float3 V = WorldSpaceViewDir(v.vertex);
					//	V = mul(unity_WorldToObject,V);//视方向从世界到模型坐标系的转换
					//	o.NdotV.x = saturate(dot(v.normal,normalize(V)));//必须在同一坐标系才能正确做点乘运算

					//	return o;
					//}

					// rim/gray/流动uv
					half4 frag(v2f IN) :SV_TARGET
					{

						//float4 _Time; // (t/20, t, t*2, t*3)
						//float4 _SinTime; // sin(t/8), sin(t/4), sin(t/2), sin(t)
						//float4 _CosTime; // cos(t/8), cos(t/4), cos(t/2), cos(t)
						//float4 unity_DeltaTime; // dt, 1/dt, smoothdt, 1/smoothdt
							// 流动uv
							float2 tmpUV = IN.uv;
							//tmpUV.x += _Time.x;
							tmpUV.y += _Time.y;
							fixed4 c = tex2D(_MainTex, tmpUV);

							//rim
							//用视方向和法线方向做点乘，越边缘的地方，法线和视方向越接近90度，点乘越接近0.
							//用（1- 上面点乘的结果）*颜色，来反映边缘颜色情况
							c.rgb += pow((1 - IN.NdotV.x) ,_RimPower)* _RimColor.rgb;

							//gray
							fixed gray = dot(c.rgb, float3(0.298912, 0.586611, 0.114478));
							c.rgb = lerp(c.rgb, fixed3(gray, gray, gray), _ShowGray);

							return c;
						}

						ENDCG
					}
		}
			FallBack "Diffuse"
}