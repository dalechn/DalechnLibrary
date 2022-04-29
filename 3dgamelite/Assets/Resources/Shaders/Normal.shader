Shader "Normal/Lighting" {

	Properties{
		  _Diffuse("Diffuse Color", Color) = (1,1,1,1)
		_Specular("Specular Color", Color) = (1,1,1,1)
		_Gloss("Gloss", Range(10, 200)) = 20
	}

		SubShader{
			Pass{
				Tags { "LightMode" = "ForwardBase" }

				CGPROGRAM
				#include "Lighting.cginc"
				#pragma vertex vert
				#pragma fragment frag

				fixed4 _Diffuse;
				fixed4 _Specular;
				float _Gloss;

				struct a2v {
					float3 vertex : POSITION;
					float3 normal: NORMAL;
				};

				struct v2f {
					float4 svPos: SV_POSITION;      // 这个是必须的，否则显示不出来
					fixed3 normalizedWorldNormal : COLOR;
					float3 worldPos: TEXCOORD0;     // 顶点世界坐标
				};

				v2f vert(a2v v) {
					v2f f;

					// 将模型空间的顶点坐标转换到裁剪空间
					f.svPos = UnityObjectToClipPos(v.vertex);

					// 将模型空间的法线转换到世界空间，然后标准化，
					//（转换到世界空间是为了后面和灯光做计算）
					f.normalizedWorldNormal = normalize(UnityObjectToWorldNormal(v.normal));

					return f;
				}

				fixed4 frag(v2f f) : SV_TARGET {

					// 1.漫反射
					// 取得光源方向
					float3 normalizedLightDir = normalize(_WorldSpaceLightPos0.xyz);
      
					//fixed dotValue = max(0, dot(normalizedLightDir, f.normalizedWorldNormal)); // lambert
					fixed dotValue = dot(normalizedLightDir, f.normalizedWorldNormal) * 0.5 + 0.5; // half-lambert

					fixed3 diffuse = _LightColor0.rgb * _Diffuse * dotValue;

					 //return fixed4(diffuse, 1);

					//2.高光反射
					// 取得反射光方向 // Phone需要
					fixed3 reflectDir = normalize(reflect(-normalizedLightDir, f.normalizedWorldNormal));
					// 取得视野方向
					fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - f.worldPos);
					 // 视野方向和光源方向的中间量 //Blinn-Phone需要
					fixed3 halfDir = normalize(viewDir + normalizedLightDir);

					//float specularValue = pow(max(dot(reflectDir, viewDir), 0), _Gloss); // Phone
					 float specularValue = pow(max(dot(f.normalizedWorldNormal, halfDir), 0), _Gloss); // Blinn-Phone

					fixed3 specular = _LightColor0.rgb * _Specular * specularValue;

					// 取得环境光
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;

					// 最终颜色
					fixed3 color = specular + diffuse + ambient;

					return fixed4(color, 1);

				}
				// Lambert: 最终颜色 = 直射光颜色 * 漫反射颜色 * max(0, dot(光源方向, 法线方向))
				//	Half-Lambert : 最终颜色 = 直射光颜色 * 漫反射颜色 * (dot(光源方向, 法线方向) * 0.5 + 0.5)
				// Phone: 最终颜色 = 直射光颜色 * 反射光颜色 * max(0, dot(反射光方向, 视野方向)) * 光泽度(gloss) + 漫反射颜色 + 环境光颜色
				//	Blinn-Phone: 最终颜色 = 直射光颜色 * 反射光颜色 * pow(max(0, dot(法线方向, 视野与光线中间向量)), 光泽度(gloss)) + 漫反射颜色 + 环境光颜色

				ENDCG
			}
	}

		Fallback "VertexLit"
}