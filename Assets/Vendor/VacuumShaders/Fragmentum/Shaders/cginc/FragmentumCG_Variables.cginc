// Upgrade NOTE: commented out 'half4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable

#ifndef VACUUM_FRAGMENTUM_CG_VARIABLES_INCLUDED
#define VACUUM_FRAGMENTUM_CG_VARIABLES_INCLUDED


//************************************************************************
//Variables
//************************************************************************
#ifdef SHADER_API_D3D11
CBUFFER_START(constants)
	StructuredBuffer<float3> buf_Center;
	StructuredBuffer<float3> buf_Normal;
	StructuredBuffer<float2> buf_UV; 
	StructuredBuffer<float4> buf_RandomVector;	
		
	StructuredBuffer<float2> buf_UV2;	

	float V_FR_Devider;
CBUFFER_END
#endif


#if defined(V_FR_MAIN_COLOR)
	fixed4 _Color;
#endif
sampler2D _MainTex;
#ifndef V_FR_SURFACE
	half4 _MainTex_ST;
#endif

#if defined(V_FR_BUMPSPECULAR) || defined(V_FR_REFLECTION_BUMPSPECULAR)
	half _Shininess;
	sampler2D _BumpMap;
	float4 _BumpMap_ST;
#endif

#if defined(V_FR_REFLECTION) || defined(V_FR_REFLECTION_BUMPSPECULAR)
	samplerCUBE _Cube;

	#ifdef V_FR_REFLECTION_COLOR
		fixed4 _ReflectColor;
	#endif
#endif

#if defined(LIGHTMAP_ON) && !defined(V_FR_SURFACE)
	// half4 unity_LightmapST;
	// sampler2D unity_Lightmap;				
#endif

#if defined(PASS_FORWARD_BASE) || defined(PASS_FORWARD_ADD)
	uniform half4 _LightColor0;
#endif

#if defined(V_FR_CUTOUT)
	half _Cutoff;
#endif

#ifdef V_FR_GLOBAL_CONTROL
	uniform half V_FR_Global_Control;
#endif

half V_FR_Fragmentum;
half V_FR_DisplaceAmount;
half V_FR_RotateSpeed;	
half4 V_FR_DisplaceDirectionObjectPosition;
#if defined(V_FR_FRAGMENTS_SCALE)
	half V_FR_FragmentsScale;
#endif


#ifdef V_FR_EXTRUDE
	half V_FR_ExtrudeAmount;
#endif

#ifdef V_FR_FRAGMENT_TEXTURE_ON
	sampler2D V_FR_FragTexture;
	half4     V_FR_FragTexture_ST;	

	#ifdef V_FR_TEXTURE_POW
		half V_FR_FragTexturePower;
	#endif
#endif


#if defined(V_FR_ACTIVATOR_PLANE) || defined(V_FR_ACTIVATOR_SPHERE)
	half V_FR_DistanceToActivator;
#endif
#if defined(V_FR_ACTIVATOR_PLANE)
	uniform half4 V_FR_ActivatorPlanePosition;
	uniform half4 V_FR_ActivatorPlaneNormal;
#endif
#if defined(V_FR_ACTIVATOR_SPHERE)
	uniform half4 V_FR_ActivatorSphereObject; // xyz - pos, w - radius
#endif

#if defined(V_FR_ROTATE_CUSTOM_POINT)
	uniform half4 V_FR_RotateCustomPointPosition;
	uniform half4 V_FR_RotateCustomPointNormal;
#endif

#if defined(V_FR_RANDOMIZE_DISTANCE_TO_ACTIVATOR)
	half V_FR_RandomizeDistanceToActivator;
#endif

#if defined(V_FR_RANDOMIZE_FRAGMENTUM)
	half V_FR_RandomizeFragmentum;
#endif
#if defined(V_FR_RANDOMIZE_SCALE)
	half V_FR_RandomizeFragmentsScale;
#endif
#if defined(V_FR_RANDOMIZE_DISPLACE_AMOUNT)
	half V_FR_RandomizeDisplaceAmount;
#endif
#if defined(V_FR_RANDOMIZE_DISPLACE_DIRECTION)
	half V_FR_RandomizeDisplaceDirection;
#endif
#if defined(V_FR_RANDOMIZE_INITIAL_ROTATION)
	half V_FR_RandomizeInitialRotation;
#endif
#if defined(V_FR_RANDOMIZE_ROTATION_SPEED)
	half V_FR_RandomizeRotationSpeed;
#endif
#if defined(V_FR_RANDOMIZE_ROTATION_TIME_OFFSET)
	half V_FR_RandomizeRotationTimeOffset;
#endif

#endif