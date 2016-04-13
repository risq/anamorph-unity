// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/The Amazing Wireframe/One Directional Light/Reflective"
{
    Properties 
    {
		//Tag
		[Tag]
		V_WIRE_TAG("", float) = 0

		[Title]
		_V_WIRE_Title_DO("Default Options", float) = 0

		_Color("Main Color", color) = (1, 1, 1, 1)
		_MainTex("Base (RGB) ReflStrength (A)", 2D) = "white"{}

		_ReflectColor("Reflection Color (RGB)", color) = (1, 1, 1, 1)
		_Cube("Reflection Cube", CUBE) = ""{}



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
		Tags { "Queue"="Geometry" 
		       "RenderType"="Opaque" 
			   "WireframeTag"="One Directional Light/Reflective"
			   "WireframeBakedTag"=""
			 }

		Pass 
	    {			
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" } 

            CGPROGRAM
		    #pragma vertex vert  
	    	#pragma fragment frag
	    	#pragma fragmentoption ARB_precision_hint_fastest		 
			
			#define UNITY_PASS_FORWARDBASE  
            #include "UnityCG.cginc"
            #include "AutoLight.cginc" 
            #pragma multi_compile_fwdbase_fullshadows
			    
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF

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

			#ifndef V_WIRE_INSIDE_TANGENT_ON
				#define V_WIRE_HAS_TEXTURE
			#endif
				
			#define V_WIRE_REFLECTION  
			#define V_WIRE_REFLECTION_COLOR

			#include "../cginc/Wire.cginc"

	    	ENDCG

    	} //Pass
		

		Pass  
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }
			Fog {Mode Off}
			ZWrite On ZTest LEqual Cull Off
			Offset 1, 1
	 
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag_ShadowCaster   
			 
			#pragma multi_compile_shadowcaster 
			#define UNITY_PASS_SHADOWCASTER
			#include "UnityCG.cginc"  
			  
					 
		    #include "../cginc/Wire.cginc"
 
			       
			ENDCG 
		}	//ShadowCaster   


		Pass 
		{  
			Name "ShadowCollector"
			Tags { "LightMode" = "ShadowCollector" }
			Fog {Mode Off}
			ZWrite On ZTest LEqual

			    
			CGPROGRAM   
			#pragma vertex vert
			#pragma fragment frag_ShadowCollector
			 
			#pragma multi_compile_shadowcollector
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			#define UNITY_PASS_SHADOWCOLLECTOR
			#define SHADOW_COLLECTOR_PASS
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

					 
 		    #include "../cginc/Wire.cginc"
			 
		 
 			ENDCG
		}

        
    } //SubShader

	CustomEditor "TheAmazingWireframeMaterial_Editor"
} //Shader
