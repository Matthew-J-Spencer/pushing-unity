using UnityEngine;

public class Level2 : MonoBehaviour
{
    [SerializeField] private Transform _cubePrefab;
    private float[] _cubeYOffsets;

    private Transform[] _spawnedCubes;

    private void Start()
    {
        var count = SceneTools.GetCount;
        _spawnedCubes = new Transform[count];
        _cubeYOffsets = new float[count];
        
        SceneTools.LoopPositions((i, p) =>
        {
            _cubeYOffsets[i] = p.y;
            _spawnedCubes[i] = Instantiate(_cubePrefab, p, Quaternion.identity, transform);
        });

        SceneTools.Instance.SetCountText(count);
        SceneTools.Instance.SetNameText("Managed Cubes");
    }

    private void Update()
    {
        var time = Time.time;
        for (var i = 0; i < _spawnedCubes.Length; i++)
        {
            var cube = _spawnedCubes[i];

            var (pos, rot) = cube.position.CalculatePos(_cubeYOffsets[i], time);
            cube.SetPositionAndRotation(pos, rot);
        }
    }
}