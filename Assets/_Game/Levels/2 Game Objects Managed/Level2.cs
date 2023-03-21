using UnityEngine;

public class Level2 : MonoBehaviour
{
    [SerializeField] private Transform _cubePrefab;

    private Transform[] _spawnedCubes;
    private float[] _cubeYOffsets;

    private void Start()
    {
        var count = SceneTools.GetCount;
        _spawnedCubes = new Transform[count];
        _cubeYOffsets = new float[count];

        var i = 0;
        for (int y = 0; y < SceneTools.Depth; y++)
        {
            for (var x = 0; x < SceneTools.SIDE_LENGTH; x++)
            {
                for (var z = 0; z < SceneTools.SIDE_LENGTH; z++)
                {
                    _cubeYOffsets[i] = y;
                    _spawnedCubes[i++] = Instantiate(_cubePrefab, new Vector3(x, y, z), Quaternion.identity, transform);
                }
            }
        }

        SceneTools.Instance.SetCountText(count);
        SceneTools.Instance.SetNameText("Managed Cubes");
    }

    private void Update()
    {
        HandleStandard();
    }

    private void HandleStandard()
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