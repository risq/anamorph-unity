// VacuumShaders 2015
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/Mesh Materializer/Unlit/Transparent"
{ 
	Properties 
	{
		[Toggle(V_MM_TEXTURE_AND_COLOR_ON)] _UseTexture ("Use Texture & Color", Float) = 0
				
		[CanBeHidden] _Color("Color", color) = (1, 1, 1, 1)
		[CanBeHidden] _MainTex("Texture", 2D) = "white"{}


		[Toggle(V_MM_REFLECTION_ON)] _UseReflection("Use Reflection", Float) = 0
		[CanBeHidden] _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		[CanBeHidden] _Cube ("Reflection Cubemap", Cube) = "_Skybox" { }


		[Toggle(V_MM_IBL_ON)] _UseIBL ("Use Image Based Lighting", Float) = 0

		[CanBeHidden] _V_MM_IBL_Cube("IBL Cube", cube ) = ""{}  
		[CanBeHidden] _V_MM_IBL_Cube_Intensity("IBL Cube Intensity", float) = 1
		[CanBeHidden] _V_MM_IBL_Cube_Contrast("IBL Cube Contrast", float) = 1 
		[CanBeHidden] _V_MM_IBL_Light_Intensity("IBL Light Intensity", Range(0, 1)) = 0.2

		[Toggle(V_MM_EMISSION_ON)] _UseEmission ("Use Emission", Float) = 0
		[CanBeHidden] _Emission("Emission", float) = 1
	}

    SubShader 
    {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100

		//Pass 
		//{
		//	ZWrite On
		//	ColorMask 0
		//}

		Alphatest Greater 0
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		ColorMask RGB

		Pass
	    {			  
		 
            CGPROGRAM 
		    #pragma vertex vert
	    	#pragma fragment frag
	    	#pragma fragmentoption ARB_precision_hint_fastest	
			
			#pragma multi_compile V_MM_TEXTURE_AND_COLOR_OFF V_MM_TEXTURE_AND_COLOR_ON
			#pragma multi_compile V_MM_REFLECTION_OFF V_MM_REFLECTION_ON
			#pragma multi_compile V_MM_IBL_OFF V_MM_IBL_ON
			#pragma multi_compile V_MM_EMISSION_OFF V_MM_EMISSION_ON

			
			#include "UnityCG.cginc"
				 
			 
			#ifdef V_MM_TEXTURE_AND_COLOR_ON	
				fixed4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
			#endif

			#ifdef V_MM_REFLECTION_ON
				fixed4 _ReflectColor;
				samplerCUBE _Cube;
			#endif

			#ifdef V_MM_IBL_ON
				samplerCUBE _V_MM_IBL_Cube;
				fixed _V_MM_IBL_Cube_Intensity;
				fixed _V_MM_IBL_Cube_Contrast;
				fixed _V_MM_IBL_Light_Intensity;
			#endif

			half _Emission;


			struct vInput
			{
				float4 vertex : POSITION;

				#ifdef V_MM_TEXTURE_AND_COLOR_ON
					float4 texcoord : TEXCOORD0;
				#endif

				#if defined(V_MM_REFLECTION_ON) || defined(V_MM_IBL_ON)
					float3 normal : NORMAL;
				#endif

				fixed4 color : COLOR;
			};

			struct vOutput
			{
				float4 pos :SV_POSITION;

				#ifdef V_MM_TEXTURE_AND_COLOR_ON
					float2 texcoord : TEXCOORD0;
				#endif

				#ifdef V_MM_REFLECTION_ON
					float3 worldRefl : TEXCOORD1;
				#endif

				#ifdef V_MM_IBL_ON
					float3 worldNormal : TEXCOORD2;
				#endif
					
				fixed4 color : COLOR;
			};

			vOutput vert(vInput v)
			{
				vOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

				#ifdef V_MM_TEXTURE_AND_COLOR_ON
					o.texcoord = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				#endif

				#ifdef V_MM_REFLECTION_ON
					float3 viewDir = -ObjSpaceViewDir(v.vertex);
					float3 viewRefl = reflect (viewDir, v.normal);
					o.worldRefl = mul ((float3x3)_Object2World, viewRefl);
				#endif

				#ifdef V_MM_IBL_ON
					o.worldNormal = mul((float3x3)_Object2World, SCALED_NORMAL);
				#endif

				o.color = v.color;			

				return o;
			}

			fixed4 frag(vOutput i) : SV_Target 
			{		
				fixed4 albedo = i.color;

				#ifdef V_MM_TEXTURE_AND_COLOR_ON
					albedo *= tex2D(_MainTex, i.texcoord) * _Color;
				#endif


				#ifdef V_MM_IBL_ON
					fixed3 ibl = ((texCUBE(_V_MM_IBL_Cube, i.worldNormal).rgb - 0.5) * _V_MM_IBL_Cube_Contrast + 0.5) * _V_MM_IBL_Cube_Intensity;
					
					albedo.rgb = albedo.rgb * (_V_MM_IBL_Light_Intensity + ibl);
				#endif


				#ifdef V_MM_REFLECTION_ON
					fixed4 reflcol = texCUBE (_Cube, i.worldRefl);
					reflcol *= albedo.a;

					albedo.rgb += reflcol.rgb * _ReflectColor.rgb;
				#endif

				#ifdef V_MM_EMISSION_ON
					albedo.rgb += _Emission * albedo.rgb * albedo.a;
				#endif
				
				return albedo;
			}


			ENDCG 

    	} //Pass			
        
    } //SubShader

} //Shader
