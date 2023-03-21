using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Level8 : MonoBehaviour
{
    [SerializeField] private Mesh _instanceMesh;
    [SerializeField] private Material _instanceMaterial;
    [SerializeField] private ComputeShader _compute;
    [SerializeField] private Transform _pusher;

    private int _count;
    private ComputeBuffer _argsBuffer;
    private readonly uint[] _args = { 0, 0, 0, 0, 0 };

    private int _kernel;

    private void Start()
    {
        _kernel = _compute.FindKernel("cs_main");
        _count = SceneTools.GetCount;

        _argsBuffer = new ComputeBuffer(1, _args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        UpdateBuffers();

        SceneTools.Instance.SetCountText(_count);
        SceneTools.Instance.SetNameText("GPU Instancing Indirect Interaction");
    }

    private void Update()
    {
        _compute.SetVector("pusher_position", _pusher.position);
        _compute.Dispatch(_kernel, Mathf.CeilToInt(_count / 64f), 1, 1);

        Graphics.DrawMeshInstancedIndirect(_instanceMesh, 0, _instanceMaterial, new Bounds(Vector3.zero, Vector3.one * 1000), _argsBuffer);
    }

    private void UpdateBuffers()
    {
        var offset = Vector3.zero;
        var data = new MeshData[_count];
        for (var i = 0; i < _count; i++)
        {
            var pos = Random.insideUnitSphere.normalized * Random.Range(10, 50) + offset;
            var rot = Quaternion.Euler(Random.insideUnitSphere.normalized);

            data[i] = new MeshData
            {
                BasePos = pos,
                Mat = Matrix4x4.TRS(pos, rot, SceneTools.CubeScale),
                Amount = 0
            };
        }

        _meshPropertiesBuffer = new ComputeBuffer(_count, 80);
        _meshPropertiesBuffer.SetData(data);

        _compute.SetBuffer(_kernel, "data", _meshPropertiesBuffer);
        _instanceMaterial.SetBuffer("data", _meshPropertiesBuffer);

        // Verts
        _args[0] = _instanceMesh.GetIndexCount(0);
        _args[1] = (uint)_count;
        _args[2] = _instanceMesh.GetIndexStart(0);
        _args[3] = _instanceMesh.GetBaseVertex(0);

        _argsBuffer.SetData(_args);
    }

    private ComputeBuffer _meshPropertiesBuffer;

    private void OnDisable()
    {
        _argsBuffer?.Release();
        _argsBuffer = null;
    }

    private struct MeshData
    {
        public float3 BasePos;
        public Matrix4x4 Mat;
        public float Amount;
    }
}