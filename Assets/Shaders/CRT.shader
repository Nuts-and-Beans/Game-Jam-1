// TODO(Zack): make this look better, currently looks pretty basic, and not particularly "CRT" like
Shader "Atari/CRT" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}       
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



            // https://www.shadertoy.com/view/MsXGD4
            float scanline(float2 uv) {
                return sin(_ScreenParams.y * uv.y * 0.7f - (_Time.y * 0.25f) * 10.f);
            }

            float slowscan(float2 uv) {
                return sin(_ScreenParams.y * uv.y * 0.009f + (_Time.y * 0.25f) * 6.f);
            }
            

            // https://www.shadertoy.com/view/XtlSD7
            float2 crt_curve_uv(float2 uv) {
                uv = uv * 2.f - 1.f;
                float2 offset = abs(uv.yx) / float2(4.f, 3.f);
                uv = uv + uv * offset * offset;
                uv = uv * 0.5f + 0.5f;
                return uv;
            }
            
            float3 draw_vignette(float3 colour, float2 uv) {
                float vignette = uv.x * uv.y * (1.f - uv.x) * (1.f - uv.y);
                vignette = clamp(pow(8.f * vignette, 0.3f), 0.f, 1.f);
                colour *= vignette;
                return colour;
            }

 
            float4 frag(VS_Out i) : SV_Target {
                float2 uv = crt_curve_uv(i.uv);
                float3 col = tex2D(_MainTex, uv);

                // this clamps the edge most regions of the screen to just be black
                if (uv.x < 0.f || uv.x > 1.f || uv.y < 0.f || uv.y > 1.f) {
                    col = float3(0.f, 0.f, 0.f);
                }

                
                col = draw_vignette(col, uv);

                // get the values for the animated scanline effects
                float scan = scanline(uv);
                float slow = slowscan(uv);

                // interpolate between the different scanline effects
                float3 final_col = col;
                col = lerp(col, lerp(scan, slow, 0.25f), 0.5f);
                col *= final_col;
                
                return float4(col, 1.f);
            }
            ENDCG
        }
    }
}
