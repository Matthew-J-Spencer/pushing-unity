using UnityEngine;

public class Level3 : MonoBehaviour
{
    [SerializeField] private Transform _cubePrefab;
    private float[] _cubeYOffsets;
    private Vector3[] _lastPositions;

    private Transform[] _spawnedCubes;

    private void Start()
    {
        var count = SceneTools.GetCount;
        _spawnedCubes = new Transform[count];
        _lastPositions = new Vector3[count];
        _cubeYOffsets = new float[count];

        SceneTools.LoopPositions((i, p) =>
        {
            _cubeYOffsets[i] = p.y;
            _lastPositions[i] = p;
            _spawnedCubes[i] = Instantiate(_cubePrefab, p, Quaternion.identity, transform);
        });

        SceneTools.Instance.SetCountText(count);
        SceneTools.Instance.SetNameText("Managed Cubes - Avoid extern");
    }

    private void Update()
    {
        var time = Time.time;
        for (var i = 0; i < _spawnedCubes.Length; i++)
        {
            var (pos, rot) = _lastPositions[i].CalculatePos(_cubeYOffsets[i], time);

            _lastPositions[i] = pos;

            _spawnedCubes[i].SetPositionAndRotation(pos, rot);
        }
    }
}