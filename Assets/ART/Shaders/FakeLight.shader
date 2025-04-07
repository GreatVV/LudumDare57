Shader "Asteroids/FakeLight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Power1 ("Power1", Float) = 1
        _Power2 ("Power2", Float) = 1
        _Offset ("Offset", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Blend One One
        ZWrite Off Fog { Mode Off }
	    Lighting Off 
        ZTest On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;
            float _Power1;
            float _Power2;
            float _Offset;

            float2 polarCoordinates(float2 UV, float2 Center, float RadialScale, float LengthScale)
            {
                float2 delta = UV - Center;
                float radius = length(delta) * 2 * RadialScale;
                float angle = atan2(delta.x, delta.y) * 1.0/6.28 * LengthScale;
                return float2(radius, angle);
            }


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float r = pow(polarCoordinates(i.uv, float2(0.5, 0.5), 1, 1).r, _Power1);
                r = clamp(1.0 - r + _Offset, 0, 1);
                r = pow(r, 2.2); //to lineral
                r = pow(r, _Power2);

                float4 color = _Color * r * i.color;
                return color * color.a;
            }
            ENDCG
        }
    }
}
