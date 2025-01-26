Shader "Unlit/ShineEffect"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _ShineColor ("Shine Color", Color) = (1,1,1,1)
        _ShineWidth ("Shine Width", Range(0.01, 0.5)) = 0.2
        _ShineSpeed ("Shine Speed", Range(0.1, 5.0)) = 1.0
        _CycleDuration ("Cycle Duration", Range(1.0, 5.0)) = 2.0 // Time for one full cycle (cover + uncover)
        _ShineInterval ("Shine Interval", Range(0.1, 2.0)) = 0.5 // Time between each shine beam
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
            float _ShineInterval;
            
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
                diagonal = diagonal * 0.5;

                // Animate the shine position over time
                float time = frac(_Time.y * _ShineSpeed / _CycleDuration); // Cycle time for covering and uncovering

                // Control the time intervals
                float shinePhase = floor(time / _ShineInterval);  // Divide time into intervals
                float shinePosition = frac(time);  // The position of the shine within the current interval

                // Determine if the current time is within a "shining" interval
                float shineMask = step(shinePosition - _ShineWidth, diagonal) * (1.0 - step(shinePosition + _ShineWidth, diagonal));
                
                // Ensure that the shine occurs only during the right intervals
                shineMask *= step(shinePhase, 1.0); // Show shine only during certain phases

                // Get the sprite texture
                fixed4 baseColor = tex2D(_MainTex, uv);

                // Apply the shine color (with alpha from the texture)
                fixed4 shineColor = _ShineColor * shineMask * baseColor.a;

                // Combine the base texture with the shine effect
                return baseColor + shineColor;
            }
            ENDCG
        }
    }
}
