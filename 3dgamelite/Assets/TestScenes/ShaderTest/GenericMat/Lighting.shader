// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Normal/Lighting" {

	Properties{
			_MainTex("main tex",2D) = "black"{}

		  _Diffuse("Diffuse Color", Color) = (1,1,1,1)
		_Specular("Specular Color", Color) = (1,1,1,1)
		_Gloss("Gloss", Range(10, 200)) = 20
	}
		//Int Float Vector Range Color 2D 3D Cube Rect
		//float 32位 half 16位 fixed8位

			  SubShader{
				  Pass{
					  //未知: tags都还不太熟悉?
					  Tags { "LightMode" = "ForwardBase" }
					  // Normal                                                     不与光照交互的规则着色器通道
					  // Vertex                                                      旧的顶点照明着色器通道
					  // VertexLM                                                 旧的顶点照明着色器通道，并且带有移动端光照图
					  // ForwardBase                                          前向渲染基本通道
					  // ForwardAdd                                            前向渲染加像素光通道
					  // LightPrePassBase                                 旧延迟光照基础通道
					  // LightPrePassFinal                                  旧延迟光照最终通道
					  // ShadowCaster                                         阴影投射和深度纹理着色器通道
					  // Deffered                                                    延迟着色器通道
					  // Meta                                                           用于产生反照率和发射值的着色器通道，用作光映射的输入。
					  // MotionVectors                                            运动矢量渲染通道
					  // ScriptableRenderPipeline                        自定义脚本通道
					  // ScriptableRenderPipelineDefaultUnlit    当光模式被设置为默认未亮或没有光模式时，设置自定义脚本管道。

  //-------------------------			  
				  //Tags { "RenderType" = "Opaque" }// 暂时未知
					  // Opaque: most of the shaders(Normal, Self Illuminated, Reflective, terrain shaders). 	//用于大多数着色器（法线着色器、自发光着色器、反射着色器以及地形的着色器）。
					  // Transparent : most semitransparent shaders(Transparent, Particle, Font, terrain additive pass shaders). 	//用于半透明着色器（透明着色器、粒子着色器、字体着色器、地形额外通道的着色器）。
					  // TransparentCutout : masked transparency shaders(Transparent Cutout, two pass vegetation shaders).	//蒙皮透明着色器（Transparent Cutout，两个通道的植被着色器）。
					  // Background : Skybox shaders.天空盒着色器。
					  // Overlay : GUITexture, Halo, Flare shaders.光晕着色器、闪光着色器。
					  // TreeOpaque : terrain engine tree bark.地形引擎中的树皮。
					  // TreeTransparentCutout : terrain engine tree leaves.地形引擎中的树叶。
					  // TreeBillboard : terrain engine billboarded trees.地形引擎中的广告牌树。
					  // Grass : terrain engine grass.地形引擎中的草。
					  // GrassBillboard : terrain engine billboarded grass.地形引擎何中的广告牌草。
  //-------------------------
				  //Tags {"Queue" = "Geometry"} 
					  // Background - 背景，一般天空盒之类的使用这个标签，最早被渲染
					  // Geometry(default)-2000  适用于大部分不透明的物体 
					  // AlphaTest -2450 - 如果Shader要使用AlphaTest功能 使用这个队列性能更高
					  // Transparent-3000 - 这个渲染队列在AlphaTest之后，Shader中有用到Alpha Blend的，或者深入不写入的都应该放在这个队列。
					  // Overlay 最后渲染的队列，全屏幕后期的 都应该使用这个

				  //Tags {"IgnoreProjector" = "True"} // 忽略Projector

  //-------------------------
		  //	Cull Off 不剔除 ,Cull Back 剔除背面（背向摄像机的面）,Cull Front 剔除前面 （朝向摄像机的面）默认值是Cull Front
		  // ZWrite 可取值为：On , Off，默认是 On 
		  // ZTest(深度测试) 可取值为：Greater , GEqual , Less , LEqual , Equal , NotEqual , Always , Never , Off，默认是 LEqual，ZTest Off 等同于 ZTest Always

  //-------------------------模板测试 常用于遮罩
		  //Stencil
	  //           {
				   //Ref 1 //设置参考值为1

				   // 数字为 Rendering.CompareFunction enum中的值
					  // Comp Never	//1	无论左边和右边为何值，使测试永远不通过，渲染时总是抛弃
					  // Comp Less	 //2	左边 < 右边时通过测试，渲染时保留，否则抛弃
					  // Comp Equal	//3	左边 = 右边时通过测试，渲染时保留，否则抛弃
					  // Comp LessEqual	//4	左边 <= 右边时通过测试，渲染时保留，否则抛弃
					  // Comp Greater	 //5	左边 > 右边时通过测试，渲染时保留，否则抛弃
					  // Comp NotEqual	//6	左边 != 右边时通过测试，渲染时保留，否则抛弃
					  // Comp GreaterEqual 	//7	左边 >= 右边时通过测试，渲染时保留，否则抛弃
					  // Comp Always	//8	使测试永远通过，渲染时总是保留（是Stencil Comparison的默认值即8）

			   // 数字为Rendering.Rendering.StencilOp enum中的值
				  // Pass Keep	 //0	保持当前buf的值不变
				  // Pass Zero	 //1	不管ref值为何值都将当前buf的值置0
				  // Pass Replace	  //2	将ref值写入buf，即将原来的buf值替换为现在的ref值
				  // Pass IncrementSaturate	//3	不管ref值为何值都将当前buf的值 + 1，超过255不变
				  // Pass DecrementSaturate	//4	不管ref值为何值都将当前buf的值置 - 1，超过0不变
				  // Pass Invert	//5	不管ref值为何值都将当前buf的值按位取反
				  // Pass IncrWrap	//6	不管ref值为何值都将当前buf的值 + 1，若当前值为255增1则变为0 
				  // Pass DecrWrap	//7	不管ref值为何值都将当前buf的值 - 1，若当前值为0减1则变为255 
			   //}

  //-------------------------
		  // LOD 100
		  // 默认函数的LOD
		  // VertexLit     kind of shaders = 100
		  //	Decal,     Reflective VertexLit = 150
		  //	Diffuse		= 200
		  //	Diffuse     Detail, Reflective Bumped Unlit, Reflective Bumped VertexLit = 250
		  //	Bumped,     Specular = 300
		  //	Bumped     Specular = 400
		  //	Parallax		= 500
		  //	Parallax     Specular = 600

  //-------------------------
		  //	finalValue = sourceFactor * sourceValue operation destinationFactor * destinationValue
		  // Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency
		  // Blend One OneMinusSrcAlpha // Premultiplied transparency
		  // Blend One One // Additive
		  // Blend OneMinusDstColor One // Soft additive
		  // Blend DstColor Zero // Multiplicative
		  // Blend DstColor SrcColor // 2x multiplicative


		  // BlendOP Add	混合后的当前颜色 + 缓存颜色
		  // BlendOP Sub	混合后的当前颜色 - 缓存颜色
		  // BlendOP RevSub	缓存颜色 - 混合后的当前颜色
		  // BlendOP Min	min（当前颜色，缓存颜色）	他们之中较小的那个
		  // BlendOP Max	max(当前颜色，缓存颜色)	他们之中较大的那个

				  CGPROGRAM
				  #include "Lighting.cginc"
				  #pragma vertex vert
				  #pragma fragment frag

				  fixed4 _Diffuse;
				  fixed4 _Specular;
				  float _Gloss;

				  sampler2D _MainTex;
				  float4 _MainTex_ST;

				  float4 _RimColor;
				  float _RimPower;
				  fixed _ShowGray;

					  struct v2f {
						  float4 svPos: SV_POSITION;      // 模型最后输出坐标SV表示接下来的管线不再更改这个数据
						  fixed3 normalizedWorldNormal : NORMAL;  //模型世界法线坐标 
						  float4 worldPos: COLOR1;     // 顶点世界坐标
						  float4 uv: TEXCOORD0;     // uv坐标

						  float4 NdotV:COLOR; //计算rim的参数
					  };

					  v2f vert(appdata_base v) {
						  v2f f;

						  // 将模型空间的顶点坐标转换到裁剪空间
						  f.svPos = UnityObjectToClipPos(v.vertex);

						  //f.uv = TRANSFORM_TEX( v.texcoord, _MainTex);
						  //#define TRANSFORM_TEX(tex,name) (tex.xy * name##_ST.xy + name##_ST.zw)
						  //通过TRANSFORM_TEX宏转化纹理坐标需要声明float4 _MainTex_ST;主要处理了Offset和Tiling的改变,值默认时等同于
						  f.uv = v.texcoord;

						  f.worldPos = mul(unity_ObjectToWorld, v.vertex);

						  // 将模型空间的法线转换到世界空间，然后标准化，
						  //（转换到世界空间是为了后面和灯光做计算）
						  f.normalizedWorldNormal = normalize(UnityObjectToWorldNormal(v.normal));

						  return f;
					  }

					  fixed4 frag(v2f f) : SV_TARGET { //模型最后输出坐标颜色

						  // 1.漫反射
						  // 取得光源方向(指向灯光位置)
						  float3 normalizedLightDir = normalize(UnityWorldSpaceLightDir(f.worldPos));

						  //fixed dotValue = max(0, dot(normalizedLightDir, f.normalizedWorldNormal)); // lambert
						  fixed dotValue = dot(normalizedLightDir, f.normalizedWorldNormal) * 0.5 + 0.5; // half-lambert

						  fixed3 diffuse = _LightColor0.rgb * _Diffuse * dotValue;

						  //return fixed4(diffuse, 1);



						 //2.高光反射
						 // 取得反射光方向 // Phone需要
						 fixed3 reflectDir = normalize(reflect(-normalizedLightDir, f.normalizedWorldNormal));
						 // 取得视野方向
						 fixed3 viewDir = normalize(UnityWorldSpaceViewDir(f.worldPos));
						 // 视野方向和光源方向的中间量 //Blinn-Phone需要
						fixed3 halfDir = normalize(viewDir + normalizedLightDir);

						//float specularValue = pow(max(dot(reflectDir, viewDir), 0), _Gloss); // Phone
						 float specularValue = pow(max(dot(f.normalizedWorldNormal, halfDir), 0), _Gloss); // Blinn-Phone

						fixed3 specular = _LightColor0.rgb * _Specular * specularValue;

						// 取得环境光
						fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
						//#define UNITY_LIGHTMODEL_AMBIENT (glstate_lightmodel_ambient * 2)

						// 最终颜色
						fixed3 color = specular + diffuse + ambient;

						//return fixed4(color, 1);

					  fixed4 col = tex2D(_MainTex, f.uv);

					  return col * fixed4(color, 1);

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
// 未知点:V2F中Color ,NORMAL可互换,猜测:v2f中的语义没有严格规定?
// 以SV开头的语义为系统数值语义（system - value semantics），是在DirectX 10中引入的
// SV_POSITION 可用POSITION代替,SV_TARGET 可用COLOR代替

//#define UNITY_MATRIX_P glstate_matrix_projection
//#define UNITY_MATRIX_V unity_MatrixV
//#define UNITY_MATRIX_I_V unity_MatrixInvV
//#define UNITY_MATRIX_VP unity_MatrixVP
//#define UNITY_MATRIX_M unity_ObjectToWorld


//inline float4 UnityObjectToClipPos(in float3 pos)
//{
//	return mul(UNITY_MATRIX_VP, mul(unity_ObjectToWorld, float4(pos, 1.0)));
//}

//inline float4 UnityWorldToClipPos(in float3 pos)
//{
//	return mul(UNITY_MATRIX_VP, float4(pos, 1.0));
//}

//inline float4 UnityViewToClipPos(in float3 pos)
//{
//	return mul(UNITY_MATRIX_P, float4(pos, 1.0));
//}

//inline float3 UnityObjectToViewPos(in float3 pos)
//{
//	return mul(UNITY_MATRIX_V, mul(unity_ObjectToWorld, float4(pos, 1.0))).xyz;
//}

//inline float3 UnityWorldToViewPos(in float3 pos)
//{
//	return mul(UNITY_MATRIX_V, float4(pos, 1.0)).xyz;
//}

//inline float3 UnityObjectToWorldDir(in float3 dir)
//{
//	return normalize(mul((float3x3)unity_ObjectToWorld, dir));
//}

//inline float3 UnityWorldToObjectDir(in float3 dir)
//{
//	return normalize(mul((float3x3)unity_WorldToObject, dir));
//}


//inline float3 UnityObjectToWorldNormal(in float3 norm)
//{
//#ifdef UNITY_ASSUME_UNIFORM_SCALING
//	return UnityObjectToWorldDir(norm);
//#else
//	// mul(IT_M, norm) => mul(norm, I_M) => {dot(norm, I_M.col0), dot(norm, I_M.col1), dot(norm, I_M.col2)}
//	return normalize(mul(norm, (float3x3)unity_WorldToObject));
//#endif
//}

//// Computes world space light direction, from world space position
//inline float3 UnityWorldSpaceLightDir(in float3 worldPos)
//{
//#ifndef USING_LIGHT_MULTI_COMPILE
//	return _WorldSpaceLightPos0.xyz - worldPos * _WorldSpaceLightPos0.w;
//#else
//#ifndef USING_DIRECTIONAL_LIGHT
//	return _WorldSpaceLightPos0.xyz - worldPos;
//#else
//	return _WorldSpaceLightPos0.xyz;
//#endif
//#endif
//}
//

//// Computes object space light direction
//inline float3 ObjSpaceLightDir(in float4 v)
//{
//	float3 objSpaceLightPos = mul(unity_WorldToObject, _WorldSpaceLightPos0).xyz;
//#ifndef USING_LIGHT_MULTI_COMPILE
//	return objSpaceLightPos.xyz - v.xyz * _WorldSpaceLightPos0.w;
//#else
//#ifndef USING_DIRECTIONAL_LIGHT
//	return objSpaceLightPos.xyz - v.xyz;
//#else
//	return objSpaceLightPos.xyz;
//#endif
//#endif
//}
//

//inline float3 UnityWorldSpaceViewDir(in float3 worldPos)
//{
//	return _WorldSpaceCameraPos.xyz - worldPos;
//}
//

//inline float3 ObjSpaceViewDir(in float4 v)
//{
//	float3 objSpaceCameraPos = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1)).xyz;
//	return objSpaceCameraPos - v.xyz;
//}

	//struct appdata_base {
	//	float4 vertex : POSITION; 
	//	float3 normal : NORMAL;
	//	float4 texcoord : TEXCOORD0;
	//	UNITY_VERTEX_INPUT_INSTANCE_ID
	//};

	//struct appdata_tan {
	//	float4 vertex : POSITION;
	//	float4 tangent : TANGENT;
	//	float3 normal : NORMAL;
	//	float4 texcoord : TEXCOORD0;
	//	UNITY_VERTEX_INPUT_INSTANCE_ID
	//};

	//struct appdata_full {
	//	float4 vertex : POSITION;		//模型坐标
	//	float4 tangent : TANGENT;	//顶点切线
	//	float3 normal : NORMAL;		//顶点法线
	//	float4 texcoord : TEXCOORD0;	//0表示第一组纹理坐标
	//	float4 texcoord1 : TEXCOORD1;
	//	float4 texcoord2 : TEXCOORD2;
	//	float4 texcoord3 : TEXCOORD3;
	//	fixed4 color : COLOR;			//顶点颜色,猜测:默认1,1,1,1?
	//	UNITY_VERTEX_INPUT_INSTANCE_ID
	//};

	//struct appdata_img
	//{
	//	float4 vertex : POSITION;
	//	half2 texcoord : TEXCOORD0;
	//	UNITY_VERTEX_INPUT_INSTANCE_ID
	//};

	//struct v2f_img
	//{
	//	float4 pos : SV_POSITION;
	//	half2 uv : TEXCOORD0;
	//	UNITY_VERTEX_INPUT_INSTANCE_ID
	//		UNITY_VERTEX_OUTPUT_STEREO
	//};

	//struct v2f_vertex_lit {
	//	float2 uv   : TEXCOORD0;
	//	fixed4 diff : COLOR0;
	//	fixed4 spec : COLOR1;
	//};

//-------------------------
	// float saturate(float x) {return clamp01(x);}
	// float step(x, y) { return x >= y ? 1 : 0;}

	// clip(_RimColor.rgb- _CutOutValue); //clip函数会将参数小于0的像素点直接丢弃掉
	//函数猜想
	//if ((_RimColor.rgb - _Cutoff) < 0)
	//{
	//	discard;
	//} 

	// half4 c = tex2D(_MainTex, IN.uv); //纹理采样函数
	// UnpackNormal (tex2D (_Normal, IN.uv)); //法线贴图采样
