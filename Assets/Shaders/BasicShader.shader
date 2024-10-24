// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/BasicShader"{
    Properties{
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader{
        Pass{
            CGPROGRAM
            //pragma
            #pragma vertex vert
            #pragma fragment frag
            //user defined variables
            uniform float4 _Color;
            //base input structurs
            struct vertexInput{
                float4 vertex : POSITION;
            };
            struct vertexOutput{
                float4 pos : SV_POSITION;
            };   
            //vertex function
            vertexOutput vert(vertexInput input){
                vertexOutput o;
                o.pos =  UnityObjectToClipPos(input.vertex);
                return o;
            }
            //fragment function
            float4 frag(vertexOutput output) : COLOR{
                return _Color;
            }
            
            ENDCG
        }
    }
    Fallback "Diffuse"
}