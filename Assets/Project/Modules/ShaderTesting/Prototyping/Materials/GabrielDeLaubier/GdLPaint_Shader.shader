Shader "Unlit/GdLPaint_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _NormalValue("Normal Value", Range(0, 10)) = 1

        _NormalTex ("Normal Texture", 2D) = "white" {}
        _BaseTex ("Base Texture", 2D) = "white" {}

        _OffsetScale("Offset Scale", Range(0, 1)) = 0.05

        _DiffuseGradient01("Diffuse Gradient 1", Color) = (0.9, 0.9, 0.9, 1)
        _DiffuseGradient02("Diffuse Gradient 2", Color) = (0.7, 0.9, 0.7, 1)

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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                half4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD1;
                float3 worldPos : TEXCOORD2;

                float3 normalWS : TEXCOORD3;
                float4 tangentWS : TEXCOORD4;
            };

            sampler2D _MainTex, _BaseTex, _NormalTex;
            float4 _MainTex_ST;
            float _OffsetScale, _NormalValue;

            fixed4 _DiffuseGradient01, _DiffuseGradient02;


            float2 ParallaxDepth(float2 uv, float3 viewDir, float offsetScale)
            {
                return uv + (viewDir.xy / viewDir.z) * offsetScale;
            }

            float3 ViewDir(float3 worldPos)
            {
                return normalize(worldPos - _WorldSpaceCameraPos);
            }

            float2 BumpOffset(float2 uv, float3 viewDir, float height, float heightScale)
            {
                float2 p = viewDir.xy / viewDir.z * (height * heightScale);
                return uv - p;   
            }

            float2 ParallaxMapping(sampler2D depthMap, float2 texCoords, float3 viewDir, float heightScale)
            { 
                float height =  tex2D(depthMap, texCoords).r;    
                float2 p = viewDir.xy / viewDir.z * (height * heightScale);
                return texCoords - p;    
            } 




            float3x3 CreateTangentToWorld(float3 normal, float3 tangent, float flipSign)
            {
                // For odd-negative scale transforms we need to flip the sign
                float sgn = flipSign;
                float3 bitangent = cross(normal, tangent) * sgn;
 
                return float3x3(tangent, bitangent, normal);
            }
            float3 TransformTangentToWorld(float3 normalTS, float3x3 tangentToWorld)
            {
                // Note matrix is in row major convention with left multiplication as it is build on the fly
                float3 result = mul(normalTS, tangentToWorld);
 
                return normalize(result);
            }

            float3 NormalMap(float3 normalWS, float2 uv, sampler2D normalTex, float4 tangentWS, float normalValue)
            {
                normalWS = normalize(normalWS);
                half3 normalTS = UnpackNormal(tex2D(normalTex, uv));
                float3x3 tangentToWorld = CreateTangentToWorld(normalWS, tangentWS.xyz * normalValue, tangentWS.w);
                normalWS = TransformTangentToWorld(normalTS, tangentToWorld);  
                return normalWS;
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = ViewDir(worldPos);

                o.worldPos = worldPos;

                o.normalWS = UnityObjectToWorldNormal(v.normal);
                o.tangentWS = float4(UnityObjectToWorldNormal(v.tangent.xyz), v.tangent.w);


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //sampler2D bumpMap = _BaseTex;
                //fixed4 paintTextureColor = tex2D(bumpMap, i.uv);
                //float2 uv = BumpOffset(i.uv, i.viewDir, paintTextureColor.z, _OffsetScale);
                //paintTextureColor = tex2D(_BaseTex, uv);

                fixed4 paintTextureColor = tex2D(_BaseTex, i.uv);

                float3 normal = NormalMap(i.normalWS, i.uv, _NormalTex, i.tangentWS, _NormalValue);
                float3 lightDirection = _WorldSpaceLightPos0.xyz;

                float d = dot(normal, lightDirection);
                d += 0.2f;
                d = round(d);
                d += 0.25f;
                d = saturate(d);



                fixed4 col = lerp(_DiffuseGradient01, _DiffuseGradient02, paintTextureColor.x*4);
                col *= d;

                return col;
            }
            ENDCG
        }
    }
}
