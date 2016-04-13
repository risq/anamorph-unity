// VacuumShaders 2015
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/Mesh Materializer/Specular/Cutout"
{
	Properties 
	{
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125

		[Toggle(V_MM_TEXTURE_AND_COLOR_ON)] _UseTexture ("Use Texture & Color", Float) = 0
				
		[CanBeHidden] _Color("Color", color) = (1, 1, 1, 1)
		[CanBeHidden] _MainTex("Texture", 2D) = "white"{}


		[Toggle(V_MM_REFLECTION_ON)] _UseReflection("Use Reflection", Float) = 0
		[CanBeHidden] _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
		[CanBeHidden] _Cube ("Reflection Cubemap", Cube) = "_Skybox" {}


		[Toggle(V_MM_IBL_ON)] _UseIBL ("Use Image Based Lighting", Float) = 0

		[CanBeHidden] _V_MM_IBL_Cube("IBL Cube", cube ) = ""{}  
		[CanBeHidden] _V_MM_IBL_Cube_Intensity("IBL Cube Intensity", float) = 1
		[CanBeHidden] _V_MM_IBL_Cube_Contrast("IBL Cube Contrast", float) = 1 

		
		[Toggle(V_MM_EMISSION_ON)] _UseEmission ("Use Emission", Float) = 0
		[CanBeHidden] _Emission("Emission", float) = 1


		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}

	SubShader 
	{
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong alphatest:_Cutoff addshadow 

		#pragma multi_compile V_MM_TEXTURE_AND_COLOR_OFF V_MM_TEXTURE_AND_COLOR_ON
		#pragma multi_compile V_MM_REFLECTION_OFF V_MM_REFLECTION_ON
		#pragma multi_compile V_MM_IBL_OFF V_MM_IBL_ON
		#pragma multi_compile V_MM_EMISSION_OFF V_MM_EMISSION_ON

		
		fixed4 _Color;
		sampler2D _MainTex;
		half _Shininess;

		#ifdef V_MM_REFLECTION_ON
			fixed4 _ReflectColor;
			samplerCUBE _Cube;
		#endif

		#ifdef V_MM_IBL_ON
			samplerCUBE _V_MM_IBL_Cube;
			fixed _V_MM_IBL_Cube_Intensity;
			fixed _V_MM_IBL_Cube_Contrast;
		#endif

		half _Emission;

		struct Input 
		{
			float2 uv_MainTex;

			#ifdef V_MM_REFLECTION_ON
				float3 worldRefl;
			#endif

			#ifdef V_MM_IBL_ON
				float3 worldNormal;
			#endif

			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = IN.color;

			#ifdef V_MM_TEXTURE_AND_COLOR_ON
				c *= tex2D(_MainTex, IN.uv_MainTex) * _Color;
			#endif

			o.Gloss = c.a;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Specular = _Shininess;

			#ifdef V_MM_IBL_ON
				fixed3 ibl = ((texCUBE(_V_MM_IBL_Cube, IN.worldNormal).rgb - 0.5) * _V_MM_IBL_Cube_Contrast + 0.5) * _V_MM_IBL_Cube_Intensity;
				
				o.Emission = ibl * o.Albedo;
			#endif

			#ifdef V_MM_REFLECTION_ON
				fixed4 reflcol = texCUBE (_Cube, IN.worldRefl);
				reflcol *= c.a;
				o.Emission += reflcol.rgb * _ReflectColor.rgb;
			#endif

			#ifdef V_MM_EMISSION_ON
				o.Emission += _Emission * c.rgb * c.a;
			#endif
		}
		ENDCG
	} 

	Fallback "VacuumShaders/Mesh MaterializerUnlit/Cutout"
}
