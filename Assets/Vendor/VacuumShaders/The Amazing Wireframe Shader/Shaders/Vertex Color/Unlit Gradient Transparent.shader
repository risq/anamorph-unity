// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/The Amazing Wireframe/Vertex Color/Unlit/Gradient/Transparent"
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
			   "WireframeTag"="Vertex Color/Unlit/Gradient/Transparent"
			   "WireframeBakedTag"=""
			 }

		Blend SrcAlpha OneMinusSrcAlpha 


		Pass 
		{
			ZWrite On
			ColorMask 0
		}

		Pass
	    {			 
		 
            CGPROGRAM 
		    #pragma vertex vert
	    	#pragma fragment frag
	    	#pragma fragmentoption ARB_precision_hint_fastest		 
			#define UNITY_PASS_UNLIT

            #pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF

			#pragma multi_compile V_WIRE_ANTIALIASING_OFF V_WIRE_ANTIALIASING_ON
			#pragma multi_compile V_WIRE_FRESNEL_OFF V_WIRE_FRESNEL_ON
			#pragma multi_compile V_WIRE_LIGHT_OFF V_WIRE_LIGHT_ON
			#pragma multi_compile V_WIRE_IBL_OFF V_WIRE_IBL_ON

			#pragma multi_compile V_WIRE_GRADIENT_PREVIEW_OFF V_WIRE_GRADIENT_PREVIEW_ON
			#pragma multi_compile V_WIRE_GRADIENT_AXIS_X V_WIRE_GRADIENT_AXIS_Y V_WIRE_GRADIENT_AXIS_Z
			#pragma multi_compile V_WIRE_GRADIENT_SPACE_LOCAL V_WIRE_GRADIENT_SPACE_WORLD



			#define V_WIRE_GRADIENT
			#define V_WIRE_GRADIENT_TRANSPARENT
			#define V_WIRE_INSIDE_TANGENT_ON

			#ifdef V_WIRE_ANTIALIASING_ON
				#pragma target 3.0
				#pragma glsl
			#endif

			#include "../cginc/Wire.cginc"

			ENDCG

    	} //Pass
			
        
    } //SubShader

	CustomEditor "TheAmazingWireframeMaterial_Editor"
} //Shader
