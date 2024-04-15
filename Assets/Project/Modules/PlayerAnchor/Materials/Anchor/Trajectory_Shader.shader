Shader "Unlit/Trajectory_Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WaveColor ("Wave Color", Color) = (1,0,0,1)
        _TipColor ("Tip Color", Color) = (1,0,0,1)
        _WaveSpeed ("Wave Speed", Range(-10, 10)) = 1
        _WaveFrequency ("Wave Frequency", Range(0, 10)) = 1
        _WaveSharpness("Wave Sharpness", Range(0, 10)) = 1
        _StartTime("Start Time", Range(0, 200)) = 1
        _WaveDirection("Wave Direction", Range(0, 1)) = 0
        _WaveDuration("Wave Duration", Range(0, 10)) = 1
        _TimeOverwrite("Time Overwrite", Range(-1, 10)) = 1

        _TipPosition ("Tip Position", Vector) = (0,0,0)
        _TipLength("Tip Length", Range(0, 10)) = 4
        _TipIsVisible("Tip Is Visible", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        Cull off

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
                float3 objectPosition : TEXCOORD1;
                float3 worldPosition : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _WaveColor, _TipColor;
            float _WaveDirection, _WaveSpeed, _WaveFrequency, _WaveSharpness, _WaveDuration;
            float _StartTime, _TimeOverwrite;
            float3 _TipPosition;
            float _TipLength, _TipIsVisible;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
                o.objectPosition = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float waveDirection = lerp(i.uv.y, i.uv.x, _WaveDirection);


                float d = length(i.worldPosition - _TipPosition);
                d = min(d, _TipLength);
                d /= _TipLength;
                d = pow(d, 10);
                d = 1 - d;
                d *= _TipIsVisible;

                fixed4 endColor = lerp(_WaveColor, _TipColor, d);

                float time = _Time.y - _StartTime + (0.4f * _WaveDuration);
                time = lerp(_TimeOverwrite, time, step(_TimeOverwrite, -0.1));

                

                float x = ((waveDirection * _WaveDuration * _WaveFrequency + time * _WaveSpeed) % _WaveDuration) / _WaveDuration;
                endColor.w = pow(abs(x), _WaveSharpness);

                return endColor;
            }
            ENDCG
        }
    }
}
