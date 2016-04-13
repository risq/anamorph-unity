// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/Fragmentum/Examples/6b Randomize Displace Amount Off"
{
    Properties     
    {   
		[HideInInspector]
		_Color ("", Color) = (1,1,1,1)
		[HideInInspector]
		_MainTex ("", 2D) = "white" {}	   
		     
		              
		            
		[HideInInspector]   
		V_FR_Fragmentum("", Range(0, 1)) = 0.5
		[HideInInspector]
		V_FR_DisplaceAmount("", float) = 1	 
		[HideInInspector]     
		V_FR_RotateSpeed("", float) = 1    
		[HideInInspector]
		V_FR_DisplaceDirectionObjectPosition("", vector) = (0, 1, 0, 0)
		[HideInInspector]  
		V_FR_FragmentsScale("", float) = 1  
		     
		[HideInInspector]   
		V_FR_DistanceToActivator("", float) = 0
		   
 		[HideInInspector]   
		V_FR_FragTexture("", 2D) = "white"{}		
		[HideInInspector] 
		V_FR_FragTexturePower("", Range(1, 10)) = 1
		     		      
		[HideInInspector] 
		V_FR_RandomizeFragmentum("", float) = 0   
		[HideInInspector]
	 	V_FR_RandomizeFragmentsScale("", float) = 0 
		[HideInInspector]
		V_FR_RandomizeDistanceToActivator("", float) = 0 
		[HideInInspector] 
		V_FR_RandomizeInitialRotation("", float) = 0
		[HideInInspector] 
		V_FR_RandomizeRotationSpeed("", float) = 0
		[HideInInspector]
		V_FR_RandomizeRotationTimeOffset("", float) = 0
		[HideInInspector]   
		V_FR_RandomizeDisplaceDirection("", Range(0, 1)) = 0
		[HideInInspector]   
		V_FR_RandomizeDisplaceAmount("", float) = 0  
		 
    }
	         
    SubShader      
    {  

		Tags { "Queue"="Geometry" "RenderType"="Opaque" "FragmentumTag"="Fragmentum/SM2/One Directional Light/Diffuse"}

//DO NOT MODIFY !!! Cull		
Cull Off
  
		LOD 200
		         
		Pass                             
	    {     
			Name "FORWARD"
			Tags { "LightMode" = "ForwardBase" } 
			  
            CGPROGRAM             
		    #pragma vertex vert 
            #pragma fragment frag

            #define UNITY_PASS_FORWARDBASE 
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            
			#pragma exclude_renderers xbox360 ps3 flash gles

			  
			#pragma multi_compile LIGHTMAP_ON LIGHTMAP_OFF
			#pragma multi_compile V_FR_EDITOR_ON V_FR_EDITOR_OFF 			
			
//DO NOT MODIFY !!! Defines
#define V_FR_GLOBAL_CONTROL

	 	      
		      

			#define SM_2
			#define PASS_FORWARD_BASE

		    #include "../cginc/FragmentumCG_MOBILE.cginc"
			   
			      
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
			  
			#pragma exclude_renderers xbox360 ps3 flash gles
            #pragma target 3.0
			#pragma glsl
			  
			#pragma multi_compile V_FR_EDITOR_ON V_FR_EDITOR_OFF 			
			
//DO NOT MODIFY !!! Defines
#define V_FR_GLOBAL_CONTROL

        
			 
			#define SM_2
			#define PASS_SHADOW_CASTER

		    #include "../cginc/FragmentumCG_MOBILE.cginc"
 
			       
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

			#pragma exclude_renderers xbox360 ps3 flash gles
            #pragma target 3.0
			#pragma glsl 
			  
			#pragma multi_compile V_FR_EDITOR_ON V_FR_EDITOR_OFF 			
			
//DO NOT MODIFY !!! Defines
#define V_FR_GLOBAL_CONTROL


			  
			#define SM_2
			#define PASS_SHADOW_COLLECTOR 
		 
 		    #include "../cginc/FragmentumCG_MOBILE.cginc"
			 
		 
 			ENDCG
		}

    } //SubShader

	CustomEditor "FragmentumMaterial_Editor"

} //Shader
