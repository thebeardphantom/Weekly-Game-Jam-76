Shader "CUSTOM/Desaturation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Desaturation("Desaturation", Range(0.0, 1.0)) = 1.0
		_Vignette("Vignette", float) = 1.0
		_VignettePow("VignettePow", float) = 1.0
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

            sampler2D _MainTex;
			float _Desaturation;
			float _Vignette;
			float _VignettePow;

			float4 Desaturate(float3 color, float Desaturation)
			{
				float3 grayXfer = float3(0.3, 0.59, 0.11);
				float value = dot(grayXfer, color);
				float3 gray = float3(value, value, value);
				return float4(lerp(color, gray, Desaturation), 1.0);
			}
            
			v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb = Desaturate(col.rgb, _Desaturation);
				float4 screen = ComputeScreenPos(i.vertex);
				float dst = distance(i.uv, float4(0.5, 0.5, 0.0, 0.0));
                return lerp(col, float4(0.0, 0.0, 0.0, 1.0), pow(dst * _Vignette, _VignettePow));
            }
            ENDCG
        }
    }
}
