Shader "Custom/Flame"
{
    // 小火苗：打火机/蜡烛用。挂到 Quad 或带 UV 的 Sprite 上，无需贴图即可用；可选贴图做形状遮罩。
    Properties
    {
        [Header(Colors)]
        _ColorBase ("底部颜色 (暗红/橙)", Color) = (0.8, 0.2, 0.05, 1)
        _ColorMid  ("中部颜色 (橙黄)", Color) = (1, 0.5, 0.1, 0.9)
        _ColorTip  ("顶部颜色 (亮黄)", Color) = (1, 0.95, 0.4, 0.5)
        _ColorOuter("外焰/透明边", Color) = (1, 0.6, 0.2, 0)

        [Header(Flicker)]
        _FlickerSpeed ("闪烁速度", Range(0.5, 8)) = 3
        _FlickerScale ("闪烁幅度", Range(0, 0.5)) = 0.15
        _VertexSway   ("顶点摆动幅度", Range(0, 0.3)) = 0.08

        [Header(Shape)]
        _Softness ("边缘柔化", Range(0.1, 1.5)) = 0.8
        _WidthTop ("顶部收窄 (0~1)", Range(0.2, 1)) = 0.4

        [Header(Blend)]
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 5  // SrcAlpha
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 1  // One (additive)
        [Toggle] _Additive ("叠加发光 (打火机/蜡烛)", Float) = 1

        _MaskTex ("形状遮罩 (可选，白=不裁切)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        Cull Off
        ZWrite Off
        Blend [_SrcBlend] [_DstBlend]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _ColorBase, _ColorMid, _ColorTip, _ColorOuter;
            float _FlickerSpeed, _FlickerScale, _VertexSway, _Softness, _WidthTop;
            float _Additive;
            sampler2D _MaskTex;
            float4 _MaskTex_ST;

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);
                float a = hash(i);
                float b = hash(i + float2(1, 0));
                float c = hash(i + float2(0, 1));
                float d = hash(i + float2(1, 1));
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }

            float fbm(float2 p)
            {
                float v = 0.0;
                v += 0.5 * noise(p);
                v += 0.25 * noise(p * 2.0);
                v += 0.125 * noise(p * 4.0);
                return v;
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float flicker : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float2 uv = v.uv;
                float t = _Time.y * _FlickerSpeed;
                float n = fbm(float2(uv.x * 4.0 + t * 0.7, uv.y * 3.0 + t * 0.5));
                float sway = (n - 0.5) * 2.0 * _VertexSway;
                float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
                worldPos.x += sway * (1.0 - uv.y);
                v.vertex = mul(unity_WorldToObject, worldPos);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = uv;
                o.flicker = n;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float t = _Time.y * _FlickerSpeed;
                float n = fbm(float2(uv.x * 5.0 + t, uv.y * 4.0 + t * 0.8));
                float flicker = 1.0 + (n - 0.5) * _FlickerScale;

                float v = uv.y;
                float h = abs(uv.x - 0.5) * 2.0;
                float widthCurve = lerp(1.0, _WidthTop, v);
                float edge = 1.0 - saturate((h - widthCurve) / _Softness);
                edge *= flicker;

                float3 col = lerp(_ColorOuter.rgb, _ColorBase.rgb, smoothstep(0.0, 0.3, v));
                col = lerp(col, _ColorMid.rgb, smoothstep(0.25, 0.55, v));
                col = lerp(col, _ColorTip.rgb, smoothstep(0.5, 0.95, v));
                col *= flicker;

                float alpha = edge * lerp(_ColorMid.a, _ColorTip.a, v);
                alpha *= smoothstep(0.0, 0.15, v) * smoothstep(0.0, 0.2, 1.0 - h);

                float mask = tex2D(_MaskTex, uv).a;
                alpha *= mask;

                return fixed4(col, alpha);
            }
            ENDCG
        }
    }

    Fallback "Unlit/Transparent"
}
