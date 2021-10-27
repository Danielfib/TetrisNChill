Shader "Unlit/Grid"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        [HDR]_GridColor("Grid Color", Color) = (1, 1, 1, 1)
        _SpaceColor("Space Color", Color) = (0, 0, 0, 0)
        _Thickness("Thickness", Range(0.02, 0.8)) = 0.05
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

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD1;
            };

            struct Interpolator
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _GridColor;
            float4 _SpaceColor;
            float _Thickness;

            Interpolator vert (MeshData v)
            {
                Interpolator o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(Interpolator i) : SV_Target
            {
                float xDist = abs(i.worldPos.x) % 1;
                float yDist = abs(i.worldPos.y) % 1;

                float mx = max(distance(xDist, 1), distance(xDist, 0));
                float my = max(distance(yDist, 1), distance(yDist, 0));
                float m = max(mx, my);
                float mLerped = lerp(-8, 1, m);

                float4 c = float4(_GridColor.xyz * mLerped, _GridColor.w) + tex2D(_MainTex, i.uv);
                //float4 c = lerp(tex2D(_MainTex, i.uv), _GridColor, m);

                return c;
            }
            ENDCG
        }
    }
}
