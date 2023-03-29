Shader "Custom/FlagShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _TextureSpeed("Texture Speed", Range(0, 1000.0)) = 1
        _WaveSpeed("WaveSpeed", Range(0, 1000.0)) = 1
        _WaveFrequency("WaveFrequency", Range(0, 1000.0)) = 1
        _WaveAmplitude("WaveAmplitude", Range(0, 1000.0)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _TextureSpeed;
            float _WaveSpeed;
            float _WaveFrequency;
            float _WaveAmplitude;

            float3 morph(float3 base)
            {
                base.z += sin(base.x * _WaveFrequency + _Time.y * _WaveSpeed) * _WaveAmplitude;
                return base;
            }
            
            float4 morph(float4 base)
            {
                base.z += sin(base.x * _WaveFrequency + _Time.y * _WaveSpeed) * _WaveAmplitude;
                return base;
            }

            void vert(inout appdata_tan v)
            {
                float3 position = morph(v.vertex);

                // normal recalculation
                float3 bitangent = cross(v.normal, v.tangent.xyz);
                float3 nt = (morph(v.vertex + v.tangent.xyz * 0.01) - position);
                float3 nb = (morph(v.vertex + bitangent * 0.01) - position);

                v.normal = normalize(cross(nt, nb));
                v.vertex = UnityObjectToClipPos(float4(position, v.vertex.w));
            }

            fixed4 frag(appdata_tan v) : SV_Target
            {
                float2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                uv.x += _Time.y * _TextureSpeed;

                float3 LightDirection = normalize(_WorldSpaceLightPos0);
                float3 NormalDirection = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                fixed4 diffuse = max(dot(LightDirection, NormalDirection), 0.0) * tex2D(_MainTex, uv);

                return diffuse;
            }

            ENDCG
        }
    }

    FallBack "Diffuse"
}
