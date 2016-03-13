// VacuumShaders 2014
// https://www.facebook.com/VacuumShaders

Shader "VacuumShaders/The Amazing Wireframe/(Preview) Color and Tangent"
{ 
    Properties 
    {
		//Tag
		[Tag]
		V_WIRE_TAG("", float) = 0

		[Title]
		_V_WIRE_Title_DO("Default Options", float) = 0

		_Color("Main Color (RGB)", color) = (1, 1, 1, 1)
		_MainTex("Base (RGB)", 2D) = "white"{}
		

		[Title]
		_V_WIRE_Title_DO("Wire Options", float) = 0

		[KeywordEnum(Color, Tangent)] V_WIRE_IN ("Using Buffer", Float) = 0
		[MaterialToggle(V_WIRE_ANTIALIASING_ON)] AO ("Antialiasing (SM3)", Float) = 0

		_V_WIRE_Color("Wire Color (RGB) Trans (A)", color) = (0, 0, 0, 1)	
		_V_WIRE_Size("Wire Size", Range(0, 0.5)) = 0.05
		
    }

    SubShader 
    {
		Tags { "RenderType"="Opaque" 
			   "WireframeTag"="(Preview) Color and Tangent"
			   "WireframeBakedTag"=""
			 }

		Pass
	    {			  
		 
            CGPROGRAM 
		    #pragma vertex vert
	    	#pragma fragment frag
	    	#pragma fragmentoption ARB_precision_hint_fastest		 
			 
			#pragma multi_compile V_WIRE_ANTIALIASING_OFF V_WIRE_ANTIALIASING_ON
			#pragma multi_compile V_WIRE_IN_COLOR V_WIRE_IN_TANGENT
					
			
			#ifdef V_WIRE_ANTIALIASING_ON
				#pragma target 3.0
				#pragma glsl
			#endif

			fixed4 _V_WIRE_Color;
			half _V_WIRE_Size;	

			fixed4 _Color;
			sampler2D _MainTex;
			half4 _MainTex_ST;


			struct vInput
			{
				float4 vertex : POSITION;
				half4 texcoord : TEXCOORD0;
				
				#ifdef V_WIRE_IN_COLOR
					fixed4 color : COLOR;
				#else
					float4 tangent : TANGENT;
					fixed4 color : COLOR;
				#endif
			};

			struct vOutput
			{
				float4 pos :SV_POSITION;
				half3 uv : TEXCOORD0;

				fixed4 mass : TEXCOORD1;	
				fixed4 color : COLOR;			
			};

			vOutput vert(vInput v)
			{
				vOutput o;

				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = half3(v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw, 0);

				#ifdef V_WIRE_IN_COLOR
					o.mass = v.color;
				#else
					o.mass = v.tangent;
					o.color = v.color;
				#endif

				return o;
			}

			fixed4 frag(vOutput i) : SV_Target 
			{	
				#ifdef V_WIRE_IN_COLOR
					fixed4 retColor = tex2D(_MainTex, i.uv.xy) * _Color;
				#else
					fixed4 retColor = i.color;
				#endif

				 
				#ifdef V_WIRE_ANTIALIASING_ON
					half3 width = abs(ddx(i.mass.xyz)) + abs(ddy(i.mass.xyz));
					half3 eF = smoothstep(half3(0, 0, 0), width * _V_WIRE_Size * 20, i.mass.xyz);		
	
					half value = min(min(eF.x, eF.y), eF.z);	
				#else
					half value = step(_V_WIRE_Size, min(min(i.mass.x, i.mass.y), i.mass.z));
				#endif
				

				return lerp(_V_WIRE_Color, retColor, value);
			}


			ENDCG 

    	} //Pass
			
        
    } //SubShader


} //Shader
