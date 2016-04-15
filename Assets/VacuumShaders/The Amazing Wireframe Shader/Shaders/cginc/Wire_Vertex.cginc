// Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

#ifndef VACUUM_WIRE_VERTEX_CGINC
#define VACUUM_WIRE_VERTEX_CGINC



struct vInput
{
    float4 vertex : POSITION;

	#if defined(UNITY_PASS_FORWARDBASE) || defined(V_WIRE_REFLECTION) || defined(V_WIRE_FRESNEL_ON) || defined(V_WIRE_FRESNEL_REFLECTION_ON) || defined(V_WIRE_IBL_ON)
		half3 normal : NORMAL;
	#endif

	#ifdef V_WIRE_HAS_TEXTURE
		half4 texcoord : TEXCOORD0;
	#endif

	#ifdef LIGHTMAP_ON
		half4 texcoord1 :TEXCOORD1;
	#endif

	#ifdef V_WIRE_INSIDE_TANGENT_ON
		half4 tangent : TANGENT;
	#endif

	fixed4 color : COLOR;
};

struct vOutput
{
	#if defined(UNITY_PASS_SHADOWCOLLECTOR) || defined(UNITY_PASS_SHADOWCASTER)

		#ifdef UNITY_PASS_SHADOWCOLLECTOR
			V2F_SHADOW_COLLECTOR;
		#endif

		#ifdef UNITY_PASS_SHADOWCASTER
			V2F_SHADOW_CASTER;
		#endif

		#ifdef V_WIRE_HAS_TEXTURE
			half2 uv :TEXCOORD5;
		#endif
				
	#else
		 float4 pos :SV_POSITION;

		#if defined(V_WIRE_HAS_TEXTURE) || defined(LIGHTMAP_ON)
			half4 uv : TEXCOORD0;	//xy - mainTex, zw - lightMap
		#endif


		#ifdef V_WIRE_REFLECTION
			half3 refl : TEXCOORD1;			
		#endif

		#ifdef V_WIRE_INSIDE_TANGENT_ON
			fixed4 vColor : TEXCOORD2;
		#endif

		#if defined(UNITY_PASS_FORWARDBASE) || defined(PASS_FORWARD_ADD) || defined(V_WIRE_FRESNEL_ON) || defined(V_WIRE_IBL_ON)
			half3 normal: TEXCOORD3;
		#endif

		#if defined(V_WIRE_FRESNEL_ON) || defined(V_WIRE_FRESNEL_REFLECTION_ON) || defined(V_WIRE_GRADIENT)
			half2 vfx : TEXCOORD4;	//x - wire_fresnel, y - reflection_fresnel
		#endif

		#ifdef V_WIRE_GRADIENT
			half3 gradPos : TEXCOORD5;
		#endif

		#if defined(UNITY_PASS_FORWARDBASE) || defined(PASS_FORWARD_ADD)	
			#ifndef V_WIRE_TRANSPARENT
				LIGHTING_COORDS(6,7)
			#endif
		#endif
	#endif

	fixed4 mass : COLOR;   
};


//////////////////////////////////////////////////////////////////////////////
//                                                                          // 
//Vertex Shaders                                                            //                
//                                                                          //               
//////////////////////////////////////////////////////////////////////////////
vOutput vert(vInput v)
{ 
	vOutput o = (vOutput)0;
	
	#ifdef V_WIRE_INSIDE_TANGENT_ON	
		o.mass = v.tangent;
		o.vColor = v.color;
	#else
		o.mass = v.color;
	#endif

	#if defined(V_WIRE_REFLECTION)
		half3 worldN = mul((half3x3)_Object2World, v.normal * 1.0);

		half3 viewDir = _WorldSpaceCameraPos.xyz - mul(_Object2World, v.vertex).xyz;
		
		o.refl = reflect( -viewDir, worldN );
	#endif

	

	
	#if defined(V_WIRE_FRESNEL_ON) || defined(V_WIRE_FRESNEL_REFLECTION_ON)
		
		half fresnel = dot ( v.normal, V_WIRE_ObjViewDir(v.vertex.xyz) );
		fresnel *= fresnel;

		#ifdef V_WIRE_FRESNEL_ON
			o.vfx.x = max(0, _V_WIRE_Fresnel_Bias - fresnel);
		#endif

		#ifdef V_WIRE_FRESNEL_REFLECTION_ON
			o.vfx.y = max(0, _V_WIRE_Fresnel_Reflection_Bias - fresnel);
		#endif
	#endif

	#if defined(UNITY_PASS_FORWARDBASE) || defined(V_WIRE_IBL_ON)
		o.normal = mul((half3x3)_Object2World, v.normal * 1.0);
	#endif
	 

	#ifdef V_WIRE_HAS_TEXTURE
		o.uv.xy = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;		
	#endif

	#ifdef V_WIRE_GRADIENT		
		#ifdef V_WIRE_GRADIENT_SPACE_LOCAL
			o.gradPos = v.vertex.xyz;
		#else
			o.gradPos = mul(_Object2World, half4(v.vertex.xyz, 1)).xyz;
		#endif 
	#endif


	//Lightmap
	#ifdef LIGHTMAP_ON
		o.uv.zw = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	#endif
	 

	//Shadows
	#if !defined(UNITY_PASS_SHADOWCASTER) && !defined(UNITY_PASS_SHADOWCOLLECTOR) 
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

		#if defined(UNITY_PASS_FORWARDBASE) || defined(PASS_FORWARD_ADD)
			TRANSFER_VERTEX_TO_FRAGMENT(o);
		#endif
	#endif

	#ifdef UNITY_PASS_SHADOWCASTER
		TRANSFER_SHADOW_CASTER(o)
	#endif

	#ifdef UNITY_PASS_SHADOWCOLLECTOR
		TRANSFER_SHADOW_COLLECTOR(o)
	#endif

	return o;
				
}

//////////////////////////////////////////////////////////////////////////////
//                                                                          // 
//Pixel Shaders                                                             //                
//                                                                          //               
//////////////////////////////////////////////////////////////////////////////
#if defined(UNITY_PASS_FORWARDBASE) || defined(UNITY_PASS_UNLIT)
fixed4 frag(vOutput i) : COLOR 
{			
	#if defined(V_WIRE_GRADIENT) && defined(V_WIRE_GRADIENT_PREVIEW_ON)
		return fixed4(V_WIRE_Gradient(i.gradPos).xxx, 1);
	#endif
	

	#ifdef V_WIRE_INSIDE_TANGENT_ON
		#ifdef V_WIRE_NO_COLOR_0
			fixed4 retColor = 0;
		#else
			#ifdef V_WIRE_NO_COLOR_1
				fixed4 retColor = 1;
			#else
				fixed4 retColor = i.vColor;
			#endif
		#endif
	#else
		fixed4 retColor = _Color;

		#ifdef V_WIRE_HAS_TEXTURE	
			half4 mainTex = tex2D(_MainTex, i.uv.xy);
			retColor *= mainTex;
		#endif
	#endif

	#if defined(LIGHTMAP_ON) && !defined(PASS_FORWARD_ADD)
		fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv.zw);
		fixed3 diff = WIRE_DecodeLightmap (lmtex);	
	#endif

	#ifndef LIGHTMAP_ON
		#ifdef UNITY_PASS_FORWARDBASE
			i.normal = normalize(i.normal);
			fixed3 diff = (_LightColor0.rgb * (max(0, dot (i.normal, _WorldSpaceLightPos0.xyz)) * LIGHT_ATTENUATION(i)) + UNITY_LIGHTMODEL_AMBIENT.xyz) * 2;
		#endif	
	#endif


	//IBL
	#ifdef UNITY_PASS_FORWARDBASE
		#ifdef V_WIRE_IBL_ON		
			diff += V_WIRE_IBL(i.normal);
			retColor.rgb = diff * retColor.rgb;
		#else
			retColor.rgb *= diff;
		#endif


		#ifdef V_WIRE_LIGHT_ON 
			_V_WIRE_Color.rgb *= diff;		
		
			#ifdef V_WIRE_GRADIENT
				_V_WIRE_GradientColor.rgb *= diff;
			#endif			
		#endif
	#endif
	#ifdef UNITY_PASS_UNLIT
		#ifdef V_WIRE_IBL_ON
			#ifdef LIGHTMAP_ON
				diff += V_WIRE_IBL(normalize(i.normal));
			#else
				fixed3 diff = UNITY_LIGHTMODEL_AMBIENT.xyz * 2 + V_WIRE_IBL(normalize(i.normal));
			#endif

			retColor.rgb = diff * retColor.rgb;

			#ifdef V_WIRE_LIGHT_ON 
				_V_WIRE_Color.rgb *= diff;		
		
				#ifdef V_WIRE_GRADIENT
					_V_WIRE_GradientColor.rgb *= diff;
				#endif			
			#endif
		#endif
	#endif


	//Reflection
	#ifdef V_WIRE_REFLECTION
		fixed4 reflTex = texCUBE( _Cube, i.refl );

		#ifdef V_WIRE_REFLECTION_COLOR
			reflTex.rgb *= _ReflectColor.rgb;
		#endif

		#ifdef V_WIRE_FRESNEL_REFLECTION_ON
			reflTex.rgb *= i.vfx.y;
		#endif

			
		#ifdef V_WIRE_INSIDE_TANGENT_ON
			retColor.rgb += reflTex.rgb * i.vColor.a;
		#else
			#ifdef V_WIRE_HAS_TEXTURE
				retColor.rgb += reflTex.rgb * mainTex.a;
			#else
				retColor.rgb += reflTex.rgb;
			#endif
		#endif
	#endif


	#ifdef V_WIRE_FRESNEL_ON
		_V_WIRE_Color.a *= i.vfx.x;
	#endif
	
	
	#ifndef V_WIRE_INSIDE_TANGENT_ON
		#ifdef V_WIRE_TRANSPARENCY_BASE_TEXTURE
			_V_WIRE_Color.a *= retColor.a;
		#endif
		#ifdef V_WIRE_TRANSPARENCY_BASE_TEXTURE_INVERT
			_V_WIRE_Color.a *= 1 - retColor.a;
		#endif
		#ifdef V_WIRE_TRANSPARENCY_CUSTOM_TEXTURE
			_V_WIRE_Color.a *= tex2D(_V_WIRE_Transparent_Tex, i.uv.xy).a;
		#endif
	#endif
	
	
		
	#ifdef V_WIRE_GRADIENT	
		Wire(retColor, i.mass, V_WIRE_Gradient(i.gradPos));
	#else
		Wire(retColor, i.mass, 1);
	#endif

	#ifdef V_WIRE_CUTOUT
		clip( retColor.a - _Cutoff );
	#endif


	return retColor;
} 
#endif //frag



//////////////////////////////////////////////////////////////////////////////
//                                                                          // 
//Shadows                                                                   //                
//                                                                          //               
//////////////////////////////////////////////////////////////////////////////
#ifdef UNITY_PASS_SHADOWCASTER
half4 frag_ShadowCaster(vOutput i) : COLOR 
{	
	#if defined(V_WIRE_CUTOUT)

		fixed4 retColor = tex2D(_MainTex, i.uv.xy) * _Color;
			
		#ifdef V_WIRE_TRANSPARENCY_BASE_TEXTURE
			_V_WIRE_Color.a *= retColor.a;
		#endif
		#ifdef V_WIRE_TRANSPARENCY_BASE_TEXTURE_INVERT
			_V_WIRE_Color.a *= 1 - retColor.a;
		#endif
		#ifdef V_WIRE_TRANSPARENCY_CUSTOM_TEXTURE
			_V_WIRE_Color.a *= tex2D(_V_WIRE_Transparent_Tex, i.uv.xy).a;
		#endif

		Wire(retColor, i.mass, 1);

			
		clip( retColor.a - _Cutoff );
		
	#endif

	SHADOW_CASTER_FRAGMENT(i)
}
#endif

#ifdef UNITY_PASS_SHADOWCOLLECTOR
half4 frag_ShadowCollector(vOutput i) : COLOR 
{
	#if defined(V_WIRE_CUTOUT)
		clip(tex2D(_MainTex, i.uv.xy).a * _Color.a - _Cutoff);
	#endif
	SHADOW_COLLECTOR_FRAGMENT(i)
}
#endif

#endif