Shader "Custom/KeyboardShader"
{
    Properties
    {
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1)
        _Roughness("Roughness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _Ambient("Ambient", Range(0, 1)) = 0.2
    }

    CGINCLUDE
#include "UnityCG.cginc"
#include "Lighting.cginc"
#define PI 3.14159265359f
#define INV_PI 0.31830988618f

    float4 _Color;
    float _Roughness;
    float _Metallic;
    float _Ambient;

    struct Attributes
    {
        float4 vertex   : POSITION;
        float3 normal   : NORMAL;
        float2 texcoord : TEXCOORD0;
    };

    struct Varyings
    {
        float4 pos      : SV_POSITION;
        float2 uv       : TEXCOORD0;
        float3 worldPos : TEXCOORD1;
        float3 normal   : TEXCOORD2;
        float3 viewDir  : TEXCOORD3;
        float3 lightDir : TEXCOORD4;
    };

    float GGXG1(float3 normal, float3 w, float a2)
    {
        if (dot(normal, w) <= 0)
        {
            return 0;
        }
        else
        {
            float tanTheta = tan(acos(dot(normal, w)));
            float G1 = 2 / (1 + sqrt(1 + a2 * tanTheta * tanTheta));
            return G1;
        }
    }

    float GGXD(float3 normal, float3 halfVec, float a2)
    {
        float NdotH = saturate(dot(normal, halfVec));
        if (NdotH == 0)
        {
            return 0;
        }
        float tanHalfVec = tan(acos(NdotH));
        float invD = 1 / (NdotH * NdotH * (a2 + tanHalfVec * tanHalfVec));
        float D = a2 * invD * invD;
        return D;
    }

    float3 GGXBRDF(float3 normal, float3 wi, float3 wo, float3 kd, float3 ks, float roughness)
    {
        float3 halfVec = normalize(wi + wo);
        float powTerm = pow(1 - max(0, dot(wi, halfVec)), 5);
        float3 F = ks + (1 - ks) * powTerm;

        float a2 = roughness * roughness;
        float Gin = GGXG1(normal, wi, a2);
        float Gout = GGXG1(normal, wo, a2);
        
        float D = GGXD(normal, halfVec, a2);
        float WdotN = abs(dot(wi, normal) * dot(wo, normal));
        
        float3 returnBRDF = 0.25 * F * Gin * Gout * D / WdotN + kd;
        return returnBRDF;
    }

    Varyings keyboardVert(Attributes v)
    {
        Varyings o;
        o.pos = UnityObjectToClipPos(v.vertex.xyz);
        o.uv = v.texcoord;
        o.worldPos = mul(unity_ObjectToWorld, v.vertex);
        o.normal = UnityObjectToWorldNormal(v.normal);
        o.viewDir = WorldSpaceViewDir(v.vertex);
        o.lightDir = WorldSpaceLightDir(v.vertex);
        return o;
    }

    float4 keyboardFrag(Varyings i) : SV_TARGET
    {
        float3 worldPos = i.worldPos;
        float3 normal = normalize(i.normal);
        float3 wi = normalize(i.viewDir);
        float3 wo = normalize(i.lightDir);

        float3 mainTex = _Color.xyz;
        float3 kd = mainTex * unity_ColorSpaceDielectricSpec.a * (1 - _Metallic);
        float3 ks = lerp(unity_ColorSpaceDielectricSpec.rgb, mainTex, _Metallic);

        float roughness = _Roughness;

        float NdotL = saturate(dot(normal, wo));
        float3 brdf = GGXBRDF(normal, wi, wo, kd, ks, roughness);

        float3 finalColor = brdf * _LightColor0 * NdotL + _Ambient * kd;
        return float4(finalColor, 1);
    }


    ENDCG


    SubShader
    {
        pass {
            CGPROGRAM
            #pragma vertex keyboardVert
            #pragma fragment keyboardFrag
            ENDCG
        }
    }
    FallBack "Diffuse"
}
