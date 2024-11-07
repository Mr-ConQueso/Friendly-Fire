Shader "Custom/DitheringShader"
{
    Properties
    {
        _ScreenWidth("Screen Width", int) = 1980
        _ScreenHeight("Screen Height", int) = 1080
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
            int _ScreenWidth;
            int _ScreenHeight;
            
            //structs
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
            //vertex functions
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            //bayer MATRIX

            static const float bayerMatrix[16] ={
                0.0, 8.0, 2.0, 10.0,
                12.0, 4.0, 14.0, 6.0,
                3.0, 11.0, 1.0, 9.0,
                15.0, 7.0, 13.0, 5.0
            };
            //fragment function
            float4 frag (v2f i) : COLOR
            {

                // Sample the texture normally to check if it's working
                float3 color = tex2D(_MainTex, i.uv);
                
                int x = int(i.uv.x * _ScreenWidth) & 3;
                int y = int(i.uv.y *  _ScreenHeight) & 3;
                int index = y * 4 + x;
                
                // Get threshold from the Bayer matrix and normalize it
                float threshold = bayerMatrix[index] / 16.0 - 0.5;

                // Compare grayscale value with the threshold to create dithering effect
                float3 outputColor = round(color*4.0+ threshold)/4.0; //>0.5  ? 1.0 : 0.0;
                
                return float4(outputColor, 1.0);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
