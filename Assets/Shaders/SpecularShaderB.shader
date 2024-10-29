Shader "Custom/Specular Shader B"{
    Properties{
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SpecColor ("SpecularsColor", Color) = (1.0, 1.0, 1.0, 1.0)
        _Shininess ("Shine", Float) = 3.0
    }
    SubShader{
        Tags{ "LightMode" = "UniversalForward"}
        Pass{
            CGPROGRAM
            //pragmas
            #pragma vertex vert
            #pragma fragment frag
            //user defined variables
            uniform float4 _Color;
            uniform float4 _SpecColor;
            uniform float _Shininess;
            //unity defined variables
            uniform float4 _LightColor0;
            //Structs
            struct vertexInput{
                float4 vertex : POSITION;
                float3  normal : NORMAL;
            };
            struct vertexOutput{
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            //vertex function
            vertexOutput vert(vertexInput v){
                vertexOutput o;
                
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.normalDir = normalize(mul(float4(v.normal, 1.0), unity_ObjectToWorld).xyz);
                o.pos = UnityObjectToClipPos(v.vertex);

                return o;
            }
            //fragment functions
            float4 frag(vertexOutput i) : COLOR{

                //vectors
                float3 normalDirection = i.normalDir;
                float3 viewDirection = normalize(float3(float4(_WorldSpaceCameraPos.xyz, 1.0) - i.posWorld.xyz));                
                float3 lightDirection;
                float atten = 1.0;

                //Lighting
                lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 diffuseReflection = atten * _LightColor0.xyz * max(0.0, dot(normalDirection, lightDirection)) * _Color.rgb;
                float3 specularReflection = _SpecColor * max(0.0, dot(normalDirection, lightDirection)) * pow(max(0.0, dot(reflect(-lightDirection, normalDirection) , viewDirection)), _Shininess);
                float3 lightFinal = diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT.xyz;

                return float4(lightFinal * _Color.rgb, 1.0);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}