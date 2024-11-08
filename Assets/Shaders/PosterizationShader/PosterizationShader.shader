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

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Declare texture and sampler
            sampler2D _MainTex;
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
            float3 posterize (float3 c, float s)
			{
				return floor(c*s)/(s - 1);
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                // Sample the texture normally to check if it's working
                float3 color = tex2D(_MainTex, i.uv);
                float3 colorOut = posterize(color, _Levels);    
                return float4(colorOut, 1.0);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
