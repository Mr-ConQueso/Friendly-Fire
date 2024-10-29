Shader "Unlit/TextureShader"
{
    Properties
    {
        _Color ("Color Tint", 2D) = "white" {}
        _MainTex ("Texture", 2D) = "white" {}
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
        _Shininess ("Shininess", float ) = 0.078125
        _RimLightColor ("RimLightColor", Color) = (1.0, 1.0, 1.0, 1.0)
        _RimLightPower ("RimLightPower", Range(0.1, 10.0)) = 3.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
            Tags { "LightMode"="UniversalForward" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //user defined variables
            uniform float4 _Color;
            uniform float4 _SpecColor;
            uniform float _Shininess;
            uniform float4 _RimLightColor;
            uniform float _RimLightPower;
            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            //unity defined variables
            uniform float4 _LightColor0;

            //structs
            struct vertexInput{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord : TEXCOORD0;
            };
            struct vertexOutput{
                float4 pos : SV_POSITION;
                float4 tex : TEXCOORD0;
                float3 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            //vertex functions
            vertexOutput vert(vertexInput v){
                vertexOutput o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.tex = float4(v.texcoord.xy, 0.0, 0.0);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.normalDir = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);   
                return o;
            }
            //fragment functions
            float4 frag(vertexOutput i) : COLOR{
                //vectors
                float3 normalDirection = normalize(i.normalDir);
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.posWorld);
                float3 lightDirection;
                float attenuation;

                if(_WorldSpaceLightPos0.w == 0.0){
                    attenuation = 1.0;
                    lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                }
                else{
                    float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld;
                    float distance = length(fragmentToLightSource);
                    attenuation = 1.0 /distance;
                    lightDirection = normalize(fragmentToLightSource);
                }
                //lighting
                float3 diffuseReflection = attenuation * _LightColor0 * saturate(dot(normalDirection, lightDirection));
                float3 specularReflection = diffuseReflection * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDir)), _Shininess) * _SpecColor.rgb;

                //Rim Lighting
                float3 rim = 1.0 - saturate(dot(viewDir, normalDirection));
                float3 rimLight = saturate(dot(normalDirection, lightDirection) * _LightColor0 * _RimLightColor * pow(rim, _RimLightPower));
                float3 lightFinal = diffuseReflection + specularReflection + rimLight + unity_AmbientSky.rgb;

                //texture
                float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
                return float4(tex.xyz * lightFinal * _Color.xyz, 1.0);  
            }
            ENDCG
        }
    }
}