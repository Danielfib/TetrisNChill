Shader "Unlit/Grid"
{
    Properties
    {
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
                if (distance(xDist, 1) < _Thickness || distance(xDist, 0) < _Thickness 
                    || distance(yDist, 1) < _Thickness || distance(yDist, 0) < _Thickness) {
                    return _GridColor;
                }
                else {
                    return _SpaceColor;
                }
            }
            ENDCG
        }
    }
}
