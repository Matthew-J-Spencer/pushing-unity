using Unity.Mathematics;
using UnityEngine;

public class Level4 : MonoBehaviour
{
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    private float[] _cubeYOffsets;
    private Matrix4x4[] _matrices;

    private Vector3[] _positions;
    private RenderParams _rp;

    private void Start()
    {
        var count = SceneTools.GetCount;
        _positions = new Vector3[count];
        _cubeYOffsets = new float[count];
        _matrices = new Matrix4x4[_positions.Length];

        SceneTools.LoopPositions((i, p) =>
        {
            _cubeYOffsets[i] = p.y;
            _positions[i] = p;
        });

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