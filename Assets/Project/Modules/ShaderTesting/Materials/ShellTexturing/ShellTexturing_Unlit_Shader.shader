Shader "Unlit/ShellTexturing_Unlit_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _Color("Color", Color) = (1,1,1,1)
        _Resolution("Resolution", Range(1, 200)) = 50
        _Height01("Height01", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            #include "../../../ShaderLibraries/Shaders/MyShaderLibraries/HashFunctions.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };




            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;
            float _Resolution;
            float _Height01;

            

            v2f vert (appdata v)
            {
                v2f o;

                const float MAX_DISPLACEMENT = 0.75f;
                float displacement = MAX_DISPLACEMENT * _Height01;
                
                float4 worldPosition = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                worldPosition.xyz += worldNormal * displacement;

                o.vertex = UnityWorldToClipPos(worldPosition);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 finalColor = fixed4(0,0,0,1);

                float2 scaledUV = i.uv * _Resolution;
                float hash = hash12(scaledUV);

                float2 localUV = scaledUV % 1.0f;      
                float2 centeredLocalUV = (localUV * 2.0f) - 1.0f;
                float centerLength = length(centeredLocalUV)    ;

                if (hash < _Height01)
                {
                    discard;
                }
                else
                {
                    float2 localUV = scaledUV % 1.0f;      
                    float2 centeredLocalUV = (localUV * 2.0f) - 1.0f;
                    float centerLength = length(centeredLocalUV);
                    finalColor.xyz = centerLength;    
                    //finalColor = _Color * lerp(0.2f, 1.0f, _Height01);            
                }

                return finalColor;
            }
            ENDCG
        }
    }
}
