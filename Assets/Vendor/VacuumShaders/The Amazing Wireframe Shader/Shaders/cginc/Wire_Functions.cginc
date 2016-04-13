// Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'

#ifndef VACUUM_WIRE_FUNCTIONS_CGINC
#define VACUUM_WIRE_FUNCTIONS_CGINC

#ifdef LIGHTMAP_ON
inline fixed3 WIRE_DecodeLightmap( fixed4 color )
{
	#if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
		return 2.0 * color.rgb;
	#else
		return (8.0 * color.a) * color.rgb;
	#endif
}
#endif

inline float3 V_WIRE_WorldSpaceViewDir( in float4 v )
{
	return _WorldSpaceCameraPos.xyz - mul(_Object2World, v).xyz;
}

inline half3 V_WIRE_ObjViewDir ( half3 vertexPos )
{				
	half3 objSpaceCameraPos = mul(_World2Object, half4(_WorldSpaceCameraPos.xyz, 1)).xyz * 1.0;
				
	return normalize(objSpaceCameraPos - vertexPos);
}

inline void Wire(inout fixed4 srcColor, fixed4 mass, half gradient)
{

	#ifdef V_WIRE_ANTIALIASING_ON
		half3 width = abs(ddx(mass.xyz)) + abs(ddy(mass.xyz));
		half3 eF = smoothstep(half3(0, 0, 0), width * _V_WIRE_Size * 20, mass.xyz);		
	
		half value = min(min(eF.x, eF.y), eF.z);		
	#else		
		half value = step(_V_WIRE_Size, min(min(mass.x, mass.y), mass.z));		
	#endif



	#ifdef V_WIRE_SAME_COLOR
		srcColor.rgb = _V_WIRE_Color.rgb;
	#endif

	
		
	#if !defined(V_WIRE_TRANSPARENT) && !defined(V_WIRE_SAME_COLOR) && !defined(V_WIRE_GRADIENT)
		_V_WIRE_Color = lerp(srcColor, _V_WIRE_Color, _V_WIRE_Color.a);
	#endif 
		


	#ifdef V_WIRE_GRADIENT
		value = lerp(value, 1, gradient);
		fixed3 gradColor = lerp(srcColor.rgb, _V_WIRE_GradientColor.rgb, _V_WIRE_GradientColor.a);
		srcColor.rgb = lerp(gradColor, srcColor.rgb, gradient);


		#ifdef V_WIRE_GRADIENT_TRANSPARENT			
			srcColor.a = lerp(gradient, 1, _V_WIRE_GradientColor.a);			
		#endif

		_V_WIRE_Color = lerp(srcColor, _V_WIRE_Color, _V_WIRE_Color.a);
	#endif


	srcColor = lerp(_V_WIRE_Color, srcColor, value);

}

#ifdef V_WIRE_GRADIENT
inline half V_WIRE_Gradient(half3 gradPos)
{
	_V_WIRE_GradientMax += _V_WIRE_GradientOffset;
	_V_WIRE_GradientMin += _V_WIRE_GradientOffset;

	#ifdef V_WIRE_GRADIENT_AXIS_X
		return saturate((gradPos.x - _V_WIRE_GradientMin) / (_V_WIRE_GradientMax - _V_WIRE_GradientMin));

	#else
		#ifdef V_WIRE_GRADIENT_AXIS_Y
			return saturate((gradPos.y - _V_WIRE_GradientMin) / (_V_WIRE_GradientMax - _V_WIRE_GradientMin));
		#else
			return saturate((gradPos.z - _V_WIRE_GradientMin) / (_V_WIRE_GradientMax - _V_WIRE_GradientMin));
		#endif
	#endif
}
#endif

#define V_WIRE_IBL(n) ((texCUBE(_V_WIRE_IBL_Cube, n).rgb - 0.5) * _V_WIRE_IBL_Contrast + 0.5) * _V_WIRE_IBL_Intensity

#endif