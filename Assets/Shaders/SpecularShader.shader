Shader "Custom/Specular Shader"{
    Properties{
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SpecColor ("SpecularsColor", Color) = (1.0, 1.0, 1.0, 1.0)
        _Shininess ("Shine", Float) = 3
    }
    SubShader{
        //Tags{ "LightMode" = "ForwardBase"}
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
                float4 col : COLOR;
            };
            //vertex function
            vertexOutput vert(vertexInput v){
                vertexOutput o;
                //vectors
                float3 normalDirection = normalize(mul(float4(v.normal, 1.0), unity_WorldToObject).xyz);
                float3 viewDirection = normalize(float3(float4(_WorldSpaceCameraPos.xyz, 1.0) - mul(unity_ObjectToWorld, v.vertex).xyz));                
                float3 lightDirection;
                float atten = 1.0;

                //Lighting
                lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 diffuseReflection = atten * _LightColor0.xyz * max(0.0, dot(normalDirection, lightDirection));
                float3 specularReflection =  max(0.0, dot(normalDirection, lightDirection)) * pow(max(0.0, dot(reflect(-lightDirection, normalDirection) , viewDirection)), _Shininess);
                float3 lightFinal = diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT;

                o.col = float4(lightFinal * _SpecColor.rgb, 1.0);
                o.pos = UnityObjectToClipPos(v.vertex);

                return o;
            }
            //fragment functions
            float4 frag(vertexOutput i) : COLOR{
                return i.col;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}