// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/The Amazing Wireframe/Vertex Color/Unlit/Addative/Only Wire"
{
    Properties 
    {
		//Tag
		[Tag]
		V_WIRE_TAG("", float) = 0
		
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
		
    }

    SubShader  
    {
		Tags { "Queue"="Transparent+20" 
		       "IgnoreProjector"="True" 
			   "RenderType"="Transparent" 
			   "WireframeTag"="Vertex Color/Unlit/Addative/Only Wire"
			   "WireframeBakedTag"=""
			 }

		
		Blend SrcAlpha One
		AlphaTest Greater .01
		ColorMask RGB
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }
		  

		Pass 
	    {			

            CGPROGRAM
		    #pragma vertex vert
	    	#pragma fragment frag 
	    	#pragma fragmentoption ARB_precision_hint_fastest		 
			#define UNITY_PASS_UNLIT

			#pragma multi_compile V_WIRE_ANTIALIASING_OFF V_WIRE_ANTIALIASING_ON

			#define V_WIRE_TRANSPARENT
			
			#ifdef V_WIRE_ANTIALIASING_ON
				#pragma target 3.0
				#pragma glsl
			#endif

			#define V_WIRE_INSIDE_TANGENT_ON
			#define V_WIRE_NO_COLOR_0
			

			#include "../cginc/Wire.cginc"

	    	ENDCG

    	} //Pass
        
    } //SubShader


	CustomEditor "TheAmazingWireframeMaterial_Editor"
} //Shader
