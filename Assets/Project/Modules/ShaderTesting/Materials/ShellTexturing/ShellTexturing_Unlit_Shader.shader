Shader "Unlit/ShellTexturing_Unlit_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _TopColor("Top Color", Color) = (1,1,1,1)
        _BottomColor("Bottom Color", Color) = (1,1,1,1)
        _Resolution("Resolution", Range(1, 200)) = 50
        _Height01("Height01", Range(0, 1)) = 0.5
        _Thickness("Thickness", Range(0, 10)) = 3.0
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
                float3 worldPosition : TEXCOORD1;
            };




            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _TopColor, _BottomColor;
            float _Resolution;
            float _Height01;
            float _Thickness;

            

            v2f vert (appdata v)
            {
                v2f o;

                const float MAX_HEIGHT_DISPLACEMENT = 0.75f;
                float heightDisplacement = MAX_HEIGHT_DISPLACEMENT * _Height01;
                
                float4 worldPosition = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                worldPosition.xyz += worldNormal * heightDisplacement;
                
                float3 bendDisplacement = normalize(float3(1, -1.5, 0)) * pow(_Height01, 3) * 0.2f;
                worldPosition.xyz += bendDisplacement;

                float3 windDisplacement = float3(0, 0, sin(_Time.y * 0.5f + worldPosition.z * 0.1f)) * pow(_Height01, 2) * 0.15f;
                worldPosition.xyz += windDisplacement;

                o.vertex = UnityWorldToClipPos(worldPosition);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPosition = worldPosition.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 finalColor = fixed4(0,0,0,1);

                //i.uv = i.worldPosition.xz;
                float2 scaledUV = i.uv * _Resolution;
                float hash = hash12(scaledUV);

                float2 localUV = scaledUV % 1.0f;      
                float2 centeredLocalUV = (localUV * 2.0f) - 1.0f;
                float distanceFromCenter = length(centeredLocalUV);
                float rectDistanceFromCenter = max(abs(centeredLocalUV.x),abs(centeredLocalUV.y)) ;

                if (hash > _Height01) // CUBOID
                // if (hash > _Height01 && distanceFromCenter < 1) // CYLINDRICAL
                // if (distanceFromCenter < (hash - _Height01) * _Thickness) // CONICAL
                // if (rectDistanceFromCenter < (hash - _Height01) * _Thickness) // CUBOID CONICAL
                {                    
                    finalColor = lerp(_BottomColor, _TopColor, _Height01);       
                }
                else
                {
                    discard;        
                }

                return finalColor;
            }
            ENDCG
        }
    }
}
