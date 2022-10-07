Shader "Atari/PixelShader" {
    Properties {
        _MainTex ("Texture", 2D)         = "white" {}
        _Pixels("Resolution", int)       = 512
        _PixelWidth("PixelWidth", int)   = 64
        _PixelHeight("PixelHeight", int) = 64
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
            };

            float _Pixels;
            float _PixelWidth;
            float _PixelHeight;
            
            sampler2D _MainTex;
            float4 _MainTex_ST;

            VS_Out vert(VS_In v) {
                VS_Out o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = v.uv;
                return o;
            }

            half4 frag(VS_Out i) : SV_Target {
                // pixelize the screen
                float dx = _PixelWidth  * (1.f / _Pixels);
                float dy = _PixelHeight * (1.f / _Pixels);
                
                float x = dx * floor(i.uv.x / dx);
                float y = dy * floor(i.uv.y / dy);
                float2 coord = float2(x, y);



                // simulate the Atari colour palette
                half4 col = tex2D(_MainTex, coord);
                float m = 16.0 / 256.0;
                col.r = ceil(col.r * 255.0 / 16.0) * m;
                col.g = ceil(col.g * 255.0 / 16.0) * m;
                col.b = ceil(col.b * 255.0 / 16.0) * m;
                return col;
            }
            ENDCG
        }
    }
}
