Shader "Unlit/Indicator-Line"
{
    Properties
    {
        _Timer ("Timer", Range(0,1)) = 0.5

        _OutlineWidth ("Outline Width", Range(0,1)) = 0.06

        _BaseColor ("Base color", Color) = (1, 0, 0.23, 1)
        _Brightness ("Brightness", Float) = 8
        _Opacity ("Opacity", Range(0,1)) = 1
        _BaseOpacity ("Base Opacity", Range(0,1)) = 0.02

        [Header(Rendering)][Space(5)]
        [Enum(UnityEngine.Rendering.CullMode)] _Culling ("Cull Mode", Int) = 2
        [Enum(Off,0,On,1)] _ZWrite("ZWrite", Int) = 0 // Off
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Int) = 4 // LEqual

        [Header(Blending)][Space(5)]
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc ("Blend mode Source", Int) = 5 // SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] _BlendDst ("Blend mode Destination", Int) = 1 // One
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline" = "UniversalPipeline"}
        Cull [_Culling]
        ZWrite [_ZWrite]
        ZTest [_ZTest]
        Blend [_BlendSrc] [_BlendDst]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                half3 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varying
            {
                float4 positionCS : SV_POSITION;
                half3 positionOS : TEXCOORD0;
                half3 objectScale : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half _Brightness;
                half _Opacity;
                half _BaseOpacity;

                half _OutlineWidth;

                half _Timer;
            CBUFFER_END

            Varying vert (Attributes IN)
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                Varying OUT = (Varying)0;
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionOS = IN.positionOS.xyz * 2;

                half3x3 m = (half3x3)UNITY_MATRIX_M;
                OUT.objectScale = half3(
                length( half3( m[0][0], m[1][0], m[2][0] ) ),
                length( half3( m[0][1], m[1][1], m[2][1] ) ),
                length( half3( m[0][2], m[1][2], m[2][2] ) )
                );
                return OUT;
            }

            half4 frag (Varying IN) : SV_Target
            {
                float timer = saturate(_Timer);

                half3 p = abs(IN.positionOS);
                half2 outlineWidth = (half2)_OutlineWidth / IN.objectScale.xy; // Keep outline width
                outlineWidth = 1 - outlineWidth;
                half outline = saturate(step(outlineWidth.x, p.x) + step(outlineWidth.y, p.y));

                half d = IN.positionOS.y * 0.5 + 0.5;
                half indicator = step(d, timer) * saturate(d * d * d + (1 - timer));

                half4 color;
                color.rgb = _BaseColor.rgb * _Brightness;
                color.a = saturate(outline + indicator + _BaseOpacity) * _BaseColor.a * saturate(_Opacity);
                return color;
            }
            ENDHLSL
        }
    }
}