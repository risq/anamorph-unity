#ifndef VACUUM_FRAGMENTUM_CG_FUNCTIONS_INCLUDED
#define VACUUM_FRAGMENTUM_CG_FUNCTIONS_INCLUDED


//************************************************************************
//Functions
//************************************************************************
#ifdef V_FR_FRAGMENTS_SCALE
inline half3 ScaleFragment(half3 vertex, half amount, half randomVectorLength, half3 center)
{
	
	#ifdef V_FR_RANDOMIZE_SCALE
		half scale = lerp(1, V_FR_FragmentsScale + randomVectorLength * V_FR_RandomizeFragmentsScale, abs(amount));
	#else
		half scale = lerp(1, V_FR_FragmentsScale, abs(amount));
	#endif
	
	  
	//Scale fragment
	return center + (vertex - center) * max(scale, 0);
}
#endif


inline float3 V_FR_RotateOrigin(in float3 p, in float3 axis, float theta)
{
	float cost = cos(theta);
	float sint = sin(theta);
			
	float uxvywz = (axis.x * p.x + axis.y * p.y + axis.z * p.z) * (1 - cost);

	float3 rotPoint = { axis.x * uxvywz + p.x * cost + (axis.y * p.z - axis.z * p.y) * sint,
	                    axis.y * uxvywz + p.y * cost + (axis.z * p.x - axis.x * p.z) * sint,
	  			        axis.z * uxvywz + p.z * cost + (axis.x * p.y - axis.y * p.x) * sint
				       };	

	return rotPoint;
}

//opengl - 67
//d3d9 - 51
//dx11 - 14
inline float3 V_FR_RotateArbitrary (in float3 p, in float3 axisPoint, in float3 dir, float theta)
{
	float cost = cos(theta);
	float sint = sin(theta);
	float cos1 = 1 - cost;

			
	float uxvywz = dir.x * p.x + dir.y * p.y + dir.z * p.z;

	float3 rotPoint = { (axisPoint.x * (dir.y * dir.y + dir.z * dir.z) - dir.x * (axisPoint.y * dir.y + axisPoint.z * dir.z - uxvywz)) * cos1 + p.x * cost + (-axisPoint.z * dir.y + axisPoint.y * dir.z - dir.z * p.y + dir.y * p.z) * sint,
                        (axisPoint.y * (dir.x * dir.x + dir.z * dir.z) - dir.y * (axisPoint.x * dir.x + axisPoint.z * dir.z - uxvywz)) * cos1 + p.y * cost + ( axisPoint.z * dir.x - axisPoint.x * dir.z + dir.z * p.x - dir.x * p.z) * sint,
					    (axisPoint.z * (dir.x * dir.x + dir.y * dir.y) - dir.z * (axisPoint.x * dir.x + axisPoint.y * dir.y - uxvywz)) * cos1 + p.z * cost + (-axisPoint.y * dir.x + axisPoint.x * dir.y - dir.y * p.x + dir.x * p.y) * sint 
				       };

	return rotPoint;
}


#ifdef LIGHTMAP_ON
inline fixed3 FR_DecodeLightmap( fixed4 color )
{
	#if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
		return 2.0 * color.rgb;
	#else
		return (8.0 * color.a) * color.rgb;
	#endif
}
#endif

inline float3 V_FR_WorldSpaceViewDir( in float4 v )
{
	return _WorldSpaceCameraPos.xyz - mul(_Object2World, v).xyz;
}

#endif