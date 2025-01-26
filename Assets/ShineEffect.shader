Shader "Unlit/ShineEffect"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _ShineWidth ("Shine Width", Range(0.01, 0.5)) = 0.2
        _ShineSpeed ("Shine Speed", Range(0.1, 5.0)) = 1.0
        
        _CycleDuration ("Cycle Duration", Range(1.0, 5.0)) = 2.0 // Time for one full cycle (cover + uncover)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ShineColor;
            float _ShineWidth;
            float _ShineSpeed;
            float _CycleDuration;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float diagonal = uv.x + uv.y;
                diagonal = diagonal*0.5;

                // Animate the shine position over time
                float time = frac(_Time.y * _ShineSpeed / _CycleDuration); // Cycle time for covering and uncovering

                // Determine if we're covering or uncovering
                float shinePosition;
                if (time < 0.5)
                {
                    // Cover the sprite from bottom-left to top-right
                    shinePosition = time * 2.0; // Line moves from 0 to 1
                }
                else
                {
                    // Uncover the sprite from bottom-left to top-right
                    shinePosition = 1.0 - ((time - 0.5) * 2.0); // Line moves from 1 to 0
                }

                // Shine mask calculation that moves diagonally
                float shineMask = 1.0 - smoothstep(shinePosition - _ShineWidth, shinePosition, diagonal) *
                                  smoothstep(shinePosition, shinePosition + _ShineWidth, diagonal);

                
                // Hämta sprite-texturen
                fixed4 baseColor = tex2D(_MainTex, uv);

                // Begränsa effekten till alfamasken
                fixed4 shineColor = _ShineColor * shineMask * baseColor.a;

                // Kombinera texturen med linjen
                return baseColor + shineColor;
            }
            ENDCG
        }
    }
}
