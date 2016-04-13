// Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

#ifndef VACUUM_FRAGMENTUM_CG_MOBILE_INCLUDED
#define VACUUM_FRAGMENTUM_CG_MOBILE_INCLUDED

#include "../cginc/FragmentumCG_Variables.cginc"
#include "../cginc/FragmentumCG_Functions.cginc"
#include "../cginc/FragmentumCG_Macros.cginc"


//************************************************************************
//Structures
//************************************************************************
#ifndef V_FR_SURFACE
struct vInput
{
    half4 vertex    : POSITION;
	half2 texcoord  : TEXCOORD0;

	#ifdef LIGHTMAP_ON
		half4 texcoord1 :TEXCOORD1;
	#endif

	half3 normal    : NORMAL;
	half4 tangent   : TANGENT;	

	fixed4 color : COLOR;
};

struct vOutput
{
	#if defined(PASS_SHADOW_COLLECTOR) || defined(PASS_SHADOW_CASTER)

		#ifdef PASS_SHADOW_COLLECTOR
			V2F_SHADOW_COLLECTOR;
		#endif

		#ifdef PASS_SHADOW_CASTER
			V2F_SHADOW_CASTER;
		#endif

		half2 tex :TEXCOORD5;

		#ifdef V_FR_CUTOUT
			half disAmount : TEXCOORD6;
		#endif
	#else

		half4 pos :SV_POSITION;
		half2 tex :TEXCOORD0;

		#ifdef LIGHTMAP_ON
			half2 lmap : TEXCOORD1;
		#endif

		#ifdef V_FR_REFLECTION
			half3 refl : TEXCOORD2;
		#else
			#ifdef V_FR_CUTOUT
				half disAmount : TEXCOORD2;
			#endif	
		#endif


		#if defined(PASS_FORWARD_BASE) || defined(PASS_FORWARD_ADD)
			#ifndef V_FR_CALC_LIGHT_PER_VERTEX
				half3 normal: TEXCOORD3;
				half3 lightDir: TEXCOORD4;
			#endif

			LIGHTING_COORDS(5,6)
		#endif
	#endif

	#ifdef V_FR_CALC_LIGHT_PER_VERTEX
		fixed4 color : COLOR;
	#endif

};
#endif

#ifdef V_FR_SURFACE
	struct Input 
	{
		half2 uv_MainTex;

		#ifdef V_FR_REFLECTION
			half3 refl;
		#endif

		#ifdef V_FR_CUTOUT
			half disAmount;
		#endif

		#ifdef V_FR_CALC_LIGHT_PER_VERTEX
			fixed4 color;
		#endif
	};
#endif

//************************************************************************
//Vertex Shader
//************************************************************************
#ifdef V_FR_SURFACE 
void vert (inout appdata_full v, out Input o)
#else
vOutput vert(vInput v)
#endif
{ 
	#ifdef V_FR_SURFACE
		UNITY_INITIALIZE_OUTPUT(Input, o);
	#else
		vOutput o;
	#endif


	 
	#ifdef V_FR_EDITOR_OFF		
		 
		#ifdef SM_MOBILE
			half3 cNormal = v.normal;
		#else
			half3 cNormal = floor(v.normal) * 0.001f;
			half3 vNormal = (-1 + frac(v.normal) * 20.0f);
		#endif
				
		#ifdef SHADER_API_GLES
			v.tangent.xyz *= v.tangent.w;
		#endif

		#ifdef V_FR_SURFACE
			v.normal = vNormal;
		#endif
		
		#if defined(V_FR_RANDOMIZE_DISPLACE_DIRECTION) || defined(V_FR_RANDOMIZE_INITIAL_ROTATION) || defined(V_FR_ROTATE_FRAGMENT_NORMAL) || defined(V_FR_ROTATE_FRAGMENT_CENTER) || defined(V_FR_ROTATE_PARENT_ORIGIN)
			half3 rNormal = normalize(-1 + v.color.xyz * 2);
		#endif
		
		#ifdef V_FR_GLOBAL_CONTROL
			V_FR_Fragmentum *= V_FR_Global_Control;
		#endif

		V_FR_DisplaceAmount *= 1.0;

		half fragmentArea = 1;
		
		//Fragment Factor
		#ifdef V_FR_FRAGMENT_TEXTURE_ON
			half2 cUV = floor(v.texcoord.xy) * 0.001;
			half4 fUV = half4(TRANSFORM_TEX (cUV, V_FR_FragTexture), 0, 0);
			fragmentArea = tex2Dlod(V_FR_FragTexture, fUV).r;

			#ifdef V_FR_TEXTURE_POW
				fragmentArea = pow(fragmentArea, V_FR_FragTexturePower);	
			#endif
		#endif

		#ifdef V_FR_RANDOMIZE_FRAGMENTUM
			fragmentArea *= saturate(V_FR_Fragmentum * (1 + v.color.w * V_FR_RandomizeFragmentum));
		#else
			fragmentArea *= V_FR_Fragmentum;
		#endif
		
		half fragmentFactor = 1;
		#ifdef V_FR_ACTIVATOR_PLANE
			half4 worldPos = mul(_Object2World, half4(v.tangent.xyz, 1));
			half3 toP = (worldPos - V_FR_ActivatorPlanePosition).xyz; 
			half planeMult = dot(normalize(V_FR_ActivatorPlaneNormal), (toP));
			
			planeMult += V_FR_DistanceToActivator;
		
			#ifdef V_FR_RANDOMIZE_DISTANCE_TO_ACTIVATOR
				planeMult += v.color.w * V_FR_RandomizeDistanceToActivator;
			#endif		
			
			#ifdef V_FR_LOCK
				fragmentFactor = clamp(planeMult, 0, 1);
			#else 
				fragmentFactor = max(0, planeMult);	
			#endif
		#endif

		#ifdef V_FR_ACTIVATOR_SPHERE
			half3 worldPos = mul(_Object2World, half4(v.tangent.xyz, 1)).xyz;
			half3 spherePos = V_FR_ActivatorSphereObject.xyz;
			half sphereRadius = V_FR_ActivatorSphereObject.w;

			half dist = distance(worldPos, spherePos) + V_FR_DistanceToActivator;	

			#ifdef V_FR_RANDOMIZE_DISTANCE_TO_ACTIVATOR
				dist += v.color.w * V_FR_RandomizeDistanceToActivator;
			#endif

			if(sphereRadius > 0)
			{
				dist = min(dist, sphereRadius);
				fragmentFactor = -dist + sphereRadius;	
			}
			else
			{
				dist = max(0, dist - abs(sphereRadius));
				fragmentFactor = dist;
			}				
			
			#ifdef V_FR_LOCK
				fragmentFactor = min(fragmentFactor, 1);
			#endif					
		#endif


		//Update displace amount;
		half disAmount = V_FR_DisplaceAmount * fragmentArea * fragmentFactor;
				
		#ifdef V_FR_RANDOMIZE_DISPLACE_AMOUNT
			disAmount *= (1 + abs(v.color.w) * V_FR_RandomizeDisplaceAmount);
		#endif

		//Scale fragment
		#ifdef V_FR_FRAGMENTS_SCALE 
			v.vertex.xyz = ScaleFragment(v.vertex.xyz, saturate(fragmentArea * fragmentFactor), v.color.w, v.tangent.xyz);
		#endif


		//Fragment move direction		
		half3 dir = cNormal;
		#if defined(V_FR_DISPLACE_DIRECTIONAL)		
			dir = mul((half3x3)_World2Object, V_FR_DisplaceDirectionObjectPosition.xyz);
			dir = normalize(dir);
		#endif
		#if defined(V_FR_DISPLACE_RADIAL)
			half3 displaceObjectPosition = mul(_World2Object, half4(V_FR_DisplaceDirectionObjectPosition.xyz, 1)).xyz * 1.0;
			dir = normalize(displaceObjectPosition - v.tangent.xyz);
						
			disAmount = min(distance(displaceObjectPosition, v.vertex), disAmount);
		#endif

		
		//Direction noise
		#ifdef V_FR_RANDOMIZE_DISPLACE_DIRECTION
			dir = lerp(dir, rNormal, V_FR_RandomizeDisplaceDirection);
		#endif

		//Rotation noise
		#ifdef V_FR_RANDOMIZE_INITIAL_ROTATION
			half angle = fragmentArea * fragmentFactor * V_FR_RandomizeInitialRotation * v.color.w;
			v.vertex.xyz = V_FR_RotateArbitrary(v.vertex, v.tangent.xyz, rNormal, angle); 
				
			#if defined(V_FR_REFLECTION) || defined(PASS_FORWARD_BASE)
				vNormal = lerp(vNormal, rNormal, angle);
				vNormal = normalize(vNormal);
			#endif
		#endif
				
		
		
		//Rotate
		#if defined(V_FR_ROTATE_FRAGMENT_NORMAL) || defined(V_FR_ROTATE_FRAGMENT_CENTER) || defined(V_FR_ROTATE_CUSTOM_POINT) || defined(V_FR_ROTATE_PARENT_ORIGIN)
			#ifdef V_FR_RANDOMIZE_ROTATION_SPEED				 	
				half theta = frac(_Time.y * (V_FR_RotateSpeed  + v.color.w * V_FR_RandomizeRotationSpeed * V_FR_RotateSpeed) * min(fragmentArea * fragmentFactor, 1)) * _2PI;
			#else
				half theta = frac(_Time.y * V_FR_RotateSpeed * min(fragmentArea * fragmentFactor, 1)) * _2PI;
			#endif  
			 
			#ifdef V_FR_RANDOMIZE_ROTATION_TIME_OFFSET
				theta += v.color.w * V_FR_RandomizeRotationTimeOffset * min(fragmentArea * fragmentFactor, 1);
			#endif 	
			
			#ifdef V_FR_ROTATE_AFTER_DISPLACE
				v.vertex.xyz += dir * disAmount;	
			#endif	
		#else

			//There is no rotation. Do standart displace.
			v.vertex.xyz += dir * disAmount;	

		#endif

		#ifdef V_FR_ROTATE_FRAGMENT_NORMAL						
			v.vertex.xyz = V_FR_RotateArbitrary(v.vertex, v.tangent.xyz, normalize(cNormal), theta);
			
			#ifndef V_FR_ROTATE_AFTER_DISPLACE
				v.vertex.xyz += dir * disAmount;
			#endif
		#endif

		#ifdef V_FR_ROTATE_FRAGMENT_CENTER		
			v.vertex.xyz = V_FR_RotateArbitrary(v.vertex, v.tangent.xyz, rNormal, theta);
			
			#ifndef V_FR_ROTATE_AFTER_DISPLACE
				v.vertex.xyz += dir * disAmount;
			#endif
		#endif	
		
		#ifdef V_FR_ROTATE_CUSTOM_POINT		
			v.vertex.xyz = V_FR_RotateArbitrary(v.vertex, mul(_World2Object, half4(V_FR_RotateCustomPointPosition.xyz, 1)).xyz * 1.0, mul(_World2Object, half4(V_FR_RotateCustomPointNormal.xyz, 0)).xyz, theta);
			
			#ifndef V_FR_ROTATE_AFTER_DISPLACE
				v.vertex.xyz += dir * disAmount;
			#endif
		#endif				

		#ifdef V_FR_ROTATE_PARENT_ORIGIN			
			v.vertex.xyz = V_FR_RotateOrigin(v.vertex, normalize(rNormal), theta);		
			
			#ifndef V_FR_ROTATE_AFTER_DISPLACE
				v.vertex.xyz += dir * disAmount;
			#endif	
		#endif
		
  	
		

		
		#ifdef V_FR_REFLECTION
			half3 viewDir = _WorldSpaceCameraPos.xyz - mul(_Object2World, v.vertex).xyz;
			half3 worldN = mul((half3x3)_Object2World, vNormal * 1.0);

			o.refl = reflect( -viewDir, worldN );
		#endif

		#ifdef PASS_FORWARD_BASE
			#ifdef V_FR_CALC_LIGHT_PER_VERTEX
				half3 normal = mul((half3x3)_Object2World, vNormal * 1.0);
				o.color = fixed4(max(0, dot (normalize(normal), _WorldSpaceLightPos0.xyz)) * _LightColor0.rgb, 0);
			#else
				o.normal = mul((half3x3)_Object2World, vNormal * 1.0);
				o.lightDir = _WorldSpaceLightPos0.xyz;
			#endif
		#endif

		#ifdef PASS_FORWARD_ADD
			#ifdef V_FR_CALC_LIGHT_PER_VERTEX
				half3 normal = mul((half3x3)_Object2World, vNormal * 1.0);
				o.color = fixed4(max(0, dot (normalize(normal), (normalize(_WorldSpaceLightPos0.xyz - mul(_Object2World, v.vertex).xyz)))) * _LightColor0.rgb, 0);
			#else
				o.normal = mul((half3x3)_Object2World, vNormal * 1.0);
				o.lightDir = _WorldSpaceLightPos0.xyz - mul(_Object2World, v.vertex).xyz;
			#endif
		#endif 

		#ifdef V_FR_SURFACE
			v.texcoord.xy = frac(v.texcoord.xy) * 10;
		#else
			o.tex = frac(v.texcoord.xy) * 10 * _MainTex_ST.xy + _MainTex_ST.zw; 
		#endif

		#ifdef V_FR_CUTOUT
			o.disAmount = fragmentArea * fragmentFactor;
		#endif
	#else 

		#ifndef V_FR_SURFACE
			o.tex = v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
		#endif

		#ifdef V_FR_CUTOUT
			o.disAmount = 1;
		#endif
	
		#ifdef V_FR_REFLECTION
			half3 viewDir = _WorldSpaceCameraPos.xyz - mul(_Object2World, v.vertex).xyz;
			half3 worldN = mul((half3x3)_Object2World, v.normal * 1.0);
			o.refl = reflect( -viewDir, worldN );
		#endif

		#ifdef PASS_FORWARD_BASE
			#ifdef V_FR_CALC_LIGHT_PER_VERTEX
				half3 normal = mul((half3x3)_Object2World, v.normal * 1.0);
				o.color = fixed4(max(0, dot (normalize(normal), _WorldSpaceLightPos0.xyz)) * _LightColor0.rgb, 0);
			#else
				o.normal = mul((half3x3)_Object2World, v.normal * 1.0);
				o.lightDir = _WorldSpaceLightPos0.xyz;
			#endif
		#endif

		#ifdef PASS_FORWARD_ADD
			#ifdef V_FR_CALC_LIGHT_PER_VERTEX
				half3 normal = mul((half3x3)_Object2World, v.normal * 1.0);
				o.color = fixed4(max(0, dot (normalize(normal), normalize(_WorldSpaceLightPos0.xyz - mul(_Object2World, v.vertex).xyz))) * _LightColor0.rgb, 0);
			#else
				o.normal = mul((half3x3)_Object2World, v.normal * 1.0);
				o.lightDir = _WorldSpaceLightPos0.xyz - mul(_Object2World, v.vertex).xyz;
			#endif
		#endif 
	#endif
	

	//Lightmap
	#ifndef  V_FR_SURFACE
		#ifdef LIGHTMAP_ON
			o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		#endif
	 

		//Shadows
		#if !defined(PASS_SHADOW_CASTER) && !defined(PASS_SHADOW_COLLECTOR) 
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);

			#if defined(PASS_FORWARD_BASE) || defined(PASS_FORWARD_ADD)
				TRANSFER_VERTEX_TO_FRAGMENT(o);
			#endif
		#endif

		#ifdef PASS_SHADOW_CASTER
			TRANSFER_SHADOW_CASTER(o)
		#endif

		#ifdef PASS_SHADOW_COLLECTOR
			TRANSFER_SHADOW_COLLECTOR(o)
		#endif

		return o;
	#endif	
} 


//************************************************************************
//Fragment Shader
//************************************************************************
#if !defined(PASS_SHADOW_COLLECTOR) && !defined(PASS_SHADOW_CASTER) && !defined(V_FR_SURFACE)
half4 frag(vOutput i) : COLOR 
{
    fixed4 mainTex = tex2D(_MainTex, i.tex);


	#if defined(V_FR_CUTOUT)
	 	clip(mainTex.a - _Cutoff * i.disAmount);
	#endif
	
	
	#if defined(LIGHTMAP_ON) && !defined(PASS_FORWARD_ADD)
		fixed4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lmap.xy);
		fixed3 lm = FR_DecodeLightmap (lmtex);

		mainTex.rgb *= lm;
	#endif


	#ifndef LIGHTMAP_ON
		#ifdef PASS_FORWARD_BASE
			#ifdef V_FR_CALC_LIGHT_PER_VERTEX
				mainTex.rgb *= ((i.color.rgb * LIGHT_ATTENUATION(i)) + UNITY_LIGHTMODEL_AMBIENT.xyz) * 2;
			#else
				fixed diff = max(0, dot (normalize(i.normal), normalize(i.lightDir)));  
				mainTex.rgb *= (_LightColor0.rgb * (diff * LIGHT_ATTENUATION(i)) + UNITY_LIGHTMODEL_AMBIENT.xyz) * 2;
			#endif
		#endif
		#ifdef PASS_FORWARD_ADD
			#ifdef V_FR_CALC_LIGHT_PER_VERTEX
				mainTex.rgb *= i.color.rgb * LIGHT_ATTENUATION(i) * 2;
			#else
				fixed diff = max(0, dot (normalize(i.normal), normalize(i.lightDir)));      
				mainTex.rgb *= _LightColor0.rgb * (diff * LIGHT_ATTENUATION(i) * 2);
			#endif
		#endif
	#endif

	//Reflection
	#ifdef V_FR_REFLECTION
		fixed4 reflTex = texCUBE( _Cube, i.refl );

		#ifdef V_FR_REFLECTION_COLOR
			reflTex.rgb *= _ReflectColor.rgb  * _ReflectColor.a;
		#endif

		mainTex.rgb += reflTex.rgb * mainTex.a;	
	#endif

	 
	#ifdef V_FR_MAIN_COLOR
		return mainTex * _Color;
	#else
		return mainTex;
	#endif
} 
#endif

#ifdef PASS_SHADOW_CASTER
half4 frag_ShadowCaster(vOutput i) : COLOR 
{
	#if defined(V_FR_CUTOUT)
		clip(tex2D(_MainTex, i.tex).a - _Cutoff * i.disAmount);
		//clip(1 - i.disAmount * tex2D(_MainTex, i.tex).a - _Cutoff);
	#endif
	SHADOW_CASTER_FRAGMENT(i)
}
#endif

#ifdef PASS_SHADOW_COLLECTOR
half4 frag_ShadowCollector(vOutput i) : COLOR 
{
	#if defined(V_FR_CUTOUT)
		clip(tex2D(_MainTex, i.tex).a - _Cutoff * i.disAmount);
		//clip(1 - i.disAmount * tex2D(_MainTex, i.tex).a - _Cutoff);
	#endif
	SHADOW_COLLECTOR_FRAGMENT(i)
}
#endif


#endif