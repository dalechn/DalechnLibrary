// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Micosmo/TriPlanarGridTerrain"
{
	Properties
	{
		_Tiling("Tiling", Range( 0 , 10)) = 0
		_EdgeSize("Edge Size", Range( 0 , 1)) = 0.01
		_EdgeColour("Edge Colour", Color) = (0,0,0,0)
		_CellColour("Cell Colour", Color) = (0,0,0,0)
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
			sampler2D _TerrainHeightmapTexture;//ASE Terrain Instancing
			sampler2D _TerrainNormalmapTexture;//ASE Terrain Instancing
		#endif//ASE Terrain Instancing
		UNITY_INSTANCING_BUFFER_START( Terrain )//ASE Terrain Instancing
			UNITY_DEFINE_INSTANCED_PROP( float4, _TerrainPatchInstanceData )//ASE Terrain Instancing
		UNITY_INSTANCING_BUFFER_END( Terrain)//ASE Terrain Instancing
		CBUFFER_START( UnityTerrain)//ASE Terrain Instancing
			#ifdef UNITY_INSTANCING_ENABLED//ASE Terrain Instancing
				float4 _TerrainHeightmapRecipSize;//ASE Terrain Instancing
				float4 _TerrainHeightmapScale;//ASE Terrain Instancing
			#endif//ASE Terrain Instancing
		CBUFFER_END//ASE Terrain Instancing
		uniform float4 _EdgeColour;
		uniform float4 _CellColour;
		uniform float _Tiling;
		uniform float _EdgeSize;


		void ApplyMeshModification( inout appdata_full v )
		{
			#if defined(UNITY_INSTANCING_ENABLED) && !defined(SHADER_API_D3D11_9X)
				float2 patchVertex = v.vertex.xy;
				float4 instanceData = UNITY_ACCESS_INSTANCED_PROP(Terrain, _TerrainPatchInstanceData);
				
				float4 uvscale = instanceData.z * _TerrainHeightmapRecipSize;
				float4 uvoffset = instanceData.xyxy * uvscale;
				uvoffset.xy += 0.5f * _TerrainHeightmapRecipSize.xy;
				float2 sampleCoords = (patchVertex.xy * uvscale.xy + uvoffset.xy);
				
				float hm = UnpackHeightmap(tex2Dlod(_TerrainHeightmapTexture, float4(sampleCoords, 0, 0)));
				v.vertex.xz = (patchVertex.xy + instanceData.xy) * _TerrainHeightmapScale.xz * instanceData.z;
				v.vertex.y = hm * _TerrainHeightmapScale.y;
				v.vertex.w = 1.0f;
				
				v.texcoord.xy = (patchVertex.xy * uvscale.zw + uvoffset.zw);
				v.texcoord3 = v.texcoord2 = v.texcoord1 = v.texcoord;
				
				#ifdef TERRAIN_INSTANCED_PERPIXEL_NORMAL
					v.normal = float3(0, 1, 0);
					//data.tc.zw = sampleCoords;
				#else
					float3 nor = tex2Dlod(_TerrainNormalmapTexture, float4(sampleCoords, 0, 0)).xyz;
					v.normal = 2.0f * nor - 1.0f;
				#endif
			#endif
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			ApplyMeshModification(v);;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 break8_g113 = ( ase_worldPos * _Tiling );
			float2 appendResult12_g113 = (float2(break8_g113.y , break8_g113.z));
			float2 temp_output_1_0_g114 = appendResult12_g113;
			float2 uv7_g117 = temp_output_1_0_g114;
			float2 w16_g117 = ( max( abs( ddx( uv7_g117 ) ) , abs( ddy( uv7_g117 ) ) ) + 0.001 );
			float2 temp_output_18_0_g117 = ( 0.5 * w16_g117 );
			float2 a23_g117 = ( uv7_g117 + temp_output_18_0_g117 );
			float temp_output_32_0_g113 = _EdgeSize;
			float temp_output_19_0_g114 = temp_output_32_0_g113;
			float size20_g114 = temp_output_19_0_g114;
			float n26_g117 = ( 1.0 / size20_g114 );
			float2 temp_cast_0 = (1.0).xx;
			float2 b24_g117 = ( uv7_g117 - temp_output_18_0_g117 );
			float2 temp_cast_1 = (1.0).xx;
			float2 i47_g117 = ( ( ( floor( a23_g117 ) + min( ( frac( a23_g117 ) * n26_g117 ) , temp_cast_0 ) ) - ( floor( b24_g117 ) + min( ( frac( b24_g117 ) * n26_g117 ) , temp_cast_1 ) ) ) / ( n26_g117 * w16_g117 ) );
			float2 break51_g117 = i47_g117;
			float halfsize22_g114 = ( temp_output_19_0_g114 * 0.5 );
			float2 temp_cast_2 = (( 2.0 * halfsize22_g114 )).xx;
			float2 uv7_g116 = ( ( temp_output_1_0_g114 * 2.0 ) - temp_cast_2 );
			float2 w16_g116 = ( max( abs( ddx( uv7_g116 ) ) , abs( ddy( uv7_g116 ) ) ) + 0.001 );
			float2 temp_output_18_0_g116 = ( 0.5 * w16_g116 );
			float2 a23_g116 = ( uv7_g116 + temp_output_18_0_g116 );
			float n26_g116 = ( 1.0 / size20_g114 );
			float2 temp_cast_3 = (1.0).xx;
			float2 b24_g116 = ( uv7_g116 - temp_output_18_0_g116 );
			float2 temp_cast_4 = (1.0).xx;
			float2 i47_g116 = ( ( ( floor( a23_g116 ) + min( ( frac( a23_g116 ) * n26_g116 ) , temp_cast_3 ) ) - ( floor( b24_g116 ) + min( ( frac( b24_g116 ) * n26_g116 ) , temp_cast_4 ) ) ) / ( n26_g116 * w16_g116 ) );
			float2 break51_g116 = i47_g116;
			float2 temp_cast_5 = (( 4.0 * halfsize22_g114 )).xx;
			float2 uv7_g115 = ( ( temp_output_1_0_g114 * 4.0 ) - temp_cast_5 );
			float2 w16_g115 = ( max( abs( ddx( uv7_g115 ) ) , abs( ddy( uv7_g115 ) ) ) + 0.001 );
			float2 temp_output_18_0_g115 = ( 0.5 * w16_g115 );
			float2 a23_g115 = ( uv7_g115 + temp_output_18_0_g115 );
			float n26_g115 = ( 1.0 / size20_g114 );
			float2 temp_cast_6 = (1.0).xx;
			float2 b24_g115 = ( uv7_g115 - temp_output_18_0_g115 );
			float2 temp_cast_7 = (1.0).xx;
			float2 i47_g115 = ( ( ( floor( a23_g115 ) + min( ( frac( a23_g115 ) * n26_g115 ) , temp_cast_6 ) ) - ( floor( b24_g115 ) + min( ( frac( b24_g115 ) * n26_g115 ) , temp_cast_7 ) ) ) / ( n26_g115 * w16_g115 ) );
			float2 break51_g115 = i47_g115;
			float cyz18_g113 = ( ( ( 1.0 - break51_g117.x ) * ( 1.0 - break51_g117.y ) ) * ( ( 1.0 - break51_g116.x ) * ( 1.0 - break51_g116.y ) ) * ( ( 1.0 - break51_g115.x ) * ( 1.0 - break51_g115.y ) ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 temp_output_5_0_g113 = abs( ase_worldNormal );
			float3 break7_g113 = temp_output_5_0_g113;
			float3 triW15_g113 = ( temp_output_5_0_g113 / ( break7_g113.x + break7_g113.y + break7_g113.z ) );
			float3 break24_g113 = triW15_g113;
			float2 appendResult14_g113 = (float2(break8_g113.x , break8_g113.z));
			float2 temp_output_1_0_g118 = appendResult14_g113;
			float2 uv7_g121 = temp_output_1_0_g118;
			float2 w16_g121 = ( max( abs( ddx( uv7_g121 ) ) , abs( ddy( uv7_g121 ) ) ) + 0.001 );
			float2 temp_output_18_0_g121 = ( 0.5 * w16_g121 );
			float2 a23_g121 = ( uv7_g121 + temp_output_18_0_g121 );
			float temp_output_19_0_g118 = temp_output_32_0_g113;
			float size20_g118 = temp_output_19_0_g118;
			float n26_g121 = ( 1.0 / size20_g118 );
			float2 temp_cast_8 = (1.0).xx;
			float2 b24_g121 = ( uv7_g121 - temp_output_18_0_g121 );
			float2 temp_cast_9 = (1.0).xx;
			float2 i47_g121 = ( ( ( floor( a23_g121 ) + min( ( frac( a23_g121 ) * n26_g121 ) , temp_cast_8 ) ) - ( floor( b24_g121 ) + min( ( frac( b24_g121 ) * n26_g121 ) , temp_cast_9 ) ) ) / ( n26_g121 * w16_g121 ) );
			float2 break51_g121 = i47_g121;
			float halfsize22_g118 = ( temp_output_19_0_g118 * 0.5 );
			float2 temp_cast_10 = (( 2.0 * halfsize22_g118 )).xx;
			float2 uv7_g120 = ( ( temp_output_1_0_g118 * 2.0 ) - temp_cast_10 );
			float2 w16_g120 = ( max( abs( ddx( uv7_g120 ) ) , abs( ddy( uv7_g120 ) ) ) + 0.001 );
			float2 temp_output_18_0_g120 = ( 0.5 * w16_g120 );
			float2 a23_g120 = ( uv7_g120 + temp_output_18_0_g120 );
			float n26_g120 = ( 1.0 / size20_g118 );
			float2 temp_cast_11 = (1.0).xx;
			float2 b24_g120 = ( uv7_g120 - temp_output_18_0_g120 );
			float2 temp_cast_12 = (1.0).xx;
			float2 i47_g120 = ( ( ( floor( a23_g120 ) + min( ( frac( a23_g120 ) * n26_g120 ) , temp_cast_11 ) ) - ( floor( b24_g120 ) + min( ( frac( b24_g120 ) * n26_g120 ) , temp_cast_12 ) ) ) / ( n26_g120 * w16_g120 ) );
			float2 break51_g120 = i47_g120;
			float2 temp_cast_13 = (( 4.0 * halfsize22_g118 )).xx;
			float2 uv7_g119 = ( ( temp_output_1_0_g118 * 4.0 ) - temp_cast_13 );
			float2 w16_g119 = ( max( abs( ddx( uv7_g119 ) ) , abs( ddy( uv7_g119 ) ) ) + 0.001 );
			float2 temp_output_18_0_g119 = ( 0.5 * w16_g119 );
			float2 a23_g119 = ( uv7_g119 + temp_output_18_0_g119 );
			float n26_g119 = ( 1.0 / size20_g118 );
			float2 temp_cast_14 = (1.0).xx;
			float2 b24_g119 = ( uv7_g119 - temp_output_18_0_g119 );
			float2 temp_cast_15 = (1.0).xx;
			float2 i47_g119 = ( ( ( floor( a23_g119 ) + min( ( frac( a23_g119 ) * n26_g119 ) , temp_cast_14 ) ) - ( floor( b24_g119 ) + min( ( frac( b24_g119 ) * n26_g119 ) , temp_cast_15 ) ) ) / ( n26_g119 * w16_g119 ) );
			float2 break51_g119 = i47_g119;
			float cxz16_g113 = ( ( ( 1.0 - break51_g121.x ) * ( 1.0 - break51_g121.y ) ) * ( ( 1.0 - break51_g120.x ) * ( 1.0 - break51_g120.y ) ) * ( ( 1.0 - break51_g119.x ) * ( 1.0 - break51_g119.y ) ) );
			float2 appendResult11_g113 = (float2(break8_g113.x , break8_g113.y));
			float2 temp_output_1_0_g122 = appendResult11_g113;
			float2 uv7_g125 = temp_output_1_0_g122;
			float2 w16_g125 = ( max( abs( ddx( uv7_g125 ) ) , abs( ddy( uv7_g125 ) ) ) + 0.001 );
			float2 temp_output_18_0_g125 = ( 0.5 * w16_g125 );
			float2 a23_g125 = ( uv7_g125 + temp_output_18_0_g125 );
			float temp_output_19_0_g122 = temp_output_32_0_g113;
			float size20_g122 = temp_output_19_0_g122;
			float n26_g125 = ( 1.0 / size20_g122 );
			float2 temp_cast_16 = (1.0).xx;
			float2 b24_g125 = ( uv7_g125 - temp_output_18_0_g125 );
			float2 temp_cast_17 = (1.0).xx;
			float2 i47_g125 = ( ( ( floor( a23_g125 ) + min( ( frac( a23_g125 ) * n26_g125 ) , temp_cast_16 ) ) - ( floor( b24_g125 ) + min( ( frac( b24_g125 ) * n26_g125 ) , temp_cast_17 ) ) ) / ( n26_g125 * w16_g125 ) );
			float2 break51_g125 = i47_g125;
			float halfsize22_g122 = ( temp_output_19_0_g122 * 0.5 );
			float2 temp_cast_18 = (( 2.0 * halfsize22_g122 )).xx;
			float2 uv7_g124 = ( ( temp_output_1_0_g122 * 2.0 ) - temp_cast_18 );
			float2 w16_g124 = ( max( abs( ddx( uv7_g124 ) ) , abs( ddy( uv7_g124 ) ) ) + 0.001 );
			float2 temp_output_18_0_g124 = ( 0.5 * w16_g124 );
			float2 a23_g124 = ( uv7_g124 + temp_output_18_0_g124 );
			float n26_g124 = ( 1.0 / size20_g122 );
			float2 temp_cast_19 = (1.0).xx;
			float2 b24_g124 = ( uv7_g124 - temp_output_18_0_g124 );
			float2 temp_cast_20 = (1.0).xx;
			float2 i47_g124 = ( ( ( floor( a23_g124 ) + min( ( frac( a23_g124 ) * n26_g124 ) , temp_cast_19 ) ) - ( floor( b24_g124 ) + min( ( frac( b24_g124 ) * n26_g124 ) , temp_cast_20 ) ) ) / ( n26_g124 * w16_g124 ) );
			float2 break51_g124 = i47_g124;
			float2 temp_cast_21 = (( 4.0 * halfsize22_g122 )).xx;
			float2 uv7_g123 = ( ( temp_output_1_0_g122 * 4.0 ) - temp_cast_21 );
			float2 w16_g123 = ( max( abs( ddx( uv7_g123 ) ) , abs( ddy( uv7_g123 ) ) ) + 0.001 );
			float2 temp_output_18_0_g123 = ( 0.5 * w16_g123 );
			float2 a23_g123 = ( uv7_g123 + temp_output_18_0_g123 );
			float n26_g123 = ( 1.0 / size20_g122 );
			float2 temp_cast_22 = (1.0).xx;
			float2 b24_g123 = ( uv7_g123 - temp_output_18_0_g123 );
			float2 temp_cast_23 = (1.0).xx;
			float2 i47_g123 = ( ( ( floor( a23_g123 ) + min( ( frac( a23_g123 ) * n26_g123 ) , temp_cast_22 ) ) - ( floor( b24_g123 ) + min( ( frac( b24_g123 ) * n26_g123 ) , temp_cast_23 ) ) ) / ( n26_g123 * w16_g123 ) );
			float2 break51_g123 = i47_g123;
			float cxy17_g113 = ( ( ( 1.0 - break51_g125.x ) * ( 1.0 - break51_g125.y ) ) * ( ( 1.0 - break51_g124.x ) * ( 1.0 - break51_g124.y ) ) * ( ( 1.0 - break51_g123.x ) * ( 1.0 - break51_g123.y ) ) );
			float4 lerpResult5 = lerp( _EdgeColour , _CellColour , ( ( cyz18_g113 * break24_g113.x ) + ( break24_g113.y * cxz16_g113 ) + ( break24_g113.z * cxy17_g113 ) ));
			float4 colour6 = lerpResult5;
			o.Albedo = colour6.rgb;
			o.Metallic = 0.0;
			o.Smoothness = 0.0;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
		UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
1809;73;2029;1447;1983.5;743.5;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;1;-1553.082,126.8923;Inherit;False;Property;_EdgeSize;Edge Size;1;0;Create;True;0;0;0;False;0;False;0.01;0.01;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1553.082,-17.1077;Inherit;False;Property;_Tiling;Tiling;0;0;Create;True;0;0;0;False;0;False;0;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;3;-1213.833,-183.0382;Inherit;False;Property;_CellColour;Cell Colour;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-1213.833,-376.0382;Inherit;False;Property;_EdgeColour;Edge Colour;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;10;-1249.082,46.8923;Inherit;False;TriPlanarPrototypeGrid;-1;;113;cd2c20a2d5548d145aa3540f6eec9f46;0;2;31;FLOAT;1;False;32;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;5;-912.8711,-155.0173;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;6;-719.2825,-158.9078;Inherit;True;colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;7;-259.8184,-2.40509;Inherit;False;6;colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-225.5577,91.36478;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-223.5577,176.3647;Inherit;False;Constant;_Float2;Float 2;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Micosmo/TriPlanarGridTerrain;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;True;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;31;2;0
WireConnection;10;32;1;0
WireConnection;5;0;4;0
WireConnection;5;1;3;0
WireConnection;5;2;10;0
WireConnection;6;0;5;0
WireConnection;0;0;7;0
WireConnection;0;3;8;0
WireConnection;0;4;9;0
ASEEND*/
//CHKSM=1C5BB701631A2FCAA463AE7EACADFBF527F60D5E