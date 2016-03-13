// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/The Amazing Wireframe/Deferred/Reflective/Diffuse" 
{ 
	Properties 
	{
		//Tag
		[Tag]
		V_WIRE_TAG("", float) = 0

		[Title]
		_V_WIRE_Title_DO("Default Options", float) = 0

		_Color("Main Color (RGB)", color) = (1, 1, 1, 1)
		_MainTex("Base (RGB) RefStrenth (A)", 2D) = "white"{}

		_ReflectColor ("Reflection Color (RGB)", Color) = (1, 1, 1, 0.5)
		_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }


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
		_V_WIRE_Fresnel_Reflection_Bias("", Range(1, 0)) = 1 
	}
	   
	SubShader 
	{
		Tags { "RenderType"="Opaque"   
		       "WireframeTag"="Deferred/Reflective/Diffuse"
			   "WireframeBakedTag"=""
			 }
		LOD 200 
		 
		CGPROGRAM
		#pragma surface surf Lambert finalcolor:wireColor

		#pragma multi_compile V_WIRE_ANTIALIASING_OFF V_WIRE_ANTIALIASING_ON
		#pragma multi_compile V_WIRE_FRESNEL_OFF V_WIRE_FRESNEL_ON
		#pragma multi_compile V_WIRE_LIGHT_OFF V_WIRE_LIGHT_ON
		#pragma multi_compile V_WIRE_IBL_OFF V_WIRE_IBL_ON
		#pragma multi_compile V_WIRE_TRANSPARENCY_OFF V_WIRE_TRANSPARENCY_BASE_TEXTURE V_WIRE_TRANSPARENCY_BASE_TEXTURE_INVERT V_WIRE_TRANSPARENCY_CUSTOM_TEXTURE
		#pragma multi_compile V_WIRE_FRESNEL_REFLECTION_OFF V_WIRE_FRESNEL_REFLECTION_ON


		#ifdef V_WIRE_ANTIALIASING_ON
			#pragma target 3.0
			#pragma glsl
		#endif
		 
		#define V_WIRE_REFLECTION 
		#define V_WIRE_REFLECTION_COLOR
		#define V_WIRE_SURFACE
		
		
		#include "../cginc/Wire.cginc"

		ENDCG
	} 

	FallBack "Reflective/VertexLit"

	CustomEditor "TheAmazingWireframeMaterial_Editor"
}
