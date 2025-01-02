Shader "Hidden/Drawing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DrawingTex ("Drawing", 2D) = "white" {}
        _Threshold1 ("Threshold1", Float) = 0
        _Threshold2 ("Threshold2", Float) = 0
        _Threshold3 ("Threshold3", Float) = 0
        _Threshold4 ("Threshold4", Float) = 0

        _Magni1 ("Magni1", Float) = 0
        _Magni2 ("Magni2", Float) = 0
        _Magni3 ("Magni3", Float) = 0
        _Magni4 ("Magni4", Float) = 0

        _Xdiff ("Xdiff", Float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _DrawingTex;
            float _Threshold1;
            float _Threshold2;
            float _Threshold3;
            float _Threshold4;
            float _Magni1;
            float _Magni2;
            float _Magni3;
            float _Magni4;

            float _Xdiff;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // 2. ピクセルの明度を計算
                half brightness = dot(col, half3(0.299, 0.587, 0.114));
                
                // 3. 明度に応じてパーリンノイズのテクスチャに係数をかけて色をピックアップ
                fixed4 drawingCol = tex2D(_DrawingTex, float2(i.uv.x + _Xdiff * i.uv.y, i.uv.y));
                if(brightness >= 0.9){
                    drawingCol = 1;
                } else if (brightness >= _Threshold1){
                    drawingCol *= _Magni1;
                } else if (brightness >= _Threshold2){
                    drawingCol *= _Magni2;
                } else if (brightness >= _Threshold3){
                    drawingCol *= _Magni3;
                }else if(brightness >= _Threshold4){
                    drawingCol *= _Magni4;
                }
                return drawingCol;
            }
            ENDCG
        }
    }
}
