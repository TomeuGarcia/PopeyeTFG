Shader "Unlit/CircularUV_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _ColorInside ("Color Inside", Color) = (1,0,0,1)
        _ColorOutside ("Color Outside", Color) = (0,1,0,0.5)
        _ColorMixSharpness("Color Mix Sharpness", Range(0.001, 5)) = 1        
        _FadeSharpness("Fade Sharpness", Range(0.001, 5)) = 1        
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha


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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

                float2 centeredUV : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _ColorInside, _ColorOutside;
            float _ColorMixSharpness, _FadeSharpness;

            v2f vert (appdata v)
            {
                v2f o;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);


                o.centeredUV = ((o.uv - float2(0.5f, 0.5f))*2.0f);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = length(i.centeredUV);

                if (t > 1) discard;

                fixed4 finalColor = lerp(_ColorInside, _ColorOutside, pow(t, _ColorMixSharpness));
                finalColor.a = pow(1-t, _FadeSharpness);
                

                return finalColor;
            }
            ENDCG
        }
    }
}
