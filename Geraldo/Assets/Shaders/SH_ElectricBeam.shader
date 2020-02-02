// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_ElectricBeam"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		[HDR]_MainColor("MainColor", Color) = (0,0.8784314,2,0)
		_ScrollSpeed("ScrollSpeed", Float) = 1
		_X_Tiling("X_Tiling", Float) = 1
		_Y_Tiling("Y_Tiling", Float) = 1
		_Width("Width", Range( 5 , 100)) = 5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard alpha:fade keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _MainColor;
		uniform sampler2D _MainTexture;
		uniform float _X_Tiling;
		uniform float _Width;
		uniform float _Y_Tiling;
		uniform float _ScrollSpeed;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Emission = _MainColor.rgb;
			float2 appendResult10 = (float2(( _X_Tiling * _Width ) , _Y_Tiling));
			float2 appendResult6 = (float2(( _Time.y * _ScrollSpeed ) , 0.0));
			float2 uv_TexCoord4 = i.uv_texcoord * appendResult10 + appendResult6;
			float2 appendResult18 = (float2(( _Time.y * ( _ScrollSpeed * -0.77 ) ) , 0.0));
			float2 uv_TexCoord20 = i.uv_texcoord * appendResult10 + appendResult18;
			float blendOpSrc22 = tex2D( _MainTexture, uv_TexCoord4 ).a;
			float blendOpDest22 = tex2D( _MainTexture, uv_TexCoord20 ).a;
			o.Alpha = ( saturate( ( blendOpSrc22 * blendOpDest22 ) ));
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
0;23;1680;954;2265.507;388.9298;1.76585;True;False
Node;AmplifyShaderEditor.RangedFloatNode;7;-1715.046,389.6236;Inherit;False;Property;_ScrollSpeed;ScrollSpeed;2;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-1232.65,657.7715;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-0.77;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;13;-1257.779,557.2642;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1406.242,457.7386;Inherit;False;Property;_Width;Width;5;0;Create;True;0;0;False;0;5;5;5;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1271.932,371.3777;Inherit;False;Property;_X_Tiling;X_Tiling;3;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-1251.116,235.159;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1058.949,502.9055;Inherit;False;Property;_Y_Tiling;Y_Tiling;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1059.912,389.2136;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1038.05,601.6381;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-1034.845,234.5799;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;18;-845.5029,602.8065;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;6;-844.0289,235.7482;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;10;-864.5697,402.0005;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-644.2783,557.4557;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;24;-661.9319,339.6227;Inherit;True;Property;_MainTexture;MainTexture;0;0;Create;True;0;0;False;0;e259abad4269943e4acffa35303922f2;None;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-659.5453,191;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;21;-378.8532,530.2024;Inherit;True;Property;_Tex02;Tex02;1;0;Create;True;0;0;False;0;-1;e259abad4269943e4acffa35303922f2;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;2;-394.1193,163.7467;Inherit;True;Property;_Tex01;Tex01;0;0;Create;True;0;0;False;0;-1;e259abad4269943e4acffa35303922f2;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-26.31537,87.98039;Inherit;False;Property;_MainColor;MainColor;1;1;[HDR];Create;True;0;0;False;0;0,0.8784314,2,0;0,0.8784314,2,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendOpsNode;22;-17.74584,386.4883;Inherit;False;Multiply;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;289.0286,77.08524;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SH_ElectricBeam;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;7;0
WireConnection;29;0;11;0
WireConnection;29;1;28;0
WireConnection;15;0;13;0
WireConnection;15;1;23;0
WireConnection;9;0;8;0
WireConnection;9;1;7;0
WireConnection;18;0;15;0
WireConnection;6;0;9;0
WireConnection;10;0;29;0
WireConnection;10;1;12;0
WireConnection;20;0;10;0
WireConnection;20;1;18;0
WireConnection;4;0;10;0
WireConnection;4;1;6;0
WireConnection;21;0;24;0
WireConnection;21;1;20;0
WireConnection;2;0;24;0
WireConnection;2;1;4;0
WireConnection;22;0;2;4
WireConnection;22;1;21;4
WireConnection;0;2;1;0
WireConnection;0;9;22;0
ASEEND*/
//CHKSM=3F12CCF02B886A5DC25F76D573E95163EEA0A0D2