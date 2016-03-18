Shader "VertexAnimation/VertexAnimationDiffuse" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Ratio ("Ratio", Range(0,1)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma glsl_no_auto_normalization
		#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;
		float _Ratio;

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
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
