Shader "Custom/Debugger"
{
    Properties
    {
        _Color ("Color", Color) = (1.0,1.0,1.0,1.0)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Pass{
            Tags {"LightMode" = "UniversalForward"}
            CGPROGRAM
            // Pragmas
            #pragma vertex vert
            #pragma fragment frag
            // User-defined variables
            sampler2D _MainTex;
            float4 _Color;

            // Structs
            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            // Vertex function
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            // Fragment function
            float4 frag(v2f i) : SV_Target
            {
                // Sample texture and apply color
                float4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
