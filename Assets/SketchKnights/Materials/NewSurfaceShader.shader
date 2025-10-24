Shader "Custom/Rim"
{
    Properties
    {
        _TintColor ("Tint Color", Color) = (0,0.5,1,1)
        _RimColor  ("Rim Color",  Color) = (0,1,1,1)
        _RimPower  ("Rim Power", Range(0,1)) = 0.4
        
        [HDR]
        _EmissionColor ("Emission Color", Color) = (0,0,0,1)
        _EmissionIntensity ("Emission Intensity", Range(0,10)) = 1.0
        _EmissionRimOnly ("Emission Rim Only", Range(0,1)) = 0.0
    }

    SubShader
    {
        // ──────────────────────────────────────────
        // 透過マテリアルだが描画順を「Geometry+1」に変更
        // （不透明のすぐ後、他の透過より先）
        // ──────────────────────────────────────────
        Tags
        {
            "Queue"         = "Geometry+1"
            "RenderType"    = "Transparent"
            "RenderPipeline"= "UniversalPipeline"
        }

        // アルファブレンドは維持しつつ深度を書き込む
        ZWrite On
        Blend  SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "RimPass"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex   vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _TintColor;
            float4 _RimColor;
            float  _RimPower;
            float4 _EmissionColor;
            float  _EmissionIntensity;
            float  _EmissionRimOnly;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 worldPos   : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
            };

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.worldPos   = mul(unity_ObjectToWorld, v.positionOS).xyz;
                o.normalWS   = TransformObjectToWorldNormal(v.normalOS);
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);
                half   rim     = 1.0 - saturate(dot(viewDir, i.normalWS));
                
                // ベースカラー計算（既存のリム効果）
                float4 baseColor = lerp(_TintColor, _RimColor, rim * _RimPower);
                
                // エミッション計算
                float3 emission = _EmissionColor.rgb * _EmissionIntensity;
                
                // エミッションをリムエリアのみに適用するかの制御
                emission = lerp(emission, emission * rim, _EmissionRimOnly);
                
                // 最終カラー = ベースカラー + エミッション
                float3 finalColor = baseColor.rgb + emission;
                
                return float4(finalColor, baseColor.a);
            }
            ENDHLSL
        }
    }
}