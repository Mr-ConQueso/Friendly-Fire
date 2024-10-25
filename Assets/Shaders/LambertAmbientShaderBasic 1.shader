Shader "Custom/LambertShaderBasic"{
    Properties{
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader{
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //user defined variables
            uniform float4 _Color;
            

            //unity defined variables
            uniform float4 _LightColor0;
            
            //base input structurs
            struct vertexInput{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct vertexOutput{
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };
            //vertex function
            vertexOutput vert(vertexInput input){
                vertexOutput o;
                
                float3 normalDirection = normalize( mul(float4(input.normal, 0.0), unity_WorldToObject).xyz);
                float3 lightDirection;
                float atten = 1.0;  
                
                lightDirection =  normalize(_WorldSpaceLightPos0.xyz);

                float3 diffuseReflection = atten * _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));

                o.color = float4(diffuseReflection, 1.0);
                o.pos = UnityObjectToClipPos(input.vertex);
                return o;
            }
            //fragment function
            float4 frag(vertexOutput output) : COLOR{
                return output.color;
            }
            ENDCG
            
        }
    }
    Fallback "Diffuse"
}