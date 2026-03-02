Shader "Custom/CRTEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineIntensity ("Scanline Intensity", Range(0, 1)) = 0.08
        _ScanlineCount ("Scanline Count", Float) = 300
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.4
        _FlickerSpeed ("Flicker Speed", Float) = 5.0
        _FlickerIntensity ("Flicker Intensity", Range(0, 0.05)) = 0.015
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            sampler2D _MainTex;
            float _ScanlineIntensity;
            float _ScanlineCount;
            float _VignetteIntensity;
            float _FlickerSpeed;
            float _FlickerIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Scanlines
                float scanline = sin(i.uv.y * _ScanlineCount * 3.14159) * 0.5 + 0.5;
                col.rgb -= scanline * _ScanlineIntensity;

                // Vignette
                float2 center = i.uv - 0.5;
                float vignette = 1.0 - dot(center, center) * _VignetteIntensity * 2.5;
                col.rgb *= saturate(vignette);

                // Flicker
                float flicker = 1.0 + sin(_Time.y * _FlickerSpeed) * _FlickerIntensity;
                col.rgb *= flicker;

                return col;
            }
            ENDCG
        }
    }
}
