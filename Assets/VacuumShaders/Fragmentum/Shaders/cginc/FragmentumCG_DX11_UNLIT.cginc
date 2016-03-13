// Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D

#ifndef VACUUM_FRAGMENTUM_CG_UNLIT_INCLUDED
#define VACUUM_FRAGMENTUM_CG_UNLIT_INCLUDED

//#include "UnityCG.cginc"
#include "../cginc/FragmentumCG_Variables.cginc"
#include "../cginc/FragmentumCG_Functions.cginc"
#include "../cginc/FragmentumCG_Macros.cginc"



//************************************************************************
//Structures
//************************************************************************
struct v_data
{
	float4 vertex    : POSITION;
    float3 normal    : NORMAL;
    float4 tangent   : TANGENT;
	float2 texcoord  : TEXCOORD0;
	float2 texcoord1 : TEXCOORD1;
};

struct GS_INPUT 
{
	float4 pos      : SV_POSITION;
	float3 normal   : NORMAL;
    float2 uvMain 	: TEXCOORD0; 
	float2 info     : TEXCOORD2;

	float2 uvLM : TEXCOORD3;
}; 

struct FS_INPUT
{
	float4	pos		: POSITION;
	float2 uvMain 	: TEXCOORD0; 

	float2 uvLM : TEXCOORD3;

};

struct v2f 
{
    float4 pos    : SV_POSITION; 
    float2 uvMain : TEXCOORD0;
	
	#ifdef LIGHTMAP_ON
		float2 uvLM : TEXCOORD3;
	#endif

	float3 I : TEXCOORD4;
};  

//Structures
 
	
//************************************************************************
//Verext 
//************************************************************************

v2f vertS (v_data v)
{
    v2f o;

	#ifdef LIGHTMAP_ON
		o.uvLM = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
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
				

		#ifdef LIGHTMAP_ON
			o.uvLM = buf_UV2[V_FR_INDEX_LM] * unity_LightmapST.xy + unity_LightmapST.zw;
		#endif
	
	#endif //V_FR_EDITOR_OFF  	 	
 	 	

	o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
	o.uvMain = TRANSFORM_TEX( v.texcoord, _MainTex);


	#if defined(V_FR_UNLIT_REFLECT)
		float3 viewDir = WorldSpaceViewDir( v.vertex );
		float3 worldN = mul((float3x3)_Object2World, v.normal * 1.0);
		o.I = reflect( -viewDir, worldN );
	#endif

          
    return o;
}

GS_INPUT vertSE (v_data v)
{
    GS_INPUT output = (GS_INPUT)0;

	#ifdef LIGHTMAP_ON
		output.uvLM = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
		output.info = float2(0, 0);
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

		#ifdef LIGHTMAP_ON
			output.uvLM = buf_UV2[V_FR_INDEX_LM] * unity_LightmapST.xy + unity_LightmapST.zw;
		#endif
		
		//x - fragmentArea
		output.info = float2(fragmentArea * fragmentFactor, 0);
	
	#endif //V_FR_EDITOR_OFF  	


	//Store position
	output.pos = v.vertex;	

	//Normal
	output.normal = v.normal;
								  
	//UV
	output.uvMain = TRANSFORM_TEX( v.texcoord, _MainTex);
	
	return output;
}
//Vertex


//************************************************************************
//Geometry 
//************************************************************************
#ifdef V_FR_EXTRUDE
[maxvertexcount(24)]
void geomSE(triangle GS_INPUT input[3], inout TriangleStream<FS_INPUT> triStream)
{
	FS_INPUT output;
	//
    // Calculate the face normal
    //
    float3 faceEdgeA = input[1].pos - input[0].pos;
    float3 faceEdgeB = input[2].pos - input[0].pos;
    float3 faceNormal = normalize( cross(faceEdgeA, faceEdgeB) );

	float3 dirV = faceNormal  * V_FR_ExtrudeAmount;

	//
    // Calculate the face center
    //
    float3 centerPos = (input[0].pos.xyz + input[1].pos.xyz + input[2].pos.xyz)/3.0;
    centerPos += dirV;

	float4 p[6];
	p[0] = mul(UNITY_MATRIX_MVP, input[0].pos);
	p[1] = mul(UNITY_MATRIX_MVP, input[1].pos);
	p[2] = mul(UNITY_MATRIX_MVP, input[2].pos);

	p[3] = mul(UNITY_MATRIX_MVP, (input[0].pos + float4(dirV, 0) * input[0].info.x));
	p[4] = mul(UNITY_MATRIX_MVP, (input[1].pos + float4(dirV, 0) * input[1].info.x));
	p[5] = mul(UNITY_MATRIX_MVP, (input[2].pos + float4(dirV, 0) * input[2].info.x));

	//
    // Output the pyramid
    //
	
	//1///////////////////////////////////////	
	output.pos = p[0];
	output.uvMain = input[0].uvMain;
	output.uvLM = input[0].uvLM;			
    triStream.Append( output );

	output.pos = p[1];
	output.uvMain = input[1].uvMain;
	output.uvLM = input[1].uvLM;	
    triStream.Append( output );

	output.pos = p[2];
	output.uvMain = input[2].uvMain;
	output.uvLM = input[2].uvLM;
	triStream.Append( output );

	triStream.RestartStrip();

	//2///////////////////////////////////////	
	output.pos = p[3];
	output.uvMain = input[0].uvMain;
	output.uvLM = input[0].uvLM;	
    triStream.Append( output );

	output.pos = p[5];
	output.uvMain = input[2].uvMain;
	output.uvLM = input[2].uvLM;	
    triStream.Append( output );

	output.pos = p[4];
	output.uvMain = input[1].uvMain;
	output.uvLM = input[1].uvLM;	
    triStream.Append( output );

	triStream.RestartStrip();

	//3///////////////////////////////////////	
	output.pos = p[0];
	output.uvMain = input[0].uvMain;
	output.uvLM = input[0].uvLM;	
    triStream.Append( output );

	output.pos = p[2];
	output.uvMain = input[2].uvMain;  
	output.uvLM = input[2].uvLM;	
    triStream.Append( output );

	output.pos = p[3];
	output.uvMain = input[1].uvMain;	
	output.uvLM = input[1].uvLM;	
    triStream.Append( output );

	triStream.RestartStrip();

	//4///////////////////////////////////////	
	output.pos = p[2];
	output.uvMain = input[2].uvMain;	
	output.uvLM = input[2].uvLM;	
    triStream.Append( output );

	output.pos = p[3];
	output.uvMain = input[1].uvMain;	
	output.uvLM = input[1].uvLM;	
    triStream.Append( output );

	output.pos = p[5];
	output.uvMain = input[0].uvMain;	
	output.uvLM = input[0].uvLM;	
    triStream.Append( output );

	triStream.RestartStrip();

	//5///////////////////////////////////////	
	output.pos = p[2];
	output.uvMain = input[2].uvMain;
	output.uvLM = input[2].uvLM;	
    triStream.Append( output );

	output.pos = p[5];
	output.uvMain = input[0].uvMain;
	output.uvLM = input[0].uvLM;	
    triStream.Append( output );

	output.pos = p[4];
	output.uvMain = input[1].uvMain;
	output.uvLM = input[1].uvLM;	
    triStream.Append( output );

	triStream.RestartStrip();

	//6///////////////////////////////////////	
	output.pos = p[2];
	output.uvMain = input[2].uvMain;
	output.uvLM = input[2].uvLM;	
    triStream.Append( output );

	output.pos = p[4];
	output.uvMain = input[1].uvMain;
	output.uvLM = input[1].uvLM;	
    triStream.Append( output );

	output.pos = p[1];
	output.uvMain = input[0].uvMain;
	output.uvLM = input[0].uvLM;	
    triStream.Append( output );

	triStream.RestartStrip();

	//7///////////////////////////////////////	
	output.pos = p[0];
	output.uvMain = input[2].uvMain;
	output.uvLM = input[2].uvLM;	
    triStream.Append( output );

	output.pos = p[3];
	output.uvMain = input[0].uvMain;
	output.uvLM = input[0].uvLM;	
    triStream.Append( output );

	output.pos = p[1];
	output.uvMain = input[1].uvMain;
	output.uvLM = input[1].uvLM;	
    triStream.Append( output );

	triStream.RestartStrip();

	//8///////////////////////////////////////	
	output.pos = p[1];
	output.uvMain = input[1].uvMain;
	output.uvLM = input[1].uvLM;	
    triStream.Append( output );

	output.pos = p[3];
	output.uvMain = input[0].uvMain;
	output.uvLM = input[0].uvLM;	
    triStream.Append( output );

	output.pos = p[4];
	output.uvMain = input[2].uvMain;
	output.uvLM = input[2].uvLM;	
    triStream.Append( output );

	triStream.RestartStrip();

}
#endif
//Geometry 


//************************************************************************
//Fragment 
//************************************************************************
fixed4 fragS (v2f i) : COLOR0
{ 	

	half4 tex = tex2D(_MainTex, i.uvMain); 

	#ifdef V_FR_MAIN_COLOR
		tex *= _Color;	
	#endif

			

	#ifdef LIGHTMAP_ON
		fixed3 lm = ( FR_DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM)));
		tex.rgb *= lm;
	#endif

	#if defined(V_FR_UNLIT_REFLECT)
		fixed3 reflTex = texCUBE( _Cube, i.I ).rgb;
		reflTex *= _ReflectColor.rgb;		

		tex.rgb += reflTex * tex.a;	
	#endif

	
	return tex;
}

fixed4 fragSE (FS_INPUT i) : COLOR0
{
	half4 tex = tex2D(_MainTex, i.uvMain); 

	#ifdef V_FR_MAIN_COLOR
		tex *= _Color;
	#endif


	#ifdef LIGHTMAP_ON
		fixed3 lm = ( FR_DecodeLightmap (UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uvLM)));
		tex.rgb *= lm;
	#endif


	return tex; 		
}

#endif