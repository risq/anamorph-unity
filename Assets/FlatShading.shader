// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:1,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:9361,x:33985,y:32560,varname:node_9361,prsc:2|emission-8709-OUT,custl-4050-OUT,olwid-3905-OUT,olcol-5050-RGB;n:type:ShaderForge.SFN_LightVector,id:9696,x:32270,y:32555,varname:node_9696,prsc:2;n:type:ShaderForge.SFN_ViewVector,id:9127,x:32270,y:32684,varname:node_9127,prsc:2;n:type:ShaderForge.SFN_Dot,id:8429,x:32449,y:32555,varname:node_8429,prsc:2,dt:0|A-9696-OUT,B-9127-OUT;n:type:ShaderForge.SFN_Multiply,id:3709,x:32641,y:32555,varname:node_3709,prsc:2|A-8429-OUT,B-6559-OUT;n:type:ShaderForge.SFN_Vector1,id:6559,x:32449,y:32718,varname:node_6559,prsc:2,v1:0.3;n:type:ShaderForge.SFN_Add,id:1879,x:32858,y:32555,varname:node_1879,prsc:2|A-3709-OUT,B-6559-OUT;n:type:ShaderForge.SFN_Power,id:8610,x:33040,y:32555,varname:node_8610,prsc:2|VAL-1879-OUT,EXP-4705-OUT;n:type:ShaderForge.SFN_Color,id:9029,x:33040,y:32403,ptovrint:False,ptlb:Diff Color,ptin:_DiffColor,varname:_DiffColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.3823529,c3:0.4718052,c4:1;n:type:ShaderForge.SFN_Multiply,id:385,x:33242,y:32555,varname:node_385,prsc:2|A-8610-OUT,B-9029-RGB;n:type:ShaderForge.SFN_Color,id:9406,x:32862,y:33009,ptovrint:False,ptlb:Fres Color,ptin:_FresColor,varname:_FresColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.3602941,c2:0,c3:0.2310852,c4:1;n:type:ShaderForge.SFN_Fresnel,id:4602,x:32859,y:33178,varname:node_4602,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7264,x:33039,y:33120,varname:node_7264,prsc:2|A-9406-RGB,B-4602-OUT,C-4476-OUT;n:type:ShaderForge.SFN_Vector1,id:4476,x:32859,y:33339,varname:node_4476,prsc:2,v1:2;n:type:ShaderForge.SFN_OneMinus,id:2520,x:33039,y:32984,varname:node_2520,prsc:2|IN-3322-OUT;n:type:ShaderForge.SFN_LightAttenuation,id:3322,x:33039,y:32751,varname:node_3322,prsc:2;n:type:ShaderForge.SFN_Multiply,id:3924,x:33236,y:32984,varname:node_3924,prsc:2|A-2520-OUT,B-7264-OUT;n:type:ShaderForge.SFN_Add,id:4194,x:33436,y:32555,varname:node_4194,prsc:2|A-385-OUT,B-3924-OUT;n:type:ShaderForge.SFN_LightColor,id:9308,x:33427,y:32863,varname:node_9308,prsc:2;n:type:ShaderForge.SFN_Multiply,id:4050,x:33627,y:32735,varname:node_4050,prsc:2|A-4194-OUT,B-9308-RGB,C-3322-OUT;n:type:ShaderForge.SFN_Color,id:5050,x:33646,y:33015,ptovrint:False,ptlb:Outline Color,ptin:_OutlineColor,varname:_OutlineColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:0,c3:0,c4:0.603;n:type:ShaderForge.SFN_Slider,id:3905,x:33603,y:32890,ptovrint:False,ptlb:Outline Width,ptin:_OutlineWidth,varname:_OutlineWidth,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.01,max:1;n:type:ShaderForge.SFN_Slider,id:4705,x:32701,y:32744,ptovrint:False,ptlb:Brightness,ptin:_Brightness,varname:_Brightness,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:10;n:type:ShaderForge.SFN_Slider,id:8709,x:33627,y:32659,ptovrint:False,ptlb:Emission,ptin:_Emission,varname:node_8709,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;proporder:9029-9406-5050-3905-4705-8709;pass:END;sub:END;*/

Shader "Shader Forge/FlatShading" {
    Properties {
        _DiffColor ("Diff Color", Color) = (1,0.3823529,0.4718052,1)
        _FresColor ("Fres Color", Color) = (0.3602941,0,0.2310852,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,0.603)
        _OutlineWidth ("Outline Width", Range(0, 1)) = 0.01
        _Brightness ("Brightness", Range(0, 10)) = 0
        _Emission ("Emission", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _OutlineColor;
            uniform float _OutlineWidth;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_FOG_COORDS(0)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = mul(UNITY_MATRIX_MVP, float4(v.vertex.xyz + v.normal*_OutlineWidth,1) );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                return fixed4(_OutlineColor.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _DiffColor;
            uniform float4 _FresColor;
            uniform float _Brightness;
            uniform float _Emission;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
////// Emissive:
                float3 emissive = float3(_Emission,_Emission,_Emission);
                float node_6559 = 0.3;
                float3 finalColor = emissive + (((pow(((dot(lightDirection,viewDirection)*node_6559)+node_6559),_Brightness)*_DiffColor.rgb)+((1.0 - attenuation)*(_FresColor.rgb*(1.0-max(0,dot(normalDirection, viewDirection)))*2.0)))*_LightColor0.rgb*attenuation);
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _DiffColor;
            uniform float4 _FresColor;
            uniform float _Brightness;
            uniform float _Emission;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
                LIGHTING_COORDS(2,3)
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float node_6559 = 0.3;
                float3 finalColor = (((pow(((dot(lightDirection,viewDirection)*node_6559)+node_6559),_Brightness)*_DiffColor.rgb)+((1.0 - attenuation)*(_FresColor.rgb*(1.0-max(0,dot(normalDirection, viewDirection)))*2.0)))*_LightColor0.rgb*attenuation);
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
