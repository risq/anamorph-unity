Shader "VertexAnimation/VertexAnimationSpecular" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
    	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_Ratio ("Ratio", Range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma glsl_no_auto_normalization
		#pragma surface surf SimpleSpecular vertex:vert

		sampler2D _MainTex;
		float _Ratio;
		float _Shininess;

		struct Input {
			float2 uv_MainTex;
		};
		
		void vert (inout appdata_full v)
		{
			float invRatio = (1.0 - _Ratio);
			v.vertex.xyz = (1.0 - _Ratio) * v.vertex.xyz + _Ratio * v.tangent.xyz;
			float3 nrm2 = v.color.xyz * 2.0 - 1.0;
			v.normal = normalize(invRatio * v.normal + _Ratio * nrm2);
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Specular = _Shininess;
		}
		
		inline fixed4 LightingSimpleSpecular (SurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			half3 h = normalize (lightDir + viewDir);
			
			fixed diff = max (0, dot (s.Normal, lightDir));
			
			float nh = max (0, dot (s.Normal, h));
			//float spec = pow (nh, s.Specular*128.0) * s.Gloss;
			float spec = pow (nh, s.Specular*128.0);
			
			fixed4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb * spec) * (atten * 2);
			c.a = 1.0; // s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
			return c;
		}
		ENDCG
	} 
	FallBack "Specular"
}
