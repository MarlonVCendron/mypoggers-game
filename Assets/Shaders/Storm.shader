Shader "Custom/Storm" {
    Properties {
        _MainTex ("Skybox Texture", Cube) = "white" {}
        _TopColor ("Top Color", Color) = (0.3, 0.3, 0.3, 1)
        _BottomColor ("Bottom Color", Color) = (0.1, 0.1, 0.1, 1)
        _GradientHeight ("Gradient Height", Range(0, 1)) = 0.5
        _NoiseScale ("Noise Scale", Range(0.1, 10)) = 1
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags { "Queue"="Background" "RenderType"="Background" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0

        samplerCUBE _MainTex;
        float4 _TopColor;
        float4 _BottomColor;
        float _GradientHeight;
        float _NoiseScale;
        float _NoiseIntensity;

        struct Input {
            float3 worldNormal;
        };

        // Simple 3D noise function
        float3 _Simplex3D(float3 P) {
            // Permutation table
            int3 perm = int3(151, 160, 137) % 12;

            // Skew and unskew factors
            float F3 = 1.0/3.0;
            float G3 = 1.0/6.0;

            // Skew the input space
            float s = dot(P, float3(F3, F3, F3));
            int3 Pi = floor(P + s);

            // Unskew the cell origin back to world space
            float t = dot(Pi, float3(G3, G3, G3));
            float3 P0 = Pi - t;

            // The simplex cell
            int3 i1 = (P0.x > P0.y) ? int3(1, 0, 0) : int3(0, 1, 0);
            int3 i2 = (P0.x > P0.z) ? int3(1, 0, 0) : int3(0, 0, 1);

            // Offsets for remaining corners
            float3 x1 = P - P0 + G3;
            float3 x2 = P - P0 - i1 + 2.0 * G3;
            float3 x3 = P - P0 - i2 + 3.0 * G3;
            float3 x4 = P - P0 - 1.0 + 4.0 * G3;

            // Gradients
            float4 g = perm * 0.0243902439 - 0.146341463;

            // Accumulate noise contributions from four corners
            float4 t0 = 0.6 - float4(dot(x0, x0), dot(x1, x1), dot(x2, x2), dot(x3, x3));
            t0 = max(t0, 0.0);
            return dot(t0 * t0 * t0 * t0, g);
        }

        void surf (Input IN, inout SurfaceOutput o) {
            float3 worldNormal = normalize(IN.worldNormal);
            float gradient = saturate((worldNormal.y + _GradientHeight) * 0.5);
                    // Generate noise based on the world normal
        float noise = (_Simplex3D(worldNormal * _NoiseScale) + 1) * 0.5 * _NoiseIntensity;

        // Calculate color from the gradient and noise
        float4 col = lerp(_BottomColor, _TopColor, gradient) + noise;

        // Sample the skybox texture and mix it with the calculated color
        float4 skyboxColor = texCUBE(_MainTex, worldNormal);
        o.Albedo = lerp(skyboxColor.rgb, col.rgb, col.a);
        o.Specular = 0;
        o.Gloss = 0;
        o.Emission = 0;
        o.Alpha = 1;
    }
    ENDCG
}
FallBack "Diffuse"
}

