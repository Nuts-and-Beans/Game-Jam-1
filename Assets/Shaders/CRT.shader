// TODO(Zack): make this look better, currently looks pretty basic, and not particularly "CRT" like
Shader "Atari/CRT" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _VertsColour("Verts fill Colour", Range(0.0, 1.0))    = 0.0
        _VertsColour2("Verts fill Colour 2", Range(0.0, 1.0)) = 0.0
        _Contrast("Contrast", Float) = 0.0
        _Brightness("Brightness", Float) = 0.0
        _ScanColour("Scanline colour", Float) = 0.0
    }
    
    SubShader {
        Cull Off ZWrite Off ZTest Always

        Pass { 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct VS_In {
                float4 pos : POSITION;
                float2 uv  : TEXCOORD0;
            };

            struct VS_Out {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                float4 scr_pos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float _VertsColour;
            uniform float _VertsColour2;
            uniform float _Contrast;
            uniform float _Brightness;
            uniform float _ScanColour;

            VS_Out vert(VS_In v) {
                VS_Out o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv  = TRANSFORM_TEX(v.uv, _MainTex);
                o.scr_pos = ComputeScreenPos(o.pos);
                return o;
            }

            
            float4 frag(VS_Out i) : SV_Target {
                float2 ps = i.scr_pos.xy * _ScreenParams.xy / i.scr_pos.w;
                int pp = (int)ps.x % 3;

                float4 col  = tex2D(_MainTex, i.uv);
                float4 muls = float4(0.f, 0.f, 0.f, 1.f);

                // NOTE(Zack): depending on if the pixel position [pp] is [1, 2, 3] we set the pixel colour for the [rgb] effect of a CRT
                if (pp == 1) {
                    muls.r = _VertsColour;
                    muls.g = _VertsColour2;
                } else if (pp == 2) {
                    muls.g = _VertsColour;
                    muls.b = _VertsColour2;
                } else {
                    muls.b = _VertsColour;
                    muls.r = _VertsColour2;
                }

                // NOTE(Zack): every 3rd line we add a scan line
                if ((int)ps.y % 3 == 0) {
                    muls *= float4(_ScanColour, _ScanColour, _ScanColour, 1.f);
                }

                col *= muls;
                col += (_Brightness / 255.f);
                col  = col - _Contrast * (col - 1.f) * col * (col - 0.5);
                return col;
            }
            ENDCG
        }
    }
}
