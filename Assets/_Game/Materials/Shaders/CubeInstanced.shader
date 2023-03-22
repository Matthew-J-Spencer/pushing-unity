Shader "Tarodev/CubeInstanced"
{
    Properties
    {
        _FarColor("Far color", Color) = (.2, .2, .2, 1)
    }
    SubShader
    {
        Pass
        {
            Tags
            {
                "RenderType"="Opaque"
                "RenderPipeline" = "UniversalRenderPipeline"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            float4 _FarColor;

            StructuredBuffer<float4> position_buffer_1;
            StructuredBuffer<float4> position_buffer_2;
            float4 color_buffer[8];

            struct attributes
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct varyings
            {
                float4 vertex : SV_POSITION;
                float3 diffuse : TEXCOORD2;
                float3 color : TEXCOORD3;
            };

            varyings vert(attributes v, const uint instance_id : SV_InstanceID)
            {
                float4 start = position_buffer_1[instance_id];
                float4 end = position_buffer_2[instance_id];

                const float t = (sin(_Time.y + start.w) + 1) / 2;

                const float3 world_start = start.xyz + v.vertex.xyz;
                const float3 world_end = end.xyz + v.vertex.xyz;

                const float3 pos = lerp(world_start, world_end, t);
                const float3 color = lerp(color_buffer[end.w % 8], _FarColor, t);

                varyings o;
                o.vertex = mul(UNITY_MATRIX_VP, float4(pos, 1.0f));
                o.diffuse = saturate(dot(v.normal, _MainLightPosition.xyz));
                o.color = color;
                
                return o;
            }

            half4 frag(const varyings i) : SV_Target
            {
                const float3 lighting = i.diffuse *  1.7;
                return half4(i.color * lighting, 1);;
            }
            ENDHLSL
        }
    }
}