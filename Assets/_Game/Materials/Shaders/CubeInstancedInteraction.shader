Shader "Tarodev/CubeInstancedInteraction"
{
    Properties
    {
        _InteractionRadius("Interaction Radius",float) = 30
        _InactiveColor("Inactive color", Color) = (.2, .2, .2, 1)
        _ActiveColor("Active color", Color) = (1, .7, .0, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _InactiveColor;
            float4 _ActiveColor;
            float _InteractionRadius;

            struct mesh_data
            {
                float3 basePos;
                float4x4 mat;
                float amount;
            };

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
            };

            struct v2_f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            StructuredBuffer<mesh_data> data;

            v2_f vert(const appdata_t i, const uint instance_id: SV_InstanceID)
            {
                v2_f o;

                const float4 pos = mul(data[instance_id].mat, i.vertex);
                o.vertex = UnityObjectToClipPos(pos);
                o.color = lerp(_InactiveColor, _ActiveColor, data[instance_id].amount);

                return o;
            }

            fixed4 frag(v2_f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}