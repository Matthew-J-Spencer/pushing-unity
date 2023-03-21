using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class Level5 : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;

    private NativeArray<float3> _nativePositions;
    private NativeArray<float> _nativeCubeYOffsets;
    private NativeArray<Matrix4x4> _nativeMatrices;
    private CubePositionJob _job;
    private RenderParams _rp;

    private void Start()
    {
        var count = SceneTools.GetCount;

        _nativePositions = new NativeArray<float3>(count, Allocator.Persistent);
        _nativeMatrices = new NativeArray<Matrix4x4>(count, Allocator.Persistent);
        _nativeCubeYOffsets = new NativeArray<float>(count, Allocator.Persistent);

        var i = 0;
        for (var y = 0; y < SceneTools.Depth; y++)
        {
            for (var x = 0; x < SceneTools.SIDE_LENGTH; x++)
            {
                for (var z = 0; z < SceneTools.SIDE_LENGTH; z++)
                {
                    _nativeCubeYOffsets[i] = y;
                    _nativePositions[i++] = new float3(x, 0, z);
                }
            }
        }

        _job = new CubePositionJob
        {
            Positions = _nativePositions,
            YOffsets = _nativeCubeYOffsets
        };

        _rp = new RenderParams(_material);

        SceneTools.Instance.SetCountText(count);
        SceneTools.Instance.SetNameText("GPU Instancing with Jobs + Burst");
    }


    private void Update()
    {
        _job.Matrix = _nativeMatrices;
        _job.Time = Time.time;
        _job.Schedule(_nativeMatrices.Length, 64).Complete();

        Graphics.RenderMeshInstanced(_rp, _mesh, 0, _nativeMatrices);
    }

    private void OnDestroy()
    {
        _nativePositions.Dispose();
        _nativeMatrices.Dispose();
        _nativeCubeYOffsets.Dispose();
    }
}

[BurstCompile]
internal struct CubePositionJob : IJobParallelFor
{
    public NativeArray<float3> Positions;
    [ReadOnly] public NativeArray<float> YOffsets;
    public NativeArray<Matrix4x4> Matrix;
    public float Time;

    public void Execute(int index)
    {
        var (pos, rot) = Positions[index].CalculatePos(YOffsets[index], Time);

        Positions[index] = pos;
        Matrix[index] = Matrix4x4.TRS(pos, rot, SceneTools.CubeScale);
    }
}