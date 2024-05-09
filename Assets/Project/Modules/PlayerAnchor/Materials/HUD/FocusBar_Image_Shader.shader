// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/FocusBar_Image_Shader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}     
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        _FillValue ("Fill Value", Range(0, 1)) = 0.4

        _Stage1Tex ("Stage 1 Texture", 2D) = "white" {}     
        _Stage2Tex ("Stage 2 Texture", 2D) = "white" {}     
        _Stage3Tex ("Stage 3 Texture", 2D) = "white" {}    

        _NoiseAmount ("Noise Amount", Float) = -0.3 
        _NoiseSpeed ("Noise Speed", Float) = -0.2
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex, _NoiseTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            float _FillValue, _NoiseAmount, _NoiseSpeed;
            sampler2D _Stage1Tex, _Stage2Tex, _Stage3Tex;


            fixed4 _ColorInterior, _ColorExterior;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            float sin01(float input)
            {
                return (sin(input) + 1) * 0.5f;
            }

            #define PI 3.14159265359
            #define PI_2 6.28318530718

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 uv = IN.texcoord.xy;                

                float uvPositionOffset = IN.vertex.x * 0.001f;
                float noise = tex2D(_NoiseTex, (uv * 0.5f)  + (_Time.y * _NoiseSpeed) + uvPositionOffset);

                uv += noise * length(uv) * _NoiseAmount;

                half4 colorStage1 = tex2D(_Stage1Tex, uv);
                half4 colorStage2 = tex2D(_Stage2Tex, uv);
                half4 colorStage3 = tex2D(_Stage3Tex, uv);

                half4 color = half4(0,0,0,0);
                color = lerp(color, colorStage1, step(0.01f, _FillValue));
                color = lerp(color, colorStage2, step(0.5f, _FillValue));
                color = lerp(color, colorStage3, step(0.99f, _FillValue));

                color = (color + _TextureSampleAdd) * IN.color;



                #ifdef UNITY_UI_CLIP_RECT                
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
                
                return color;
            }
        ENDCG
        }
    }
}