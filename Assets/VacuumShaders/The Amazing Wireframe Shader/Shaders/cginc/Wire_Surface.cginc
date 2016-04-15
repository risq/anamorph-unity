#ifndef VACUUM_WIRE_SURFACE_CGINC
#define VACUUM_WIRE_SURFACE_CGINC

struct Input 
{
	half2 uv_MainTex;

	#if defined(V_WIRE_BUMP) && !defined(V_WIRE_BUMP_NO_UV)
		float2 uv_BumpMap;
	#endif
	
	#if defined(V_WIRE_FRESNEL_ON) || defined(V_WIRE_FRESNEL_REFLECTION_ON)
		float3 viewDir;
	#endif

	#if defined(V_WIRE_IBL_ON) || defined(V_WIRE_FRESNEL_ON) || defined(V_WIRE_FRESNEL_REFLECTION_ON)
		float3 worldNormal;
	#endif

	#ifdef V_WIRE_REFLECTION
		float3 worldRefl;
	#endif
	
	#ifdef V_WIRE_TANGETNROT
		INTERNAL_DATA
	#endif


	#ifdef V_WIRE_GRADIENT
		half3 gradPos;
	#endif

	float4 color : COLOR;
};

#ifdef V_WIRE_GRADIENT
void vert (inout appdata_full v, out Input o) 
{
	UNITY_INITIALIZE_OUTPUT(Input,o);
	
	#ifdef V_WIRE_GRADIENT		
		#ifdef V_WIRE_GRADIENT_SPACE_LOCAL
			o.gradPos = v.vertex.xyz;
		#else
			o.gradPos = mul(_Object2World, half4(v.vertex.xyz, 1)).xyz;
		#endif 
	#endif
}
#endif


//////////////////////////////////////////////////////////////////////////////
//                                                                          // 
//Surface Shader                                                            //                
//                                                                          //               
//////////////////////////////////////////////////////////////////////////////
void surf (Input IN, inout SurfaceOutput o) 
{
	half4 retColor = tex2D (_MainTex, IN.uv_MainTex);
	
	#ifdef V_WIRE_BUMP
		#ifdef V_WIRE_BUMP_NO_UV
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
		#else
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		#endif
	#endif


	#if defined(V_WIRE_FRESNEL_ON) || defined(V_WIRE_FRESNEL_REFLECTION_ON)
		#ifdef V_WIRE_BUMP
			half fresnel = dot (normalize(IN.viewDir), o.Normal);
			fresnel *= fresnel;
		#else
			half fresnel = dot (normalize(IN.viewDir), IN.worldNormal);
			fresnel *= fresnel;
		#endif
	#endif

	//Reflection
	#ifdef V_WIRE_REFLECTION
		#ifdef V_WIRE_BUMP
			fixed4 reflcol = texCUBE (_Cube, WorldReflectionVector (IN, o.Normal));
		#else
			fixed4 reflcol = texCUBE (_Cube, IN.worldRefl);
		#endif
		
		#ifdef V_WIRE_REFLECTION_COLOR
			reflcol.rgb *= _ReflectColor.rgb;			
		#endif

		#ifdef V_WIRE_FRESNEL_REFLECTION_ON
			reflcol.rgb *= max(0, _V_WIRE_Fresnel_Reflection_Bias - fresnel);
		#endif
		

		o.Emission = reflcol.rgb * retColor.a;
	#endif

	//Specular
	#ifdef V_WIRE_GLOSS
		o.Gloss = retColor.a;
		o.Specular = _Shininess;
	#endif

	retColor *= _Color;

	//Wire
	#ifdef V_WIRE_FRESNEL_ON
		#ifdef V_WIRE_BUMP
			_V_WIRE_Color.a *= max(0, _V_WIRE_Fresnel_Bias - saturate(fresnel));
		#else
			_V_WIRE_Color.a *= max(0, _V_WIRE_Fresnel_Bias - saturate(fresnel));
		#endif
	#endif


	#ifdef V_WIRE_GRADIENT
		half grad = V_WIRE_Gradient(IN.gradPos);
	#endif

	#ifdef V_WIRE_LIGHT_ON 
		#ifdef V_WIRE_TRANSPARENCY_BASE_TEXTURE
			_V_WIRE_Color.a *= retColor.a;
		#endif
		#ifdef V_WIRE_TRANSPARENCY_BASE_TEXTURE_INVERT
			_V_WIRE_Color.a *= 1 - retColor.a;
		#endif
		#ifdef V_WIRE_TRANSPARENCY_CUSTOM_TEXTURE
			_V_WIRE_Color.a *= tex2D(_V_WIRE_Transparent_Tex, IN.uv_MainTex).a;
		#endif

		#ifdef V_WIRE_GRADIENT
			Wire(retColor, IN.color, grad);
		#else
			Wire(retColor, IN.color, 1);
		#endif
	#endif


	o.Albedo = retColor.rgb;
	o.Alpha = retColor.a; 


	//Point light fix for unlit gradient wire
	#ifndef V_WIRE_LIGHT_ON 
		#ifdef V_WIRE_GRADIENT			
			//o.Alpha *= grad;

			#ifndef V_WIRE_GRADIENT_TRANSPARENT
			//	o.Albedo *= grad;
			#endif
		#endif
	#endif
	

	#ifdef V_WIRE_IBL_ON
		#ifdef V_WIRE_BUMP
			o.Emission += V_WIRE_IBL(WorldNormalVector (IN, o.Normal)) * o.Albedo;
		#else
			o.Emission += V_WIRE_IBL(normalize(IN.worldNormal)) * o.Albedo;
		#endif
	#endif
}

void wireColor (Input IN, SurfaceOutput o, inout fixed4 color)
{	
	#if defined(V_WIRE_GRADIENT) && defined(V_WIRE_GRADIENT_PREVIEW_ON)
		color = fixed4( V_WIRE_Gradient(IN.gradPos).xxx, 1);
	#else
		//#if defined(UNITY_PASS_FORWARDBASE)
			#ifndef V_WIRE_LIGHT_ON

				#ifdef V_WIRE_TRANSPARENCY_BASE_TEXTURE
					_V_WIRE_Color.a *= o.Alpha;
				#endif
				#ifdef V_WIRE_TRANSPARENCY_BASE_TEXTURE_INVERT
					_V_WIRE_Color.a *= 1 - o.Alpha;
				#endif
				#ifdef V_WIRE_TRANSPARENCY_CUSTOM_TEXTURE
					_V_WIRE_Color.a *= tex2D(_V_WIRE_Transparent_Tex, IN.uv_MainTex).a;
				#endif

				#ifdef V_WIRE_GRADIENT
					Wire(color, IN.color, V_WIRE_Gradient(IN.gradPos));
				#else
					Wire(color, IN.color, 1);					
				#endif
			#endif
		//#endif
	#endif  	
} 

#endif