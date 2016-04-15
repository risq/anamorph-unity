// Upgrade NOTE: commented out 'half4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

#ifndef VACUUM_WIRE_VARIABLES_CGINC
#define VACUUM_WIRE_VARIABLES_CGINC


 
fixed4 _Color;

#ifdef V_WIRE_SURFACE
	sampler2D _MainTex;
#else
	#ifdef V_WIRE_HAS_TEXTURE
		sampler2D _MainTex;
		half4 _MainTex_ST;
	#endif
#endif

#ifdef V_WIRE_BUMP
	sampler2D _BumpMap;
#endif

#ifdef V_WIRE_GLOSS
	half _Shininess;
#endif

#if defined(LIGHTMAP_ON) && !defined(V_WIRE_SURFACE)
	// half4 unity_LightmapST;
	// sampler2D unity_Lightmap;				
#endif

#if defined(UNITY_PASS_FORWARDBASE) && !defined(V_WIRE_SURFACE)
	uniform half4 _LightColor0;
#endif

#if defined(V_WIRE_REFLECTION) || defined(V_WIRE_REFLECTION_BUMPSPECULAR)
	samplerCUBE _Cube;

	#ifdef V_WIRE_REFLECTION_COLOR
		fixed4 _ReflectColor;
	#endif
#endif

#if defined(V_WIRE_CUTOUT) && !defined(V_WIRE_SURFACE)
	half _Cutoff;
#endif

fixed4 _V_WIRE_Color;
half _V_WIRE_Size;

#ifdef V_WIRE_GRADIENT
	half _V_WIRE_GradientMin;
	half _V_WIRE_GradientMax;
	half _V_WIRE_GradientOffset;
	fixed4 _V_WIRE_GradientColor;
#endif

#ifdef V_WIRE_FRESNEL_ON
	half _V_WIRE_Fresnel_Bias;
#endif

#ifdef V_WIRE_IBL_ON
	half _V_WIRE_IBL_Intensity;
	half _V_WIRE_IBL_Contrast;
	samplerCUBE _V_WIRE_IBL_Cube;	
#endif

#ifdef V_WIRE_TRANSPARENCY_CUSTOM_TEXTURE
	sampler2D _V_WIRE_Transparent_Tex;
#endif

#ifdef V_WIRE_FRESNEL_REFLECTION_ON
	half _V_WIRE_Fresnel_Reflection_Bias;
#endif

#endif