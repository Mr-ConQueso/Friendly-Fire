Shader "Custom/URPPosterizationShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Levels ("Posterization Levels", Float) = 4.0
    }

    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "PosterizationEffect"

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
                noperspective float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            float3 posterize (float3 c, float s)
			{
				return floor(c*s)/(s-1);
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                // Sample the texture normally to check if it's working
                float3 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).rgb;
                float3 colorOut = posterize(color, _Levels)
                return float4(colorTex, 1.0);
            }
            ENDHLSL
        }
    }
    Fallback "Diffuse"
}
