Shader "Custom/URPPosterizationShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Levels ("Posterization Levels", Float) = 4.0
    }

    SubShader
    {

        Pass
        {
            Name "PosterizationEffect"
            Tags { "LightMode" = "UniversalForward" }

            HLSLINCLUDE
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Declare texture and sampler
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float _Levels;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).rgb;

                color = floor(color * _Levels) / _Levels;

                return float4(color, 1.0);
            }
            ENDHLSL
        }
    }
    Fallback "Diffuse"
}
