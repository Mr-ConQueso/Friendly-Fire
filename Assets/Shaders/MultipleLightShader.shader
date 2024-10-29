Shader "Custom/MultipleLight"{
    Properties{
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SpecColor ("SpecularsColor", Color) = (1.0, 1.0, 1.0, 1.0)
        _Shininess ("Shine", Float) = 3
        _RimLightColor ("RimLightColor", Color) = (1.0, 1.0, 1.0, 1.0)
        _RimLightPower ("RimLightPower", Range(0.1, 10.0)) = 3.0
    }
    SubShader{
        Tags{ 
            "RenderType" = "Opaque"
        }

        Pass{
            Tags{ "LightMode" = "UniversalForward" }
            Name "ForwardBase"
            CGPROGRAM
            //pragmas
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            //user defined variables
            uniform float4 _Color;
            uniform float4 _SpecColor;
            uniform float _Shininess;
            uniform float4 _RimLightColor;
            uniform float _RimLightPower;
            //unity defined variables
            uniform float4 _LightColor0;

            //Structs
            struct vertexInput{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct vertexOutput{
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            //vertex function
            vertexOutput vert(vertexInput v){
                vertexOutput o;
                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            //fragment functions
            float4 frag(vertexOutput i) : COLOR{
                //vectors
                float3 normalDirection = i.normalDir;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float atten = 1.0;
                //Lighting
                float3 diffuseReflection =  atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));
                float3 specularReflection = atten * saturate(dot(normalDirection, lightDirection)) * pow(saturate(dot(reflect(-lightDirection, normalDirection) , viewDirection)), _Shininess) * _SpecColor.xyz;
                //rim light
                float3 rim = 1 - dot(normalize(viewDirection), normalDirection);
                float3 rimLight = atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection)) * pow(rim, _RimLightPower) * _RimLightColor.xyz;
                float3 lightFinal = rimLight + diffuseReflection + specularReflection + unity_AmbientSky.xyz;
                return float4(lightFinal * _Color.xyz, 1.0);
            }
            ENDCG
        }
        
        Pass{
            Tags{ "LightMode" = "UniversalForwardAdd" }
            Name "ForwardAdd"
            Blend One One
            CGPROGRAM
            //pragmas
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdadd_fullshadows
            //user defined variables
            uniform float4 _Color;
            uniform float4 _SpecColor;
            uniform float _Shininess;
            uniform float4 _RimLightColor;
            uniform float _RimLightPower;
            //unity defined variables
            uniform float4 _LightColor0;

            //Structs
            struct vertexInput{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct vertexOutput{
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            //vertex function
            vertexOutput vert(vertexInput v){
                vertexOutput o;
                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            //fragment functions
            float4 frag(vertexOutput i) : COLOR{
                //vectors
                float3 normalDirection = i.normalDir;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz );
                float atten = 1.0;
                //Lighting
                float3 diffuseReflection = atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));
                float3 specularReflection = atten * saturate(dot(normalDirection, lightDirection)) * pow(saturate(dot(reflect(-lightDirection, normalDirection) , viewDirection)), _Shininess);
                //rim light
                float3 rim = 1 - dot(normalize(viewDirection), normalDirection);
                float3 rimLight = atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection)) * pow(rim, _RimLightPower) * _RimLightColor.xyz;
                float3 lightFinal = rimLight + diffuseReflection + specularReflection;
                return float4(lightFinal * _Color.xyz, 1.0);
            }
            ENDCG
        }
            
    }
    Fallback "Specular"
}