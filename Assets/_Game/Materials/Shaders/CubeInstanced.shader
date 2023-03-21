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
            //   #include "UnityCG.cginc"
            //  #include "UnityLightingCommon.cginc"
            //   #include "AutoLight.cginc"

            float4 _FarColor;

            StructuredBuffer<float4> position_buffer_1;
            StructuredBuffer<float4> position_buffer_2;
            float4 color_buffer[32];

            struct Attributes
            {
                float3 normal : NORMAL;
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float3 ambient : TEXCOORD1;
                float3 diffuse : TEXCOORD2;
                float3 color : TEXCOORD3;
            };

            Varyings vert(Attributes v, const uint instance_id : SV_InstanceID)
            {
                float4 start = position_buffer_1[instance_id];
                float4 end = position_buffer_2[instance_id];

                const float t = (sin(_Time.y + start.w) + 1) / 2;

                const float3 world_start = start.xyz + v.vertex.xyz;
                const float3 world_end = end.xyz + v.vertex.xyz;

                const float3 pos = lerp(world_start, world_end, t);
                const float3 color = lerp(color_buffer[end.w], _FarColor, t);

                Varyings o;
                o.vertex = mul(UNITY_MATRIX_VP, float4(pos, 1.0f));
                  o.ambient = float3(0.5,.1,1);// SampleSH9();// SampleSH9(float4(v.normal, 1.0f));
                  o.diffuse = (saturate(dot(v.normal, float3(0,1,0))) * float3(1,1,1));
               o.ambient = float3(1,1,1) * 0.1f;
                 o.diffuse = float3(1,1,1)* 0.1f;
                o.color = color;


                return o;
            }

            half4 frag(const Varyings i) : SV_Target
            {
                const float3 lighting = i.diffuse * 15 + i.ambient;
                return half4(i.color * lighting, 1);;
            }
            ENDHLSL
        }
    }
}

//{
//    Properties
//    {
//        _FarColor("Far color", Color) = (.2, .2, .2, 1)
//    }
//    SubShader
//    {
//        Pass
//        {
//            Tags
//            {
//                "RenderType"="Opaque"
//                "RenderPipeline" = "UniversalRenderPipeline"
//            }
//
//            HLSLPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            #pragma multi_compile_instancing
//            
//          
//            #include "UnityCG.cginc"
//            #include "UnityLightingCommon.cginc"
//            #include "AutoLight.cginc"
//
//            float4 _FarColor;
//
//            StructuredBuffer<float4> position_buffer_1;
//            StructuredBuffer<float4> position_buffer_2;
//            float4 color_buffer[32];
//
//            struct v2_f
//            {
//                float4 vertex : SV_POSITION;
//                float3 ambient : TEXCOORD1;
//                float3 diffuse : TEXCOORD2;
//                float3 color : TEXCOORD3;
//                
//            };
//
//            v2_f vert(appdata_base v, const uint instance_id : SV_InstanceID)
//            {
//                float4 start = position_buffer_1[instance_id];
//                float4 end = position_buffer_2[instance_id];
//
//                const float t = (sin(_Time.y + start.w) + 1) / 2;
//
//                const float3 world_start = start.xyz + v.vertex.xyz;
//                const float3 world_end = end.xyz + v.vertex.xyz;
//
//                const float3 pos = lerp(world_start, world_end, t);
//                const float3 color = lerp(color_buffer[end.w], _FarColor, t);
//
//                v2_f o;
//                o.vertex = mul(UNITY_MATRIX_VP, float4(pos, 1.0f));
//                o.ambient = ShadeSH9(float4(v.normal, 1.0f));
//                o.diffuse = (saturate(dot(v.normal, _WorldSpaceLightPos0.xyz)) * _LightColor0.rgb);
//                o.color = color;
//
//
//                return o;
//            }
//
//            fixed4 frag(const v2_f i) : SV_Target
//            {
//                const float3 lighting = i.diffuse * SHADOW_ATTENUATION(i) + i.ambient;
//                return fixed4(i.color * lighting, 1);;
//            }
//            ENDHLSL
//        }
//    }
//}