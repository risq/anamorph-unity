Shader "Custom/Snow" 
{
Properties 
{
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_Shadow("Shadow Texture", 2D) = ""{}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}
	SubShader { 
		Tags { "RenderType"="Opaque" }
		LOD 400
	
	CGPROGRAM
	#pragma surface surf BlinnPhong
	#pragma exclude_renderers flash 

	sampler2D _MainTex;
	sampler2D _Shadow;
	sampler2D _BumpMap;
	fixed4 _Color;
	half _Shininess;

	struct Input 
	{
		float2 uv_MainTex;
		float2 uv_Shadow; 
		float2 uv_BumpMap;
	};

	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);

		fixed3 shadowTex = tex2D(_Shadow, IN.uv_Shadow).rgb;

		o.Albedo = tex.rgb * _Color.rgb * shadowTex;
		o.Gloss = tex.a;
		o.Alpha = tex.a * _Color.a;
		o.Specular = _Shininess;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}

	ENDCG
	}

FallBack "Specular"
}
