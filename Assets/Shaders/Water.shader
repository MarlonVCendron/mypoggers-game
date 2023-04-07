Shader "Custom/Water" {
    Properties {
        _Color("Color", Color) = (1, 1, 1, 1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0, 1)) = 0.5
        _Metallic("Metallic", Range(0, 1)) = 0.0
        _BumpMap("Bump Map", 2D) = "bump" {}
        _BumpIntensity("Bump Intensity", Range(0, 1)) = 0.5
        _FresnelStrength("Fresnel Strength", Range(0, 1)) = 0.5
        _ShadowStrength("Shadow Strength", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 100

        CGPROGRAM
        #pragma surface surf BlinnPhong specular

        sampler2D _MainTex;
        sampler2D _BumpMap;
        float _BumpIntensity;
        float _Glossiness;
        float _Metallic;
        float _FresnelStrength;
        float _ShadowStrength;
        float4 _Color;

        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float3 viewDir;
        };

        void surf(Input IN, inout SurfaceOutput o) {
            // Albedo
            o.Albedo = _Color.rgb * tex2D(_MainTex, IN.uv_MainTex).rgb;

            // Bump mapping
            float3 bump = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Normal = bump * _BumpIntensity;

            // Glossiness and Metallic
            o.Specular = _Glossiness;
            o.Gloss = _Metallic;

            // Fresnel reflections
            float fresnel = pow(1 - saturate(dot(o.Normal, IN.viewDir)), 3);
            o.Specular += fresnel * _FresnelStrength;

            // Shadows
            o.Specular *= 1 - _ShadowStrength * (1 - tex2D(_MainTex, IN.uv_MainTex).a);
        }
        ENDCG
    }
    FallBack "Diffuse"
}