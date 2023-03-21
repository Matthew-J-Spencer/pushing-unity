using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Level4 : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;

    private float3[] _positions;
    private Matrix4x4[] _matrices;
    private float[] _cubeYOffsets;
    private RenderParams _rp;

    private void Start()
    {
        var count = SceneTools.GetCount;
        _positions = new float3[count];
        _cubeYOffsets = new float[count];
        _matrices = new Matrix4x4[_positions.Length];

        var i = 0;
        for (var y = 0; y < SceneTools.Depth; y++)
        {
            for (var x = 0; x < SceneTools.SIDE_LENGTH; x++)
            {
                for (var z = 0; z < SceneTools.SIDE_LENGTH; z++)
                {
                    _cubeYOffsets[i] = y;
                    _positions[i++] = new float3(x, 0, z);
                }
            }
        }

        _rp = new RenderParams(_material);

        SceneTools.Instance.SetCountText(count);
        SceneTools.Instance.SetNameText("GPU Instancing");
    }

    private void Update()
    {
        var time = Time.time;
        for (var i = 0; i < _positions.Length; i++)
        {
            var (pos, rot) = _positions[i].CalculatePos(_cubeYOffsets[i], time);

            _matrices[i].SetTRS(pos, rot, SceneTools.CubeScale);

            _positions[i].y = pos.y;
        }

        Graphics.RenderMeshInstanced(_rp, _mesh, 0, _matrices);
    }
}