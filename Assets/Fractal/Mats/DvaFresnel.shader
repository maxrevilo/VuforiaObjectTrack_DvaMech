// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CustomShader/Rim Light" {
 
    Properties {
        _Color ("Rim Color", Color) = (1, 1, 1, 1)
        _Power ("Power", float) = 1.5
        _Scale ("Scale", float) = 1.0
        _Bias ("Bias", float) = 0
    }
   
 
 
    SubShader {
		
        Tags { "RenderType" = "Overlay" "IgnoreProjector"="True"  "Queue" = "Transparent"}
        Cull back
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend one One
     
        Pass {      
         
            CGPROGRAM
             
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct vIN {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 texcoord : TEXCOORD0;
                };
             
                struct vOUT {
                    float4 pos : SV_POSITION;
                    float4 R : COLOR;
                };
             
                uniform float4 _Color;
                uniform float _Power;
                uniform float _Scale;
                uniform float _Bias;
             
				vOUT vert(vIN v)
				{
					vOUT o;
					o.pos = UnityObjectToClipPos(v.vertex);

					float3 posWorld = mul(unity_ObjectToWorld, v.vertex).xyz;
					float3 normWorld = normalize(mul(v.normal, unity_WorldToObject));

					float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
					o.R = _Bias + _Scale * pow(1.0 + dot(I, normWorld), _Power);

					return o;
				}

				float4 frag(vOUT i) :  COLOR
				{  
					float4 col = float4(0.0, 0.0, 0.0, 0.0);
					return lerp(col, _Color, i.R);
				}
             
            ENDCG
        }
    }
}