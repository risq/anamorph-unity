// Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'

#ifndef VACUUM_FRAGMENTUM_DX11_CG_INCLUDED
#define VACUUM_FRAGMENTUM_DX11_CG_INCLUDED


#include "../cginc/FragmentumCG_Variables.cginc"
#include "../cginc/FragmentumCG_Functions.cginc"
#include "../cginc/FragmentumCG_Macros.cginc"


//************************************************************************
//Structures
//************************************************************************
struct Input   
{			
	float2 uv_MainTex;

	#ifdef V_FR_BUMPSPECULAR
		float2 BumpMapUV; 
	#endif

	#ifdef V_FR_REFLECTION
		float3 worldRefl; 	
	#endif

	#ifdef V_FR_REFLECTION_BUMPSPECULAR
		float2 BumpMapUV; 
		float3 worldRefl; 
		INTERNAL_DATA	
	#endif
	
	#if defined(V_FR_CUTOUT)
		float disAmount;
	#endif	
			 
};	 
 

//************************************************************************
//Verext 
//************************************************************************
void vert (inout appdata_full v, out Input o)
{
	UNITY_INITIALIZE_OUTPUT(Input, o);
			

	#if defined(V_FR_BUMPSPECULAR) || defined(V_FR_REFLECTION_BUMPSPECULAR)
		o.BumpMapUV = TRANSFORM_TEX (v.texcoord, _BumpMap);
	#endif
	
	#if defined(V_FR_REFLECTION) || defined(V_FR_REFLECTION_BUMPSPECULAR)
		float3 viewDir = WorldSpaceViewDir( v.vertex );
		float3 worldN = mul((float3x3)_Object2World, v.normal * 1.0);
		o.worldRefl = reflect( -viewDir, worldN );
	#endif

	
	#ifdef V_FR_EDITOR_OFF
	  
		//Get Index 
		int index = V_FR_INDEX; 

		#ifdef V_FR_GLOBAL_CONTROL
			V_FR_Fragmentum *= V_FR_Global_Control;
		#endif

		V_FR_DisplaceAmount *= 1.0;


		half fragmentArea = 1;
		//Fragment Factor
		#ifdef V_FR_FRAGMENT_TEXTURE_ON
			float4 fUV = float4(TRANSFORM_TEX (buf_UV[index], V_FR_FragTexture), 0, 0);
			fragmentArea = tex2Dlod(V_FR_FragTexture, fUV).r;

			#ifdef V_FR_TEXTURE_POW
				fragmentArea = pow(fragmentArea, V_FR_FragTexturePower);	
			#endif	
		#endif

		#ifdef V_FR_RANDOMIZE_FRAGMENTUM
			fragmentArea *= saturate(V_FR_Fragmentum * (1 + buf_RandomVector[index].w * V_FR_RandomizeFragmentum));
		#else
			fragmentArea *= V_FR_Fragmentum;
		#endif
		
		
		float fragmentFactor = 1;
		#if defined(V_FR_ACTIVATOR_PLANE)
			float4 worldPos = mul(_Object2World, float4(buf_Center[index], 1));
			float3 toP = worldPos - V_FR_ActivatorPlanePosition; 
			float planeMult = dot(normalize(V_FR_ActivatorPlaneNormal), (toP));
						
			planeMult += V_FR_DistanceToActivator;

			#ifdef V_FR_RANDOMIZE_DISTANCE_TO_ACTIVATOR
				planeMult += buf_RandomVector[index].w * V_FR_RandomizeDistanceToActivator;
			#endif		
			
			#ifdef V_FR_LOCK
				fragmentFactor = clamp(planeMult, 0, 1);
			#else 
				fragmentFactor = max(0, planeMult);	
			#endif
		#endif

		#if defined(V_FR_ACTIVATOR_SPHERE)
			float3 worldPos = mul(_Object2World, float4(buf_Center[index], 1)).xyz;
			float3 spherePos = V_FR_ActivatorSphereObject.xyz;
			float sphereRadius = V_FR_ActivatorSphereObject.w;

			float dist = distance(worldPos, spherePos) + V_FR_DistanceToActivator;	

			#ifdef V_FR_RANDOMIZE_DISTANCE_TO_ACTIVATOR
				dist += buf_RandomVector[index].w * V_FR_RandomizeDistanceToActivator;
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
			disAmount *= (1 + abs(buf_RandomVector[index].w) * V_FR_RandomizeDisplaceAmount);
		#endif
	
		//Scale fragment
		#ifdef V_FR_FRAGMENTS_SCALE
			v.vertex.xyz = ScaleFragment(v.vertex.xyz, saturate(fragmentArea * fragmentFactor), buf_RandomVector[index].w, buf_Center[index]);
		#endif

	

		//Fragment move direction		
		float3 dir = buf_Normal[index];	
		#ifdef V_FR_DISPLACE_DIRECTIONAL		
			dir = mul((float3x3)_World2Object, V_FR_DisplaceDirectionObjectPosition.xyz);
			dir = normalize(dir);
		#endif
		#ifdef V_FR_DISPLACE_RADIAL		
			half3 displaceObjectPosition = mul(_World2Object, half4(V_FR_DisplaceDirectionObjectPosition.xyz, 1)).xyz * 1.0;
			dir = normalize(displaceObjectPosition - buf_Center[index]);
						
			disAmount = min(distance(displaceObjectPosition, v.vertex), disAmount);
		#endif	

		
		//Direction noise
		#ifdef V_FR_RANDOMIZE_DISPLACE_DIRECTION
			dir = lerp(dir, buf_RandomVector[index].xyz, V_FR_RandomizeDisplaceDirection);
		#endif

		//Rotation noise
		#if defined(V_FR_RANDOMIZE_INITIAL_ROTATION)
			half angle = fragmentArea * fragmentFactor * V_FR_RandomizeInitialRotation * buf_RandomVector[index].w;
			v.vertex.xyz = V_FR_RotateArbitrary(v.vertex, buf_Center[index], buf_RandomVector[index].xyz, angle); 
		#endif
				
		v.vertex.xyz += dir * disAmount;  
				   
		//Rotate	
		#if defined(V_FR_ROTATE_FRAGMENT_NORMAL) || defined(V_FR_ROTATE_FRAGMENT_CENTER) || defined(V_FR_ROTATE_CUSTOM_POINT) || defined(V_FR_ROTATE_PARENT_ORIGIN)
			#ifdef V_FR_RANDOMIZE_ROTATION_SPEED				 	
				half theta = frac(_Time.y * (V_FR_RotateSpeed  + buf_RandomVector[index].w * V_FR_RandomizeRotationSpeed * V_FR_RotateSpeed) * min(fragmentArea * fragmentFactor, 1)) * _2PI;
			#else
				half theta = frac(_Time.y * V_FR_RotateSpeed * min(fragmentArea * fragmentFactor, 1)) * _2PI;
			#endif  
			 
			#ifdef V_FR_RANDOMIZE_ROTATION_TIME_OFFSET
				theta += buf_RandomVector[index].w * V_FR_RandomizeRotationTimeOffset * min(fragmentArea * fragmentFactor, 1);
			#endif 			
		#endif

		#ifdef V_FR_ROTATE_FRAGMENT_NORMAL						
			v.vertex.xyz = V_FR_RotateArbitrary(v.vertex, buf_Center[index], buf_Normal[index], theta);
		#endif

		#ifdef V_FR_ROTATE_FRAGMENT_CENTER		
			v.vertex.xyz = V_FR_RotateArbitrary(v.vertex, buf_Center[index], buf_RandomVector[index].xyz, theta);
		#endif	
		
		#ifdef V_FR_ROTATE_CUSTOM_POINT		
			v.vertex.xyz = V_FR_RotateArbitrary(v.vertex, mul(_World2Object, half4(V_FR_RotateCustomPointPosition.xyz, 1)).xyz * 1.0, mul(_World2Object, half4(V_FR_RotateCustomPointNormal.xyz, 0)).xyz, theta);
		#endif				

		#ifdef V_FR_ROTATE_PARENT_ORIGIN			
			v.vertex.xyz = V_FR_RotateOrigin(v.vertex, buf_RandomVector[index].xyz, theta);			
		#endif

		

		#ifdef V_FR_CUTOUT
			o.disAmount = fragmentArea * fragmentFactor;
		#endif
				


				
		v.texcoord1 = float4(buf_UV2[V_FR_INDEX_LM], 0, 0);
	
	#else //V_FR_EDITOR_ON

		#ifdef V_FR_CUTOUT
			float4 fUV = float4(TRANSFORM_TEX (v.texcoord, V_FR_FragTexture), 0, 0);
			fixed fragmentArea = tex2Dlod(V_FR_FragTexture, fUV);

			#ifdef V_FR_TEXTURE_POW
				fragmentArea = pow(fragmentArea, V_FR_FragTexturePower);	
			#endif

			fragmentArea *= V_FR_Fragmentum;

			o.disAmount = fragmentArea * V_FR_Fragmentum;
		#endif

	#endif  	     
}

//Vertex

void surf (Input IN, inout SurfaceOutput o) 
{
	fixed4 mainTex = tex2D(_MainTex, IN.uv_MainTex);	

	#if defined(V_FR_CUTOUT)
	 	clip(mainTex.a - _Cutoff * IN.disAmount);
	#endif

	#ifdef V_FR_MAIN_COLOR
		mainTex.rgb *= _Color.rgb;
	#endif
	
	o.Albedo = mainTex.rgb;	
	

		
	#if defined(V_FR_BUMPSPECULAR) || defined(V_FR_REFLECTION_BUMPSPECULAR)
		o.Gloss = mainTex.a;
		o.Specular = _Shininess;			
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.BumpMapUV));
	#endif

	#if defined(V_FR_REFLECTION) || defined(V_FR_REFLECTION_BUMPSPECULAR)
		fixed4 reflcol;
		
		#ifdef V_FR_REFLECTION
			reflcol = texCUBE (_Cube, IN.worldRefl);
		#endif

		#ifdef V_FR_REFLECTION_BUMPSPECULAR
			float3 worldRefl = WorldReflectionVector (IN, o.Normal);
			reflcol = texCUBE (_Cube, worldRefl);
		#endif

		#ifdef V_FR_REFLECTION_COLOR
			reflcol.rgb *= _ReflectColor.rgb  * _ReflectColor.a;
		#endif

		o.Emission = reflcol * mainTex.a;
	#endif
	



	#ifdef V_FR_MAIN_COLOR
		mainTex.a *= _Color.a;
	#endif

	o.Alpha = mainTex.a;

}

#endif