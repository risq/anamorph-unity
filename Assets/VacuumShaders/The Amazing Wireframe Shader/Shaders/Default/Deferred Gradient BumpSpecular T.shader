// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/The Amazing Wireframe/Deferred/Gradient/Transparent Bumped Specular" 
{
	Properties 
	{		
		//Tag
		[Tag]
		V_WIRE_TAG("", float) = 0

		[Title]
		_V_WIRE_Title_DO("Default Options", float) = 0

		_Color("Main Color (RGB)", color) = (1, 1, 1, 1)
		_SpecColor ("Specular Color (RGB)", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_MainTex("Base (RGB)", 2D) = "white"{}

		_BumpMap ("Normalmap", 2D) = "bump" {}



		[HideInInspector]
		_V_WIRE_Color("Wire Color (RGB) Trans (A)", color) = (0, 0, 0, 1)
		[HideInInspector]	
		_V_WIRE_Size("Wire Size", Range(0, 0.5)) = 0.05

		[HideInInspector]
		_V_WIRE_Transparent_Tex("", 2D) = ""{}

		[HideInInspector]
		_V_WIRE_IBL_Intensity("", float) = 1
		[HideInInspector] 
		_V_WIRE_IBL_Contrast("", float) = 1 
		[HideInInspector]
		_V_WIRE_IBL_Cube("", cube) = ""{}

		[HideInInspector]   
		_V_WIRE_Fresnel_Bias("Fresnel Bias", Range(1, 0)) = 0
		 
		[HideInInspector]   
		_V_WIRE_GradientMin("", float) = -0.1
		[HideInInspector]   
		_V_WIRE_GradientMax("", float) = 0.1
		[HideInInspector]   
		_V_WIRE_GradientOffset("", float) = 0
		[HideInInspector]   
		_V_WIRE_GradientColor("", color) = (1, 1, 1, 1)	
		
	}
	 
	SubShader 
	{ 
		Tags { "Queue"="Transparent+10" 
		       "IgnoreProjector"="True" 
			   "RenderType"="Transparent" 
			   "WireframeTag"="Deferred/Gradient/Transparent Bumped Specular"
			   "WireframeBakedTag"=""
			 }
		LOD 200 


		Pass 
		{
			ZWrite On
			ColorMask 0
		}
		
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert finalcolor:wireColor alpha

		 
		#pragma multi_compile V_WIRE_ANTIALIASING_OFF V_WIRE_ANTIALIASING_ON		
		#pragma multi_compile V_WIRE_FRESNEL_OFF V_WIRE_FRESNEL_ON

		#pragma multi_compile V_WIRE_LIGHT_OFF V_WIRE_LIGHT_ON
		#pragma multi_compile V_WIRE_IBL_OFF V_WIRE_IBL_ON

		#pragma multi_compile V_WIRE_GRADIENT_PREVIEW_OFF V_WIRE_GRADIENT_PREVIEW_ON
		#pragma multi_compile V_WIRE_GRADIENT_AXIS_X V_WIRE_GRADIENT_AXIS_Y V_WIRE_GRADIENT_AXIS_Z
		#pragma multi_compile V_WIRE_GRADIENT_SPACE_LOCAL V_WIRE_GRADIENT_SPACE_WORLD

		#ifdef V_WIRE_ANTIALIASING_ON
			#pragma target 3.0
			#pragma glsl 
		#endif
		 

	    #define V_WIRE_HAS_TEXTURE
		#define V_WIRE_GRADIENT
		#define V_WIRE_GRADIENT_TRANSPARENT

		#define V_WIRE_SURFACE

		#define V_WIRE_BUMP
		#define V_WIRE_BUMP_NO_UV
		#define V_WIRE_GLOSS
		
		#ifdef V_WIRE_IBL_ON
			#define V_WIRE_TANGETNROT
		#endif
		  
				
		#include "../cginc/Wire.cginc"

		ENDCG
	} 
	
	Fallback "Transparent/VertexLit"

	CustomEditor "TheAmazingWireframeMaterial_Editor"
}
