// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Micosmo/TriPlanarGrid"
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
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _EdgeColour;
		uniform float4 _CellColour;
		uniform float _Tiling;
		uniform float _EdgeSize;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 break8_g100 = ( ase_worldPos * _Tiling );
			float2 appendResult12_g100 = (float2(break8_g100.y , break8_g100.z));
			float2 temp_output_1_0_g101 = appendResult12_g100;
			float2 uv7_g104 = temp_output_1_0_g101;
			float2 w16_g104 = ( max( abs( ddx( uv7_g104 ) ) , abs( ddy( uv7_g104 ) ) ) + 0.001 );
			float2 temp_output_18_0_g104 = ( 0.5 * w16_g104 );
			float2 a23_g104 = ( uv7_g104 + temp_output_18_0_g104 );
			float temp_output_32_0_g100 = _EdgeSize;
			float temp_output_19_0_g101 = temp_output_32_0_g100;
			float size20_g101 = temp_output_19_0_g101;
			float n26_g104 = ( 1.0 / size20_g101 );
			float2 temp_cast_0 = (1.0).xx;
			float2 b24_g104 = ( uv7_g104 - temp_output_18_0_g104 );
			float2 temp_cast_1 = (1.0).xx;
			float2 i47_g104 = ( ( ( floor( a23_g104 ) + min( ( frac( a23_g104 ) * n26_g104 ) , temp_cast_0 ) ) - ( floor( b24_g104 ) + min( ( frac( b24_g104 ) * n26_g104 ) , temp_cast_1 ) ) ) / ( n26_g104 * w16_g104 ) );
			float2 break51_g104 = i47_g104;
			float halfsize22_g101 = ( temp_output_19_0_g101 * 0.5 );
			float2 temp_cast_2 = (( 2.0 * halfsize22_g101 )).xx;
			float2 uv7_g102 = ( ( temp_output_1_0_g101 * 2.0 ) - temp_cast_2 );
			float2 w16_g102 = ( max( abs( ddx( uv7_g102 ) ) , abs( ddy( uv7_g102 ) ) ) + 0.001 );
			float2 temp_output_18_0_g102 = ( 0.5 * w16_g102 );
			float2 a23_g102 = ( uv7_g102 + temp_output_18_0_g102 );
			float n26_g102 = ( 1.0 / size20_g101 );
			float2 temp_cast_3 = (1.0).xx;
			float2 b24_g102 = ( uv7_g102 - temp_output_18_0_g102 );
			float2 temp_cast_4 = (1.0).xx;
			float2 i47_g102 = ( ( ( floor( a23_g102 ) + min( ( frac( a23_g102 ) * n26_g102 ) , temp_cast_3 ) ) - ( floor( b24_g102 ) + min( ( frac( b24_g102 ) * n26_g102 ) , temp_cast_4 ) ) ) / ( n26_g102 * w16_g102 ) );
			float2 break51_g102 = i47_g102;
			float2 temp_cast_5 = (( 4.0 * halfsize22_g101 )).xx;
			float2 uv7_g103 = ( ( temp_output_1_0_g101 * 4.0 ) - temp_cast_5 );
			float2 w16_g103 = ( max( abs( ddx( uv7_g103 ) ) , abs( ddy( uv7_g103 ) ) ) + 0.001 );
			float2 temp_output_18_0_g103 = ( 0.5 * w16_g103 );
			float2 a23_g103 = ( uv7_g103 + temp_output_18_0_g103 );
			float n26_g103 = ( 1.0 / size20_g101 );
			float2 temp_cast_6 = (1.0).xx;
			float2 b24_g103 = ( uv7_g103 - temp_output_18_0_g103 );
			float2 temp_cast_7 = (1.0).xx;
			float2 i47_g103 = ( ( ( floor( a23_g103 ) + min( ( frac( a23_g103 ) * n26_g103 ) , temp_cast_6 ) ) - ( floor( b24_g103 ) + min( ( frac( b24_g103 ) * n26_g103 ) , temp_cast_7 ) ) ) / ( n26_g103 * w16_g103 ) );
			float2 break51_g103 = i47_g103;
			float cyz18_g100 = ( ( ( 1.0 - break51_g104.x ) * ( 1.0 - break51_g104.y ) ) * ( ( 1.0 - break51_g102.x ) * ( 1.0 - break51_g102.y ) ) * ( ( 1.0 - break51_g103.x ) * ( 1.0 - break51_g103.y ) ) );
			float3 ase_worldNormal = i.worldNormal;
			float3 temp_output_5_0_g100 = abs( ase_worldNormal );
			float3 break7_g100 = temp_output_5_0_g100;
			float3 triW15_g100 = ( temp_output_5_0_g100 / ( break7_g100.x + break7_g100.y + break7_g100.z ) );
			float3 break24_g100 = triW15_g100;
			float2 appendResult14_g100 = (float2(break8_g100.x , break8_g100.z));
			float2 temp_output_1_0_g105 = appendResult14_g100;
			float2 uv7_g108 = temp_output_1_0_g105;
			float2 w16_g108 = ( max( abs( ddx( uv7_g108 ) ) , abs( ddy( uv7_g108 ) ) ) + 0.001 );
			float2 temp_output_18_0_g108 = ( 0.5 * w16_g108 );
			float2 a23_g108 = ( uv7_g108 + temp_output_18_0_g108 );
			float temp_output_19_0_g105 = temp_output_32_0_g100;
			float size20_g105 = temp_output_19_0_g105;
			float n26_g108 = ( 1.0 / size20_g105 );
			float2 temp_cast_8 = (1.0).xx;
			float2 b24_g108 = ( uv7_g108 - temp_output_18_0_g108 );
			float2 temp_cast_9 = (1.0).xx;
			float2 i47_g108 = ( ( ( floor( a23_g108 ) + min( ( frac( a23_g108 ) * n26_g108 ) , temp_cast_8 ) ) - ( floor( b24_g108 ) + min( ( frac( b24_g108 ) * n26_g108 ) , temp_cast_9 ) ) ) / ( n26_g108 * w16_g108 ) );
			float2 break51_g108 = i47_g108;
			float halfsize22_g105 = ( temp_output_19_0_g105 * 0.5 );
			float2 temp_cast_10 = (( 2.0 * halfsize22_g105 )).xx;
			float2 uv7_g106 = ( ( temp_output_1_0_g105 * 2.0 ) - temp_cast_10 );
			float2 w16_g106 = ( max( abs( ddx( uv7_g106 ) ) , abs( ddy( uv7_g106 ) ) ) + 0.001 );
			float2 temp_output_18_0_g106 = ( 0.5 * w16_g106 );
			float2 a23_g106 = ( uv7_g106 + temp_output_18_0_g106 );
			float n26_g106 = ( 1.0 / size20_g105 );
			float2 temp_cast_11 = (1.0).xx;
			float2 b24_g106 = ( uv7_g106 - temp_output_18_0_g106 );
			float2 temp_cast_12 = (1.0).xx;
			float2 i47_g106 = ( ( ( floor( a23_g106 ) + min( ( frac( a23_g106 ) * n26_g106 ) , temp_cast_11 ) ) - ( floor( b24_g106 ) + min( ( frac( b24_g106 ) * n26_g106 ) , temp_cast_12 ) ) ) / ( n26_g106 * w16_g106 ) );
			float2 break51_g106 = i47_g106;
			float2 temp_cast_13 = (( 4.0 * halfsize22_g105 )).xx;
			float2 uv7_g107 = ( ( temp_output_1_0_g105 * 4.0 ) - temp_cast_13 );
			float2 w16_g107 = ( max( abs( ddx( uv7_g107 ) ) , abs( ddy( uv7_g107 ) ) ) + 0.001 );
			float2 temp_output_18_0_g107 = ( 0.5 * w16_g107 );
			float2 a23_g107 = ( uv7_g107 + temp_output_18_0_g107 );
			float n26_g107 = ( 1.0 / size20_g105 );
			float2 temp_cast_14 = (1.0).xx;
			float2 b24_g107 = ( uv7_g107 - temp_output_18_0_g107 );
			float2 temp_cast_15 = (1.0).xx;
			float2 i47_g107 = ( ( ( floor( a23_g107 ) + min( ( frac( a23_g107 ) * n26_g107 ) , temp_cast_14 ) ) - ( floor( b24_g107 ) + min( ( frac( b24_g107 ) * n26_g107 ) , temp_cast_15 ) ) ) / ( n26_g107 * w16_g107 ) );
			float2 break51_g107 = i47_g107;
			float cxz16_g100 = ( ( ( 1.0 - break51_g108.x ) * ( 1.0 - break51_g108.y ) ) * ( ( 1.0 - break51_g106.x ) * ( 1.0 - break51_g106.y ) ) * ( ( 1.0 - break51_g107.x ) * ( 1.0 - break51_g107.y ) ) );
			float2 appendResult11_g100 = (float2(break8_g100.x , break8_g100.y));
			float2 temp_output_1_0_g109 = appendResult11_g100;
			float2 uv7_g112 = temp_output_1_0_g109;
			float2 w16_g112 = ( max( abs( ddx( uv7_g112 ) ) , abs( ddy( uv7_g112 ) ) ) + 0.001 );
			float2 temp_output_18_0_g112 = ( 0.5 * w16_g112 );
			float2 a23_g112 = ( uv7_g112 + temp_output_18_0_g112 );
			float temp_output_19_0_g109 = temp_output_32_0_g100;
			float size20_g109 = temp_output_19_0_g109;
			float n26_g112 = ( 1.0 / size20_g109 );
			float2 temp_cast_16 = (1.0).xx;
			float2 b24_g112 = ( uv7_g112 - temp_output_18_0_g112 );
			float2 temp_cast_17 = (1.0).xx;
			float2 i47_g112 = ( ( ( floor( a23_g112 ) + min( ( frac( a23_g112 ) * n26_g112 ) , temp_cast_16 ) ) - ( floor( b24_g112 ) + min( ( frac( b24_g112 ) * n26_g112 ) , temp_cast_17 ) ) ) / ( n26_g112 * w16_g112 ) );
			float2 break51_g112 = i47_g112;
			float halfsize22_g109 = ( temp_output_19_0_g109 * 0.5 );
			float2 temp_cast_18 = (( 2.0 * halfsize22_g109 )).xx;
			float2 uv7_g110 = ( ( temp_output_1_0_g109 * 2.0 ) - temp_cast_18 );
			float2 w16_g110 = ( max( abs( ddx( uv7_g110 ) ) , abs( ddy( uv7_g110 ) ) ) + 0.001 );
			float2 temp_output_18_0_g110 = ( 0.5 * w16_g110 );
			float2 a23_g110 = ( uv7_g110 + temp_output_18_0_g110 );
			float n26_g110 = ( 1.0 / size20_g109 );
			float2 temp_cast_19 = (1.0).xx;
			float2 b24_g110 = ( uv7_g110 - temp_output_18_0_g110 );
			float2 temp_cast_20 = (1.0).xx;
			float2 i47_g110 = ( ( ( floor( a23_g110 ) + min( ( frac( a23_g110 ) * n26_g110 ) , temp_cast_19 ) ) - ( floor( b24_g110 ) + min( ( frac( b24_g110 ) * n26_g110 ) , temp_cast_20 ) ) ) / ( n26_g110 * w16_g110 ) );
			float2 break51_g110 = i47_g110;
			float2 temp_cast_21 = (( 4.0 * halfsize22_g109 )).xx;
			float2 uv7_g111 = ( ( temp_output_1_0_g109 * 4.0 ) - temp_cast_21 );
			float2 w16_g111 = ( max( abs( ddx( uv7_g111 ) ) , abs( ddy( uv7_g111 ) ) ) + 0.001 );
			float2 temp_output_18_0_g111 = ( 0.5 * w16_g111 );
			float2 a23_g111 = ( uv7_g111 + temp_output_18_0_g111 );
			float n26_g111 = ( 1.0 / size20_g109 );
			float2 temp_cast_22 = (1.0).xx;
			float2 b24_g111 = ( uv7_g111 - temp_output_18_0_g111 );
			float2 temp_cast_23 = (1.0).xx;
			float2 i47_g111 = ( ( ( floor( a23_g111 ) + min( ( frac( a23_g111 ) * n26_g111 ) , temp_cast_22 ) ) - ( floor( b24_g111 ) + min( ( frac( b24_g111 ) * n26_g111 ) , temp_cast_23 ) ) ) / ( n26_g111 * w16_g111 ) );
			float2 break51_g111 = i47_g111;
			float cxy17_g100 = ( ( ( 1.0 - break51_g112.x ) * ( 1.0 - break51_g112.y ) ) * ( ( 1.0 - break51_g110.x ) * ( 1.0 - break51_g110.y ) ) * ( ( 1.0 - break51_g111.x ) * ( 1.0 - break51_g111.y ) ) );
			float4 lerpResult204 = lerp( _EdgeColour , _CellColour , ( ( cyz18_g100 * break24_g100.x ) + ( break24_g100.y * cxz16_g100 ) + ( break24_g100.z * cxy17_g100 ) ));
			float4 colour46 = lerpResult204;
			o.Albedo = colour46.rgb;
			o.Metallic = 0.0;
			o.Smoothness = 0.0;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
1809;73;2029;1447;1039.75;309.9305;1;True;False
Node;AmplifyShaderEditor.RangedFloatNode;169;-608,576;Inherit;False;Property;_EdgeSize;Edge Size;1;0;Create;True;0;0;0;False;0;False;0.01;0.01;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-608,432;Inherit;False;Property;_Tiling;Tiling;0;0;Create;True;0;0;0;False;0;False;0;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;202;-304,496;Inherit;False;TriPlanarPrototypeGrid;-1;;100;cd2c20a2d5548d145aa3540f6eec9f46;0;2;31;FLOAT;1;False;32;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;207;-268.7505,266.0695;Inherit;False;Property;_CellColour;Cell Colour;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;206;-268.7505,73.06946;Inherit;False;Property;_EdgeColour;Edge Colour;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;204;32.21132,294.0904;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;225.7999,290.1999;Inherit;True;colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;778.264,458.7026;Inherit;False;46;colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;1;812.5247,552.4725;Inherit;False;Constant;_Float0;Float 0;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;814.5247,637.4724;Inherit;False;Constant;_Float1;Float 1;0;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1011.525,491.9724;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Micosmo/TriPlanarGrid;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;16;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;202;31;158;0
WireConnection;202;32;169;0
WireConnection;204;0;206;0
WireConnection;204;1;207;0
WireConnection;204;2;202;0
WireConnection;46;0;204;0
WireConnection;0;0;47;0
WireConnection;0;3;1;0
WireConnection;0;4;2;0
ASEEND*/
//CHKSM=36F194C1252391424AD8CBD927E7AA3287850590