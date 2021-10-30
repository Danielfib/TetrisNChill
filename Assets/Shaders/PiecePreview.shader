Shader "Unlit/PiecePreview"
{
    Properties
    {
        [HDR]_PreviewColor("Preview Color", Color) = (0.3, 0.3, 0.3, 0.3)
    }
    SubShader
    {
        Tags { 
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha // Traditional transparency
            //Blend One OneMinusSrcAlpha // Premultiplied transparency
            //Blend One One // Additive
            //Blend OneMinusDstColor One // Soft additive
            //Blend DstColor Zero // Multiplicative
            //Blend DstColor SrcColor // 2x multiplicative

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

            float4 _PreviewColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _PreviewColor;
            }
            ENDCG
        }
    }
}
